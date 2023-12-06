using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Models.Book;
using BookStoreApp.API.Models.User;

namespace BookStoreApp.API.Configurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AuthorCreateDto, Author>().ReverseMap();
        CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();
        CreateMap<AuthorUpdateDto, Author>().ReverseMap();
        CreateMap<Book, BookReadOnlyDto>()
            .ForMember(x => x.AuthorName, opt =>
                opt.MapFrom(src => $"{src.Author!.LastName}, {src.Author!.FirstName}"))
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Book, BookDetailsDto>()
            .ForMember(x => x.AuthorName, opt =>
                opt.MapFrom(src => $"{src.Author!.LastName}, {src.Author!.FirstName}"))
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<BookCreateDto, Book>().ReverseMap();
        CreateMap<BookUpdateDto, Book>().ReverseMap();

        CreateMap<UserRegisterDto, ApplicationUser>()
            .ForMember(x => x.UserName, 
                opt => opt.MapFrom(src => src.Email))
            .ForMember(x=>x.NormalizedEmail, opt => 
                opt.MapFrom(src=> src.Email!.ToUpper()))
            .ForMember(x => x.NormalizedUserName, opt =>
                opt.MapFrom(src => src.Email!.ToUpper()))
            .ReverseMap();
    }
}