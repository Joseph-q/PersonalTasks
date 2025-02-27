using AutoMapper;
using PersonalTasks.Auth.Controller.DTOs.Request;
using PersonalTasks.Models;
using PersonalTasks.Tasks.Controller.DTOs.Request;
using PersonalTasks.Tasks.Controller.DTOs.Response;


namespace PersonalTasks
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //User mapping
            // Hashes the password before mapping it to the PasswordHash field.
            CreateMap<CreateUserRequest, User>()
                   .ForMember(d => d.PasswordHashed, o => o.MapFrom(s => BCrypt.Net.BCrypt.HashPassword(s.Password)));

            //TaskItem mapping
            CreateMap<CreateTaskRequest, TaskItem>();

            //UpdateTaskRequest mapping
            CreateMap<UpdateTaskRequest, TaskItem>()
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(s => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(d => d.Completed, o => o.MapFrom(s => s.IsCompleted ?? false))
                .ForMember(d => d.Title, o => o.MapFrom((s, d) => string.IsNullOrEmpty(s.Title) ? s.Title : d.Title)) // If the title is null or empty, it will keep the original title
                .ForMember(d => d.Description, o => o.MapFrom((s, d) => string.IsNullOrEmpty(s.Description) ? s.Description : d.Description)); // If the description is null or empty, it will keep the original description
            ;

            //Task mapping
            CreateMap<TaskItem, TaskResponse>()
                .ForMember(d => d.IsCompleted, o => o.MapFrom(s => s.Completed))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt.ToString("yyyy-MM-dd")))
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(s => s.UpdatedAt.HasValue ? s.UpdatedAt.Value.ToString("yyyy-MM-dd") : null))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));
        }
    }
}
