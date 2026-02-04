using AutoMapper;
using ExpenseControl.Application.DTOs.Category;
using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Application.DTOs.Transaction;
using ExpenseControl.Domain.Entities;

namespace ExpenseControl.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Person, PersonDto>();

        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.Purpose, opt => opt.MapFrom(src => src.Purpose.ToString()));

        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person.Name))
            .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Category.Description));
    }
}