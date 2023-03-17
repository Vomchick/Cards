using Cards.API.Data;
using Cards.API.Data.Repositories;
using Cards.API.Interfaces.RepositoryChilds;
using Cards.API.Models.ContextModels;
using CardsAPI.Auth.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Inject DBContext
    builder.Services.AddDbContext<CardsDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CardsDBConnectionString")));

    //Inject Interfaces
    builder.Services.AddScoped<ICardRepository, CardRepository>();

    var authOptions = builder.Configuration.GetSection("Auth").Get<AuthOptions>();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = authOptions.Audience,

                ValidateLifetime = true,

                IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            };
        });
    builder.Services.AddAuthorization();

    //Разрешили всем обращаться к нашему api?
    builder.Services.AddCors((setup) =>
    {
        setup.AddPolicy("default", (options) =>
        {
            options.AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            options.WithOrigins("http://localhost:4200");
        });
    });

    builder.Services.AddMemoryCache();

    //Logging
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("default");

    //app.UseCookiePolicy(new CookiePolicyOptions
    //{
    //    HttpOnly = HttpOnlyPolicy.Always,
    //    Secure = CookieSecurePolicy.Always,
    //    MinimumSameSitePolicy = SameSiteMode.None
    //});

    //app.Use(async (context, next) =>
    //{
    //    var token = context.Request.Cookies["token"];
    //    if (!string.IsNullOrEmpty(token))
    //        context.Request.Headers.Add("Authorization", "Bearer " + token);

    //    await next();
    //});


    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    logger.Error(ex);
    throw (ex);
}
finally
{
    LogManager.Shutdown();
}
