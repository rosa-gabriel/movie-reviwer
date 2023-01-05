using API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddAplicationServices(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await SeedUser.SeedDataBase(app);

app.Run();
