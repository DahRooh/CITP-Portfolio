using Mapster;
using DataLayer;
using MovieWebserver.Controllers;
using DataLayer.IDataServices;
using DataLayer.DataServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ITitleDataService, TitleDataService>();
builder.Services.AddTransient<IPersonDataService, PersonDataService>();
builder.Services.AddTransient<IUserDataService, UserDataService>();
builder.Services.AddTransient<GetUrl>();


var ds = new TitleDataService();
var people = ds.GetMovies();

foreach (var item in people) { 
    Console.WriteLine(item.Title._Title);
}

/*
var ds1 = new UserDataService();

var result = ds1.GetSessions(2);
Console.WriteLine(result[0].Id);
Console.WriteLine(result[0].UserId);
*/




builder.Services.AddMapster();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

