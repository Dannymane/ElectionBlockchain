using ElectionBlockchain.Services.ConcreteServices;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Services.Configuration.AutoMapperProfiles;
using System.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MainProfile));
// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<ILeaderService, LeaderService>();
builder.Services.AddScoped<IVerifierService, VerifierService>();

builder.Services.AddControllers(); 


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
