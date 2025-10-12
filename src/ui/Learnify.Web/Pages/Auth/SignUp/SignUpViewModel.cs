namespace Learnify.Web.Pages.Auth.SignUp;

public sealed record SignUpViewModel
{
    [Display(Name = "First Name:")]
    [Required(ErrorMessage = "First Name is required")]
    [MinLength(3, ErrorMessage = "First Name must be at least 3 characters")]
    [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
    public required string FirstName { get; init; }

    [Display(Name = "Last Name:")]
    [Required(ErrorMessage = "Last Name is required")]
    [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters")]
    [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
    public required string LastName { get; init; }

    [Display(Name = "User Name:")]
    [Required(ErrorMessage = "User Name is required")]
    [RegularExpression(@"^[a-zA-Z0-9._-]{3,20}$",
        ErrorMessage = "User Name can only contain letters, numbers, and . _ - characters")]
    [MinLength(3, ErrorMessage = "User Name must be at least 3 characters")]
    [MaxLength(20, ErrorMessage = "User Name cannot exceed 20 characters")]
    public required string UserName { get; init; }

    [Display(Name = "Email:")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; init; }

    [Display(Name = "Password:")]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must contain upper, lower, number, and special character")]
    public required string Password { get; init; }

    [Display(Name = "Password Confirm:")]
    [Required(ErrorMessage = "Password Confirm is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match")]
    public required string PasswordConfirm { get; init; }

    public static SignUpViewModel Empty => new()
    {
        FirstName = string.Empty,
        LastName = string.Empty,
        UserName = string.Empty,
        Email = string.Empty,
        Password = string.Empty,
        PasswordConfirm = string.Empty
    };

    public static SignUpViewModel GetExampleModel => new()
    {
        FirstName = "Ahmet",
        LastName = "Yildiz",
        UserName = "ahmet.yildiz",
        Email = "ahmet.yildiz@example.com",
        Password = "StrongPass1!",
        PasswordConfirm = "StrongPass1!"
    };
}
