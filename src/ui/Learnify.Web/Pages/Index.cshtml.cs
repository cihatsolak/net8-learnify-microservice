namespace Learnify.Web.Pages;

public class IndexModel(CatalogService catalogService) : BasePageModel
{
    public List<CourseViewModel> Courses { get; set; } = [];


    public async Task<IActionResult> OnGet()
    {
        var coursesAsResult = await catalogService.GetAllCoursesAsync();
        if (coursesAsResult.IsFail) 
            return ErrorPage(coursesAsResult);

        Courses = coursesAsResult.Data!;

        return Page();
    }
}
