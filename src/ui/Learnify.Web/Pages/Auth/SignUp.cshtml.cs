namespace Learnify.Web.Pages.Auth;

public class SignUpModel(SignUpService signUpService) : PageModel
{
    [BindProperty] //HTML'den gelen veriyi bu property'e baðlar
    public SignUpViewModel SignUpViewModel { get; set; } = SignUpViewModel.GetExampleModel;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) 
            return Page();

        var result = await signUpService.CreateAccount(SignUpViewModel);
        if (result.IsFail)
        {
            ModelState.AddModelError(string.Empty, result.Fail.Title);

            if (!string.IsNullOrEmpty(result.Fail.Detail))
            {
                ModelState.AddModelError(string.Empty, result.Fail.Detail);
            }

            return Page();
        }

        return RedirectToPage("/Index");
    }
}
