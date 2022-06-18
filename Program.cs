using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixMinApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var sqlConBuilder = new SqlConnectionStringBuilder();
sqlConBuilder.ConnectionString= builder.Configuration.GetConnectionString("SQLDbConnection");
//sqlConBuilder.UserID=builder.Configuration["UserId"];
//sqlConBuilder.Password=builder.Configuration["Password"];

builder.Services.AddDbContext<AppDbContext>(opt =>opt.UseSqlServer(sqlConBuilder.ConnectionString));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};*/

/*app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");*/

app.Run();

/*record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}*/



/*
dotnet new webapi -minimal -n SixMinApi
dotnet run
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet user-secrets init
docker --version
docker ps
docker-compose up -d
docker-compose stop
mkdir folderName
https://www.youtube.com/watch?v=5YB49OEmbbE&t=2s&ab_channel=LesJackson ->51.09
dotnet user-secrets set "UserId" "sa"
dotnet user-secrets set "Password" "pa55w0rd!"


dotnet ef migrations add initialmigration
dotnet tool install --global dotnet-ef
dotnet ef migrations add initialmigration
dotnet ef database update


dotnet build


*/