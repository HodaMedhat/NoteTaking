using AutoMapper;
using NoteTaking.Models;
using NoteTaking.ViewModel;

namespace NoteTaking.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserVM>().ReverseMap();
            CreateMap<Note, NoteVM>().ReverseMap();

        }
    }
}
