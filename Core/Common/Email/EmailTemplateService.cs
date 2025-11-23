namespace Application.Common.Email
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GetForgetPasswordTemplate(string userName, string link)
        {
            var filePath = Path.Combine(
            AppContext.BaseDirectory,
            "EmailTemplates",
            "ForgetPasswordEmail.html");

            var html = File.ReadAllText(filePath);
            return html.Replace("{UserName}", userName).Replace("{ResetLink}",link);
        }

        public string GetWelcomeEmailTemplate(string userName)
        {
            var filePath = Path.Combine(
          AppContext.BaseDirectory,
          "EmailTemplates",
          "WelcomeEmail.html");

            var html = File.ReadAllText(filePath);
            return html.Replace("{UserName}", userName);
        }
    }
}
