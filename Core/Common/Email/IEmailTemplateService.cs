namespace Application.Common.Email
{
    public interface IEmailTemplateService
    {
        string GetWelcomeEmailTemplate(string userName);

        string GetForgetPasswordTemplate(string userName, string link);
    }
}
