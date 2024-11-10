namespace Learnify.Catalog.API.Features.Courses;

public sealed record CourseResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    CategoryResponse Category,
    FeatureResponse Feature
    );

public sealed record FeatureResponse(
    int Duration, 
    float Rating, 
    string EducatorFullName
    );
