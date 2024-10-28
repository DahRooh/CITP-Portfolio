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


var ds = new UserDataService();
var people = ds.GetUsers();


var people1 = ds.GetUser(2);


    Console.WriteLine("username= " + people1.Username);
    Console.WriteLine("id= " + people1.Id);


var ds1 = new UserDataService();

var result = ds1.GetSessions(2);

foreach (var item in result)
{
    Console.WriteLine(item);
}





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

