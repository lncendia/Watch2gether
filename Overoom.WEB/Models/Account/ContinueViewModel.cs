namespace Overoom.WEB.Models.Account;

public class ContinueViewModel
{
    public ContinueViewModel(string returnUrl)
    {
        ReturnUrl = returnUrl;
    }

    public string ReturnUrl { get; }
}