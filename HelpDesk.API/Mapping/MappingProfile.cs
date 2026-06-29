using AutoMapper;
using HelpDesk.API.DTO;
using HelpDesk.API.DTOs;
using HelpDesk.API.Models;

namespace HelpDesk.API.Mapping
{
     public class MappingProfile : Profile
     {
          public MappingProfile()
          {
               CreateMap<Ticket, TicketDto>()
                   .ForMember(
                       dest => dest.Category,
                       opt => opt.MapFrom(src => src.Category!.Name))
                   .ForMember(
                       dest => dest.Priority,
                       opt => opt.MapFrom(src => src.Priority!.Name))
                   .ForMember(
                       dest => dest.Status,
                       opt => opt.MapFrom(src => src.Status!.Name))
                   .ForMember(
                       dest => dest.CreatedBy,
                       opt => opt.MapFrom(src => src.Creator!.FullName))
                   .ForMember(
                       dest => dest.AssignedTo,
                       opt => opt.MapFrom(src =>
                           src.Assignee != null
                               ? src.Assignee.FullName
                               : null));

               CreateMap<CreateTicketDto, Ticket>();

               CreateMap<TicketComment, TicketCommentDto>()
                   .ForMember(
                       dest => dest.UserName,
                       opt => opt.MapFrom(src => src.User!.FullName));

               CreateMap<CreateCommentDto, TicketComment>();

               CreateMap<UpdateCommentDto, TicketComment>();

               CreateMap<User, UserDto>();

               CreateMap<CreateUserDto, User>();

               CreateMap<UpdateUserDto, User>();
          }
     }
}