
using ECatalog.API.Middleware;
using ECatalog.Application.CQRS.Handler.CreateCatalogItem;
using ECatalog.Application.Interfaces;
using ECatalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ECatalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] ({CorrelationId}) {Message:lj}{NewLine}{Exception}")
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            builder.Host.UseSerilog();


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<ICatalogItemRepository, CatalogItemRepository>();

            builder.Services.AddDbContext<CatalogDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateCatalogItemCommand).Assembly);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestPath", httpContext.Request.Path);
                    diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
                    diagnosticContext.Set("StatusCode", httpContext.Response.StatusCode);
                };
            });

            app.UseAuthorization();


            app.MapControllers();




            app.Run();
        }
    }
}
