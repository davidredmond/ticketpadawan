using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using TP.Domain;
using TP.Domain.Commands.Event;
using TP.Domain.Commands.Ticket;
using TP.Domain.Commands.Venue;
using TP.Domain.Enum;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;
using TP.Domain.Models.Ticket;
using TP.Domain.Models.Venue;
using TP.Domain.Queries;
using TP.Domain.Queries.Event;
using TP.Domain.Queries.Report;
using TP.Domain.Queries.Ticket;
using TP.Domain.Queries.User;
using TP.Domain.Queries.Venue;
using Wolverine;

const string SIGNING_KEY = "CaseBeanHeadQuitEdgeGlueWoodFork";

var bld = WebApplication.CreateBuilder();
bld.Services
    .AddAuthenticationJwtBearer(a => a.SigningKey = SIGNING_KEY)
    .AddAuthorization()
    .AddFastEndpoints();
bld.Services.Configure<JwtCreationOptions>(a =>
{
    a.Issuer = "localhost";
    a.Audience = "localhost";
    a.SigningKey = SIGNING_KEY;
});
bld.Services.AddAuthorizationBuilder()
    .AddPolicy(UserRoleEnum.ADMINISTRATOR.ToFriendlyString(), p => p.RequireRole(UserRoleEnum.ADMINISTRATOR.ToFriendlyString()))
    .AddPolicy(UserRoleEnum.USER.ToFriendlyString(), p => p.RequireRole(UserRoleEnum.USER.ToFriendlyString()));

bld.Services.AddScoped<IDispatcher, Dispatcher>();
bld.Services.AddDbContext<TP.Database.TPDbContext>();

bld.Services.AddTransient<TP.Domain.Commands.ICommandHandler<CreateVenueCommand, WorkResult<VenueModel>>, CreateVenueCommandHandler>();
bld.Services.AddTransient<IQueryHandler<GetAllVenuesQuery, WorkResult<IEnumerable<VenueModel>>>, GetAllVenuesQueryHandler>();

bld.Services.AddTransient<TP.Domain.Commands.ICommandHandler<PurchaseTicketCommand, WorkResult<IEnumerable<TicketModel>>>, PurchaseTicketCommandHandler>();
bld.Services.AddTransient<IQueryHandler<GetAvailableTicketsForEventQuery, WorkResult<IEnumerable<TicketPriceModel>>>, GetAvailableTicketsForEventQueryHandler>();

bld.Services.AddTransient<TP.Domain.Commands.ICommandHandler<RefundTicketCommand, WorkResult<bool>>, RefundTicketCommandHandler>();
bld.Services.AddTransient<TP.Domain.Commands.ICommandHandler<CancelEventCommand, WorkResult<int>>, CancelEventCommandHandler>();
bld.Services.AddTransient<TP.Domain.Commands.ICommandHandler<CreateEventCommand, WorkResult<EventModel>>, CreateEventCommandHandler>();
bld.Services.AddTransient<TP.Domain.Commands.ICommandHandler<DeleteEventCommand, WorkResult<int>>, DeleteEventCommandHandler>();
bld.Services.AddTransient<TP.Domain.Commands.ICommandHandler<UpdateEventCommand, WorkResult<int>>, UpdateEventCommandHandler>();
bld.Services.AddTransient<IQueryHandler<GetAllActiveEventsQuery, WorkResult<IEnumerable<EventModel>>>, GetAllActiveEventsQueryHandler>();
bld.Services.AddTransient<IQueryHandler<GetEventByIdQuery, WorkResult<EventModel>>, GetEventByIdQueryHandler>();
bld.Services.AddTransient<IQueryHandler<GetSalesReportForEventQuery, WorkResult<EventSalesModel>>, GetSalesReportForEventQueryHandler>();
bld.Services.AddTransient<IQueryHandler<GetUserAuthorisationQuery, WorkResult<IEnumerable<UserRoleEnum>>>, GetUserAuthorisationQueryHandler>();

bld.Services.SwaggerDocument();

bld.Host.UseWolverine(opt =>
{
    opt.Discovery.IncludeAssembly(typeof(EventModel).Assembly);
});

var app = bld.Build();
app.UseFastEndpoints()
   .UseSwaggerGen();
app.MapGet("/", () => Results.Redirect("/swagger/")).ExcludeFromDescription();
app.Run();
