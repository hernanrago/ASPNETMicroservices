using Basket.API.GrpcServices;
using Basket.API.Repositories;
using MassTransit;
using static Discount.gRPC.Protos.DiscountProtoService;

var builder = WebApplication.CreateBuilder(args);

// Redis configuration.
builder.Services.AddStackExchangeRedisCache(opts => opts.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// General configuration.
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Grpc configuration.
builder.Services.AddGrpcClient<DiscountProtoServiceClient>(c => c.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl")));
builder.Services.AddScoped<DiscountGrpcService>();

// MassTransit-RabbitMQ configuration.
builder.Services.AddMassTransit(c =>
{
    c.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
