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
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "交易資料產生器");
    c.RoutePrefix = string.Empty;
});


app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => $"Hello !!!");

app.Run();
