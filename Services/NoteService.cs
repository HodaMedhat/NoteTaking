using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.JSInterop.Implementation;
using NoteTaking.Models;
using NoteTaking.ViewModel;

namespace NoteTaking.Services
{
    public class NoteService
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public NoteService(IMapper mapper, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _env = env;
        }
        public decimal Add(NoteVM entity)
        {
            try
            {
                PostgresContext db = new PostgresContext();
                NoteTaking.Models.Note obj = _mapper.Map<NoteVM, NoteTaking.Models.Note>(entity);

                obj.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                db.Notes.Add(obj);
                db.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "NoteService/Add",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return 0;
            }
        }
        public decimal Update(NoteVM entity)
        {
            try
            {
                PostgresContext db = new PostgresContext();
                var obj = db.Notes.Find(entity.Id);
                if (obj != null)
                {
                    obj.Title = entity.Title;
                    obj.Description = entity.Description;
                    db.SaveChanges();
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "NoteService/Update",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return 0;
            }
        }
        public NoteVM Find(NoteVM entity)
        {
            try
            {
                PostgresContext db = new PostgresContext();
                var obj = db.Notes.Find(entity.Id);
                return _mapper.Map<NoteTaking.Models.Note, NoteVM>(obj);
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "NoteService/Find",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return null;
            }
        }
        public void Delete(decimal id)
        {
            try
            {
                PostgresContext db = new PostgresContext();
                var obj = db.Notes.Find(id);
                if (obj != null)
                {
                    obj.IsDeleted = 1;
                    obj.DeletedAt = DateOnly.FromDateTime(DateTime.Now);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "NoteService/Delete",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
            }
        }

        public void Restore(decimal id)
        {
            try
            {
                PostgresContext db = new PostgresContext();
                var obj = db.Notes.Find(id);
                if (obj != null)
                {
                    obj.IsDeleted = 0;
                    obj.DeletedAt = null;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "NoteService/Restore",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
            }
        }
        public IList<NoteVM> GetAll()
        {
            try
            {
                PostgresContext db = new PostgresContext();
                var objLst = (from x in db.Notes
                              join y in db.Users on x.UserId equals y.Id
                              where x.IsDeleted == 0
                              select new Note
                              {
                                  Id = x.Id,
                                  Title = x.Title,
                                  Description = x.Description,
                                  CreatedAt = x.CreatedAt,
                                  User = y 
                              }).ToList();

                return _mapper.Map<IList<Note>, IList<NoteVM>>(objLst);
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "NoteService/GetAll",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return null;
            }
        }

        public IList<NoteVM> GetUserNotes(decimal id)
        {
            try
            {
                PostgresContext db = new PostgresContext();
                var objLst = (from x in db.Notes
                              where x.UserId == id && x.IsDeleted == 0
                              select x).ToList();

                return _mapper.Map<IList<Note>, IList<NoteVM>>(objLst);
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "NoteService/GetUserNotes",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return null;
            }

        }

        public IList<NoteVM> GetUserTrash(decimal id)
        {
            try
            {
                var cutoff = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));
                PostgresContext db = new PostgresContext();
                var objLst = (from x in db.Notes
                              where x.UserId == id && x.IsDeleted == 1
                              && x.DeletedAt >= cutoff
                              select x).ToList();

                return _mapper.Map<IList<Note>, IList<NoteVM>>(objLst);
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "NoteService/GetUserTrash",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return null;
            }

        }
    }
}
