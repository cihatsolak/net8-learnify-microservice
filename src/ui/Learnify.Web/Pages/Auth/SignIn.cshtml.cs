namespace Learnify.Web.Pages.Auth;

public class SignInModel(SignInService signInService) : PageModel
{
    [BindProperty] 
    public required SignInViewModel SignInViewModel { get; set; } = SignInViewModel.GetExampleModel;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) 
            return Page();

        var result = await signInService.AuthenticateAsync(SignInViewModel);
        if (result.IsFail)
        {
            ModelState.AddModelError(string.Empty, $"{result.Fail.Title} - {result.Fail.Detail}");

            return Page();
        }

        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnGetSignOutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Index");
    }
}
