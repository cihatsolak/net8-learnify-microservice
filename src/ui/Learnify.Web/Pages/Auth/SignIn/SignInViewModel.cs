namespace Learnify.Web.Pages.Auth.SignIn;

public sealed record SignInViewModel
{
    [Display(Name = "Email:")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public required string Email { get; init; }

    [Display(Name = "Password:")]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public required string Password { get; init; }

    public static SignInViewModel Empty => new()
    {
        Email = string.Empty,
        Password = string.Empty
    };

    public static SignInViewModel GetExampleModel => new()
    {
        Email = "ahmet.yildiz@example.com",
        Password = "Password12*"
    };
}
