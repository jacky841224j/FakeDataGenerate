using FakeDataGenerate.Extensions;
using FakeDataGenerate.Handler;
using FakeDataGenerate.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbconstring = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDapper(dbconstring);
builder.Services.AddScoped<IRandomDataGenerate,RandomDataGenerate>();

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
app.MapGet("/", () => $"Hello !");

app.Run();
