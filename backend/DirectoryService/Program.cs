using DirectoryService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Bármilyen URL-ről jöhet kérés (Safari megnyugszik)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Hozzáadjuk a saját szervizünket a Dependency Injection konténerhez
builder.Services.AddSingleton<EmployeeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger bekapcsolása fejlesztői módban
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseAuthorization();
app.MapControllers();

app.Run();