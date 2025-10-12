namespace Learnify.Web.Pages.Auth;

public class SignUpModel : PageModel
{
    [BindProperty] //HTML'den gelen veriyi bu property'e baðlar
    public SignUpViewModel SignUpViewModel { get; set; } = SignUpViewModel.Empty;

    public void OnGet()
    {
    }
}
