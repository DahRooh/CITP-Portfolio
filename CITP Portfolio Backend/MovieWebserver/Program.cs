using Mapster;
using DataLayer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ITitleDataService, TitleDataService>();
builder.Services.AddTransient<IPersonDataService, PersonDataService>();

var ds = new PersonDataService();

var movies = ds.GetPeople();
foreach (var item in movies) { 
    Console.WriteLine(item.Name);
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

