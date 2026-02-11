namespace NoteTaking.Services
{
    public class NoteCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public NoteCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var noteService = scope.ServiceProvider.GetRequiredService<NoteService>();
                    noteService.DeleteExpiredNotes();
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }

}
