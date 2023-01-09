




using API;
using Serilog;

var builder = WebApplication.CreateBuilder(args);



//agregar Serilog
IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
builder.Host.UseSerilog();



// Add services to the container.

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();



startup.Configure(app, app.Environment);


app.Run();











