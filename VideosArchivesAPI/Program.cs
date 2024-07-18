using Microsoft.AspNetCore.Server.Kestrel.Core;
using VideosArchiveAPI.Helpers.Configs;
using VideosArchiveAPI.Interfaces;
using VideosArchiveAPI.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Bind configuration section
        builder.Services.Configure<UploadSettings>(builder.Configuration.GetSection("UploadSettings"));

        // Configure services and middleware
        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = 209715200; // 200 MB
        });

        //add cors
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder
                    .WithOrigins("https://localhost:44328") // URL of the MVC project
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Register the UploadService
        builder.Services.AddScoped<IUploadsService, UploadsService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles(); // Ensure static files are served

        app.UseCors("AllowSpecificOrigin");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}