using CloudStorage.Persistence;
using CloudStorage.Services;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();

Configure(app);

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var fileContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(fileContext);
    }
    catch (Exception exception)
    {
        throw new Exception($"{exception}");
    }
}


app.Run();

void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
    });

    services.AddPersistence(configuration);
    services.AddServices(configuration);
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();

    app.UseHttpsRedirection();

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always
    });


    app.UseCors("AllowAll");

    app.UseAuthorization();

    app.UseAuthentication();

    app.MapControllers();
}
