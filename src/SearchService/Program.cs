using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService;

var builder = WebApplication.CreateBuilder(args);

// Adding logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

builder.Services.AddMassTransit(x =>
{
    // Khai bao noi tieu dung
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    // Su dung diem cuoi endpoint
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

    // Importance: Khai bao su dung rabbitmq
    x.UsingRabbitMq((context, cfg) =>
    {

        // Add config host
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", Host =>
        {
            Host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            Host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });


        cfg.ReceiveEndpoint("rearch-auction-created", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, 5));

            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (System.Exception e)
    {
        Console.WriteLine(e);
    }
});

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy() =>
HttpPolicyExtensions.HandleTransientHttpError()
.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
.WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));

