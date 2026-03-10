var builder = WebApplication.CreateBuilder(args);

// Beállítjuk a HTTP klienseket, amikkel a belső hálózaton elérjük a többi szervizt
builder.Services.AddHttpClient("DirectoryService", client =>
{
    // A docker-compose.yaml-ben lévő belső hálózati neveket használjuk
    client.BaseAddress = new Uri("http://directory-service:8080"); 
});

builder.Services.AddHttpClient("VaultService", client =>
{
    client.BaseAddress = new Uri("http://vault-service:8080");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();