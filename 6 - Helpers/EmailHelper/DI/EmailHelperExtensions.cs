using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;



namespace EmailHelper.DI
{
    public static class EmailHelperExtensions
    {
        public static IOptionsMonitor<EmailConfigOptions> emailConfigOptions { get; set; }

        public static void AddEmailService(IServiceCollection services, IConfiguration Configuration)
        {

            services.AddOptions<EmailConfigOptions>()
                .Bind(Configuration.GetSection(EmailConfigOptions.EmailConfig))
           .ValidateDataAnnotations();

            services.AddScoped<IEmailService, EmailService>();
        }
    }
}