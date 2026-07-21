namespace WorkServices.Infrastructure.Services
{
    public static class EmailTemplates
    {
        public static string ConfirmAccount(string name, string confirmationLink)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Confirm Your Account -WORK SERVICES </title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }}
        .container {{
            max-width: 500px;
            width: 100%;
            background: white;
            border-radius: 20px;
            box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
            padding: 40px 30px;
            text-align: center;
        }}
        .logo {{
            font-size: 28px;
            font-weight: 700;
            color: white;
            margin-bottom: 8px;
            letter-spacing: 1px;
        }}
        .logo span {{
            color: #f39c12;
        }}
        .tagline {{
            color: rgba(255,255,255,0.7);
            font-size: 14px;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .greeting {{
            font-size: 20px;
            font-weight: 600;
            color: #1a1a2e;
            margin-bottom: 20px;
        }}
        .message {{
            color: #4a5568;
            line-height: 1.8;
            margin-bottom: 30px;
            font-size: 15px;
        }}
        .button {{
            display: inline-block;
            background: linear-gradient(135deg, #f39c12 0%, #e74c3c 100%);
            color: white;
            text-decoration: none;
            padding: 16px 40px;
            border-radius: 50px;
            font-weight: 600;
            font-size: 16px;
            transition: transform 0.2s, box-shadow 0.2s;
            box-shadow: 0 10px 20px rgba(243, 156, 18, 0.3);
        }}
        .button:hover {{
            transform: translateY(-2px);
            box-shadow: 0 15px 30px rgba(243, 156, 18, 0.4);
        }}
        .footer {{
            background: #f7fafc;
            padding: 25px 30px;
            text-align: center;
            border-top: 1px solid #e2e8f0;
        }}
        .footer p {{
            color: #718096;
            font-size: 13px;
            margin-bottom: 8px;
        }}
        .social-links a {{
            color: #667eea;
            text-decoration: none;
            margin: 0 8px;
            font-size: 13px;
        }}
        .note {{
            background: #fff3cd;
            border-left: 4px solid #f39c12;
            padding: 15px 20px;
            margin-top: 25px;
            border-radius: 0 8px 8px 0;
            font-size: 14px;
            color: #856404;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='logo'>Work<span>Services</span></div>
            <div class='tagline'>Connecting Customers with Skilled Artisans</div>
        </div>
        <div class='content'>
            <div class='greeting'>Hello, {name}!</div>
            <div class='message'>
                Welcome to <strong>Work Services </strong>! We're thrilled to have you join our family.
                <br><br>
                To get started, please confirm your account by clicking the button below.
            </div>
            <div style='text-align: center;'>
                <a href='{confirmationLink}' class='button'>Confirm My Account</a>
            </div>
            <div class='note'>
                ⚠️ This confirmation link will expire in 24 hours. If you didn't create an account with us, please ignore this email.
            </div>
        </div>
        <div class='footer'>
            <p>Questions? Contact us at <strong>info@workservices.com</strong></p>
            <p>Work Services | Connecting Customers with Skilled Artisans</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}