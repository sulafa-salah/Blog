using Blog.Contracts.BackgroundServices;
using Blog.Persistence;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Services.BackgroundServices
{
    public class EmailBGService : BackgroundService
    {
        private readonly IBus bus;

        public EmailBGService(IBus bus)
        {
            this.bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var service = new ServiceCollection();
         
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            var provider = service.BuildServiceProvider();

            var result = bus.ConnectReceiveEndpoint(e =>
            {
                e.Consumer<EmailBGServiceMessageConsumer>(provider);
            });
            return Task.CompletedTask;
        }
    }

    public class EmailBGServiceMessageConsumer : IConsumer<EmailMessage>
    {  
        private readonly IEmailService emailService;

        public EmailBGServiceMessageConsumer(IEmailService emailService)
        {
            this.emailService = emailService;
        }
        public async Task Consume(ConsumeContext<EmailMessage> context)
        {
            await emailService.RegisterUserEmailAndLog(context.Message.Email);
        }

    }
}
