using Mapster;
using DataLayer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ITitleDataService, TitleDataService>();
builder.Services.AddTransient<IPersonDataService, PersonDataService>();

var ds = new TitleDataService();

var people = ds.GetTitles();
foreach (var item in people) { 
    Console.WriteLine(item._Title);
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

