using MassTransit;
using NotificationService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("nt", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        // Add config host
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", Host =>
        {
            Host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            Host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        // config endpoint
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<NotificationHub>("/notifications");

app.Run();
