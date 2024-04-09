using AutoMapper;
using ISWBlacklist.Application.DTOs.Auth;
using ISWBlacklist.Application.DTOs.BlacklistedItem;
using ISWBlacklist.Application.DTOs.Item;
using ISWBlacklist.Application.DTOs.User;
using ISWBlacklist.Common.Utilities;
using ISWBlacklist.Domain.Entities;

namespace ISWBlacklist.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Item, BlacklistedItemResponseDto>();
            CreateMap<CreateUserDto, AppUser>();
            CreateMap<AppUser, UserResponseDto>();
            CreateMap<UpdateUserDto, AppUser>();
            CreateMap<AppUser, UpdateUserResponseDto>();
            CreateMap<AppUser, UserResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateModified));
            CreateMap<PageResult<IEnumerable<AppUser>>, PageResult<IEnumerable<UserResponseDto>>>();
            CreateMap<PageResult<IEnumerable<Item>>, PageResult<IEnumerable<BlacklistedItemResponseDto>>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.PerPage, opt => opt.MapFrom(src => src.PerPage))
                .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
                .ForMember(dest => dest.TotalPageCount, opt => opt.MapFrom(src => src.TotalPageCount))
                .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount));
            CreateMap<ItemCreationDto, Item>();
            CreateMap<ItemUpdateDto, Item>();
            CreateMap<Item, ItemResponseDto>();
            CreateMap<PageResult<IEnumerable<Item>>, PageResult<IEnumerable<ItemResponseDto>>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.PerPage, opt => opt.MapFrom(src => src.PerPage))
                .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
                .ForMember(dest => dest.TotalPageCount, opt => opt.MapFrom(src => src.TotalPageCount))
                .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount));
            CreateMap<RegisterUserDto, AppUser>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
