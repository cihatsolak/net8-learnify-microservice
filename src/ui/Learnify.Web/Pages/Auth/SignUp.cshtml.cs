namespace Learnify.Web.Pages.Auth;

public class SignUpModel : PageModel
{
    [BindProperty] //HTML'den gelen veriyi bu property'e ba�lar
    public SignUpViewModel SignUpViewModel { get; set; } = SignUpViewModel.Empty;

    public void OnGet()
    {
    }
}
