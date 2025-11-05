using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NoteTaking.ViewModel;

namespace NoteTaking.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Notes_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('notetaking.note_seq'::regclass)");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_Notes");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('notetaking.user_seq'::regclass)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<NoteTaking.ViewModel.NoteVM> NoteVM { get; set; } = default!;
}
