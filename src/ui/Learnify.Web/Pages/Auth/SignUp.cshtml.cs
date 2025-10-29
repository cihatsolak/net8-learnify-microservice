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

        var result = await signUpService.CreateAccountAsync(SignUpViewModel);

        if (result.IsFail)
        {
            AddModelErrors(result);
            return Page();
        }

        return RedirectToPage("/Index");
    }

    private void AddModelErrors(ServiceResult result)
    {
        if (string.IsNullOrWhiteSpace(result.Fail?.Title) && string.IsNullOrWhiteSpace(result.Fail?.Detail))
            return;

        if (!string.IsNullOrWhiteSpace(result.Fail.Title))
            ModelState.AddModelError(string.Empty, result.Fail.Title);

        if (!string.IsNullOrWhiteSpace(result.Fail.Detail))
            ModelState.AddModelError(string.Empty, result.Fail.Detail);
    }
}
