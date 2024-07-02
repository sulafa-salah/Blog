using Blog.Contracts;
using Blog.Options;
using Blog.Services.BackgroundServices;
using MassTransit;
using MassTransit.Definition;

namespace Blog.Installers
{
    public class MassTransitInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {

            RabbitMQOption rabbitMQOption = new RabbitMQOption();
            configuration.GetSection(nameof(RabbitMQOption)).Bind(rabbitMQOption);
            services.AddMassTransit(cf =>
            {
                // Endpoint formatter name
                cf.SetSnakeCaseEndpointNameFormatter();

                #region Consumers Declartion
                
                cf.AddConsumer<EmailBGServiceMessageConsumer>();
               
                #endregion

                // Config RabbitMQ
                cf.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQOption.Host, configrator =>
                    {
                        configrator.Username(rabbitMQOption.UserName);
                        configrator.Password(rabbitMQOption.Password);
                    });

                    cfg.ClearMessageDeserializers();
                    cfg.UseRawJsonSerializer();
                    cfg.ConfigureEndpoints(context, SnakeCaseEndpointNameFormatter.Instance);

                    // Recieve endpoint
                    #region Consumers 
                  

                    cfg.ReceiveEndpoint(AppConst.RMQueues.UserRegistrationMQ, c =>
                    {
                        c.ConfigureConsumer<EmailBGServiceMessageConsumer>(context);
                    });
                 
                    #endregion

                });

            });

            services.AddMassTransitHostedService();

            // Register Hosted Services

            services.AddHostedService<EmailBGService>();
          
        }
    }
}
