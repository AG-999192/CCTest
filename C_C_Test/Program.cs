using C_C_Test;
using C_C_Test.Conversions;
using C_C_Test.DataAccess;
using C_C_Test.FileIO;
using MediatR;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<IFileParsing, FileParsing>();
        builder.Services.AddScoped<IDataRepository, DataRepository>();
        builder.Services.AddScoped<IConversion, Conversion>();
        
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

        builder.Logging.AddSerilog();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        

        app.Run();
    }
}