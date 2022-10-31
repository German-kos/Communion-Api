using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Data;
using api.Extensions;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddHttpContextAccessor();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy
.AllowAnyHeader()
.AllowAnyMethod()
.WithOrigins("http://localhost:3000")
);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
