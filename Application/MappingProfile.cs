using System.Linq;
using Application.DTO;
using AutoMapper;
using Business;

namespace Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Business.Course, CourseDTO>()
            .ForMember(c => c.Instructors, mp => mp.MapFrom(ci => ci.CourseInstructors.Select(i => i.Instructor).ToList()))
            .ForMember(c => c.Commentaries, com => com.MapFrom(cl => cl.Commentaries))
            .ForMember(c => c.Price, p => p.MapFrom(pr => pr.Price));
            CreateMap<CourseInstructor, CourseInstructorDTO>();
            CreateMap<Business.Instructor, InstructorDTO>();
            CreateMap<Business.Commentary, CommentaryDTO>();
            CreateMap<Price, PriceDTO>();
        }
    }
}