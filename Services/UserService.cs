using AutoMapper;
using NoteTaking.ViewModel;

namespace NoteTaking.Services
{
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public UserService(IMapper mapper,IWebHostEnvironment env)
        {
            _mapper = mapper;
            _env = env;
        }
        public decimal Add(UserVM entity)
        {
            throw new NotImplementedException();
        }
        public UserVM Login(string username, string password) 
        {
            throw new NotImplementedException();
        }
    }
}
