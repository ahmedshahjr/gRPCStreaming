using Client.Services;
using Client_Protos;
using Grpc.Core;
using Grpc.Net.Client.Configuration;

var builder = WebApplication.CreateBuilder(args);
var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information);
    logging.AddFilter("Grpc", LogLevel.Information);

});
var methodConfig = new MethodConfig
{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 5,
        InitialBackoff = TimeSpan.FromSeconds(0.5),
        MaxBackoff = TimeSpan.FromSeconds(0.5),
        BackoffMultiplier = 1,
        RetryableStatusCodes = { StatusCode.Unavailable }
    }
};

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPrimeNumberService, PrimeNumberService>();
builder.Services.AddGrpcClient<PrimeNumberProtoStreaming.PrimeNumberProtoStreamingClient>(server =>
{
    server.Address = new Uri(builder.Configuration["GrpcServerUrl"]);
}).ConfigureChannel(channel =>
{
    channel.HttpHandler = new SocketsHttpHandler
    {
        EnableMultipleHttp2Connections = true
    };
    channel.LoggerFactory = loggerFactory;
    channel.ServiceConfig = new ServiceConfig
    {
        MethodConfigs = { methodConfig }
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
