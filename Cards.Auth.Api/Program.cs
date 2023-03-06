using Cards.Auth.Api.Data;
using Cards.Auth.Api.Data.Repositories;
using Cards.Auth.Api.Interfaces.RepositoryChilds;
using CardsAPI.Auth.Common;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

//Inject DBContext
builder.Services.AddDbContext<CardsDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("CardsDBConnectionString")));

//Inject Interfaces
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Configuration setup (for JWT Auth)
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth"));

builder.Services.AddCors((setup) =>
{
    setup.AddPolicy("default", (options) =>
    {
        options.AllowAnyMethod().AllowAnyHeader().AllowCredentials();
        options.WithOrigins("http://localhost:4200");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("default");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
