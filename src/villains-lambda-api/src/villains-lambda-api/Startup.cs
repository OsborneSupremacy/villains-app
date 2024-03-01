using Microsoft.OpenApi.Models;

namespace Villains.Lambda.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Villains.Lambda.Api", Version = "v1" });
        });
        
        // AWS stuff
        AmazonDynamoDBClient dynamoClient = new();
        
        services.AddSingleton<IAmazonDynamoDB>(dynamoClient);
        services.AddSingleton<IAmazonS3>(new AmazonS3Client());

        services.AddScoped<VillainsService>();
        services.AddScoped<ImageService>();
        
        // don't want to use assembly scanning (`AddValidatorsFromAssemblyContaining`) for performance reasons
        services.AddScoped<IValidator<NewVillain>, NewVillainValidator>();
        services.AddScoped<IValidator<Villain>, VillainValidator>();
        
        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
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
        });
    }
}