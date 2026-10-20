namespace WorkServices.Infrastructure.Services;

public static class PasswordResetEmailTemplate
{
    public static string Create(
        string fullName,
        string resetLink)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resetLink);

        var safeName = System.Net.WebUtility.HtmlEncode(fullName);
        var safeLink = System.Net.WebUtility.HtmlEncode(resetLink);

        return $"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width, initial-scale=1">
                <title>Reset your Work Services password</title>
            </head>
            <body style="font-family:Arial,sans-serif;color:#222;line-height:1.6">
                <div style="max-width:600px;margin:auto;padding:24px">
                    <h2>Reset your password</h2>
                    <p>Hello {safeName},</p>
                    <p>We received a request to reset your Work Services account password.</p>
                    <p>
                        <a href="{safeLink}"
                           style="display:inline-block;padding:12px 20px;
                                  background:#2563eb;color:#fff;
                                  text-decoration:none;border-radius:6px">
                            Reset Password
                        </a>
                    </p>
                    <p>This link expires in 30 minutes and can only be used once.</p>
                    <p>If you did not request a password reset, you can ignore this email.</p>
                    <p>Work Services</p>
                </div>
            </body>
            </html>
            """;
    }
}