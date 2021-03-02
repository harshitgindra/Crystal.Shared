using AutoMapper;

namespace Samples.EfCore.Web
{
    public class AppMapper: Profile
    {
        public AppMapper()
        {
            CreateMap<BookViewModel, Book>().ReverseMap();
        }
    }
}
