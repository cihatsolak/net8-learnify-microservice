namespace Learnify.Catalog.API.Features.Courses;

public class CourseMapping : Profile
{
    public CourseMapping()
    {
        CreateMap<CreateCourseCommand, Course>()
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.Now));
    }
}
