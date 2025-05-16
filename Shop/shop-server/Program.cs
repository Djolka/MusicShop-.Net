using Microsoft.EntityFrameworkCore;
using MusicShop.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:4200") 
               .AllowAnyOrigin()   
               .AllowAnyMethod()  
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
//app.UseAuthorization();
app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run("http://localhost:3000");
