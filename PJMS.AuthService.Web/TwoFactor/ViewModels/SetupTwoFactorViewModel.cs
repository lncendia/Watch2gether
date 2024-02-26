using System.Text;
using System.Web;
using PJMS.AuthService.Web.TwoFactor.InputModels;

namespace PJMS.AuthService.Web.TwoFactor.ViewModels;

public class SetupTwoFactorViewModel : SetupTwoFactorInputModel
{
    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    public SetupTwoFactorViewModel(string authenticatorKey, string name, string projectName)
    {
        AuthenticatorKey = FormatKey(authenticatorKey);
        QrCodeUrl = GenerateQrCodeUrl(name, projectName, authenticatorKey);
    }

    public string QrCodeUrl { get; }

    private static string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        var currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
            currentPosition += 4;
        }

        if (currentPosition < unformattedKey.Length)
            result.Append(unformattedKey[currentPosition..]);

        return result.ToString().ToLowerInvariant();
    }

    private static string GenerateQrCodeUrl(string name, string projectName, string unformattedKey)
    {
        return string.Format(AuthenticatorUriFormat, HttpUtility.UrlEncode(projectName),
            HttpUtility.UrlEncode(name), unformattedKey);
    }
}