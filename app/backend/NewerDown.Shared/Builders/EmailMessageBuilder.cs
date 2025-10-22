namespace NewerDown.Shared.Builders;

public class EmailMessageBuilder
{
    private string _title = "";
    private string _description = "";
    private string _url = "";
    private string _level = "info"; 
    private string _date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

    public EmailMessageBuilder SetTitle(string title)
    {
        _title = title;
        return this;
    }

    public EmailMessageBuilder SetDescription(string description)
    {
        _description = description;
        return this;
    }

    public EmailMessageBuilder SetUrl(string url)
    {
        _url = url;
        return this;
    }

    public EmailMessageBuilder SetLevel(string level)
    {
        _level = level.ToLower();
        return this;
    }

    public EmailMessageBuilder SetDate(DateTime date)
    {
        _date = date.ToString("yyyy-MM-dd HH:mm:ss");
        return this;
    }

    public string BuildHtml()
    {
        string color = _level switch
        {
            "critical" => "#d9534f",
            "warning" => "#f0ad4e",
            _ => "#0275d8"
        };

        return $@"
        <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f8f9fa; color: #333; }}
                    .container {{ padding: 20px; border: 1px solid #ddd; background-color: #fff; max-width: 600px; margin: auto; }}
                    .header {{ font-size: 20px; font-weight: bold; margin-bottom: 10px; color: {color}; }}
                    .content {{ font-size: 14px; line-height: 1.5; }}
                    .button {{
                        display: inline-block;
                        padding: 10px 15px;
                        margin-top: 15px;
                        background-color: {color};
                        color: #fff;
                        text-decoration: none;
                        border-radius: 5px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>🚨 {_title}</div>
                    <div class='content'>
                        <p><strong>Description:</strong> {_description}</p>
                        <p><strong>Date:</strong> {_date}</p>
                        {(string.IsNullOrEmpty(_url) ? "" : $"<a class='button' href='{_url}'>Watch incident</a>")}
                    </div>
                </div>
            </body>
        </html>";
    }
}