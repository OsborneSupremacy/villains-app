using System.Reflection;
using Amazon.S3;
using Microsoft.OpenApi.Models;

namespace Villains.Lambda.Api;

/// <summary>
/// The startup class for the application.
/// </summary>
public class Startup
{
    /// <summary>
    /// The constructor for the startup class.
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// The configuration for the application.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Villains.Lambda.Api", Version = "v1" });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        // AWS stuff
        AmazonDynamoDBClient dynamoClient = new();

        // my services
        services.AddSingleton<IAmazonDynamoDB>(dynamoClient);
        services.AddSingleton<IAmazonS3>(new AmazonS3Client());

        services.AddScoped<IVillainsService, VillainsService>();
        services.AddScoped<IImageService, ImageService>();

        // don't want to use assembly scanning (`AddValidatorsFromAssemblyContaining`) for performance reasons
        services.AddScoped<IValidator<NewVillain>, NewVillainValidator>();
        services.AddScoped<IValidator<Villain>, VillainValidator>();

        services.AddControllers();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/health", async context =>
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync("Healthy");
            });
        });
    }
}