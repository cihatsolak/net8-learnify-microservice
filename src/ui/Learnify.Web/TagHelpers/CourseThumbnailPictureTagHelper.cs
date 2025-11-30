namespace Learnify.Web.TagHelpers;

[HtmlTargetElement("course-thumbnail-picture")]
public class CourseThumbnailPictureTagHelper(MicroserviceOption microserviceOption) : TagHelper
{
    public string Src { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "img";

        if (string.IsNullOrEmpty(Src))
        {
            string blankCourseThumbnailImagePath = "/images/blank_course_thumbnail.jpg";
            output.Attributes.SetAttribute("src", blankCourseThumbnailImagePath);
        }
        else
        {
            string courseThumbnailImagePath = $"{microserviceOption.File.BaseAddress}/{Src}";
            output.Attributes.SetAttribute("src", courseThumbnailImagePath);
        }

        return base.ProcessAsync(context, output);
    }
}
