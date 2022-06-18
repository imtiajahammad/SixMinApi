using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixMinApi.Data;
using SixMinApi.Dtos;
using SixMinApi.Models;


/*
What are Minimal APIs?
->
Minimal APIs are architected to create HTTP APIs with minimal dependencies. 
They are ideal for microservices and apps that want to include only the minimal files, features, and dependencies in ASP.NET Core.


Minimal APIs:
			* Don't support model validation
                - Model Validation occurs after model binding
                - reports business rule type errors
                - Out the box with the [ApiController] attribute
                - Valication can be added to .NET6 Minimal APIs eg: FluentValidation, MinimalValidation
			* Don't support for JSONPatch
			* Don't support filters
                - allow you to run code before or after stages in the Filter Pipeline
                - Each Filter Type is executed at a different state in the pipeline. Such as Authorization, Resource, Action, Exception, Result etc
                - Custom Filters can be scoped Globally, to a Controller, To an action
                - We have Synchronous and Asynchronous versions			
                - Request Pipeline is => [Request->ExceptionHandler->HttpSRedirection->Routing->Authentication->Authorization->Custom->EndPoint->Custom->Authorization->Authentication->Routing->HttpSRedirection->ExceptionHandler->Response]
                - Filter Pipeline is=> [Endpoint->Auth Filters->Rouserce Filters->Model Binding->Model Validation->Action Filters->Exception Filters->Result Filters]
			* Don't support custom model binding(Support for IModelBinder)
                - Allow Controller Actions to work directly with Model Types
                - Can be used in more comples "binding scenarios"
                - Default model binders support mos common .NET types
                - Model Binding Occurs before Model Validation

*/

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
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("api/v1/commands",async(ICommandRepo repo, IMapper mapper)=> {
   var commands = await repo.GetAllCommands();
   return Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
});


app.MapGet("api/v1/commands/{id}",async(ICommandRepo repo, IMapper mapper,int id)=> {
   var command = await repo.GetCommandById(id);
   if(command != null)
   {
    return Results.Ok(mapper.Map<CommandReadDto>(command));
   }
   return Results.NotFound();
   
});
app.MapPost("api/v1/commands", async (ICommandRepo repo, IMapper mapper, CommandCreateDto cmdCreateDto)=>{
    var commandModel = mapper.Map<Command>(cmdCreateDto);
    
    await repo.CreateCommand(commandModel);

    await repo.SaveChanges();

    var cmdReadDto = mapper.Map<CommandReadDto>(commandModel);

    return Results.Created($"api/v1/commands/{commandModel.Id}",cmdReadDto);



});


app.MapPut("api/v1/commands/{id}",async(ICommandRepo repo, IMapper mapper,int id, CommandUpdateDto cmdUpdateDto)=> {
var command = await repo.GetCommandById(id);
   if(command == null)
   {
    return Results.NotFound();
   }
   mapper.Map(cmdUpdateDto,command);
   await repo.SaveChanges();

   return Results.NoContent();
});


app.MapDelete("api/v1/commands/{id}",async(ICommandRepo repo, IMapper mapper,int id)=> {
    var command = await repo.GetCommandById(id);
   if(command == null)
   {
    return Results.NotFound();
   }

   repo.DeleteCommand(command);
   await repo.SaveChanges();

   return Results.NoContent();
});


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
ALL THE COMMAND LINES USED THROUGHOUT THE PROJECT

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
dotnet run
dotnet watch



*/