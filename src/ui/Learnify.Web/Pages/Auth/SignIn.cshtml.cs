namespace Learnify.Web.Pages.Auth;

public class SignInModel : PageModel
{
    [BindProperty] 
    public required SignInViewModel SignInViewModel { get; set; } = SignInViewModel.GetExampleModel;

    public void OnGet()
    {
    }
}
