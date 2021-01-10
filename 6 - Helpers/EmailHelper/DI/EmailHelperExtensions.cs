using EmailHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;



namespace Microsoft.Extensions.DependencyInjection
{
    public static class EmailHelperExtensions
    {
        public static IOptionsMonitor<EmailConfigOptions> emailConfigOptions { get; set; }

        public static void AddEmailService(IServiceCollection services, IConfiguration Configuration)
        {
            var config = new EmailConfigOptions();
            Configuration.Bind(EmailConfigOptions.EmailConfig, config);

            services.AddSingleton<EmailConfigOptions>(config);

            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
