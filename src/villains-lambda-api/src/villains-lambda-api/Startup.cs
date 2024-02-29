

using Amazon.DynamoDBv2.DataModel;
using Villains.Lambda.Api.Services;

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
        // AWS stuff
        AmazonDynamoDBClient dynamoClient = new();
        DynamoDBContext dbContext = new(dynamoClient);
        
        services.AddSingleton<IAmazonDynamoDB>(dynamoClient);
        services.AddSingleton<IDynamoDBContext>(dbContext);
        services.AddSingleton<IAmazonS3>(new AmazonS3Client());

        services.AddScoped<VillainsService>();
        
        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/",
                async context =>
                {
                    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                });
        });
    }
}