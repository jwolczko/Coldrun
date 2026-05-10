using Coldrun.Modules.TruckManagement.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        var app = BuildApplication(args);

        app.Run();
    }

    internal static WebApplication BuildApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddControllers()
            .AddApplicationPart(typeof(TruckManagementModule).Assembly);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddTruckManagementModule(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "Coldrun API v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.MapControllers();
        return app;
    }
}
