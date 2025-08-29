namespace NewerDown.Functions.Builders;

public static class EmailMessageBuilder
{
    public static string BuildEmailMessage(string subject, string recipientEmail)
    {
        string body = $@"
            <p>Hello, {recipientEmail}</p>
            <p>You have been invited to join NewerDown.</p>
            <p>If you did not request this, you can safely ignore this email.</p>";
        
        return $@"
            <html>
                <head>
                    <title>{subject}</title>
                </head>
                <body>
                    <p>{body}</p>
                    <p>Best regards,<br/>NewerDown Team</p>
                </body>
            </html>";
    }
}