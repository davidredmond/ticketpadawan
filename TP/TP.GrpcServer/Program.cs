using TP.Domain;
using TP.Domain.Commands;
using TP.Domain.Queries;
using TP.GrpcServer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IDispatcher, Dispatcher>();
builder.Services.AddDbContext<TP.Database.TPDbContext>();
builder.Services.AddGrpc();

var dispatchHandlers = typeof(IDispatcher).Assembly.GetTypes()
    .Where(t => t.IsAbstract == false && t.IsInterface == false)
    .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
    .Where(x => x.Interface.IsGenericType && (x.Interface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) || x.Interface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));

foreach (var handler in dispatchHandlers)
{
    builder.Services.AddTransient(handler.Interface, handler.Type);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<EventService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
