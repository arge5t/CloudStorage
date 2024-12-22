using CloudStorage.Domain.Entities;
using CloudStorage.Persistence;
using CloudStorage.Services;
using CloudStorage.Services.Implementations;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

    services.AddCors();

    services.AddPersistence(configuration);
    services.AddServices(configuration);

    var jwt = new Jwt();
    configuration.Bind("Jwt", jwt);

    services.AddSingleton(jwt);
    services.AddTransient<TokenService>();

    services.AddAuthorization();

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ClockSkew = jwt.ExpirationTime
        };
    });
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


    app.UseCors(x => x
        .WithOrigins("http://localhost:3000")
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();
}
