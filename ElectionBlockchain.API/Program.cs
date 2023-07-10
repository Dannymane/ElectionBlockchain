using ElectionBlockchain.Services.ConcreteServices;
using ElectionBlockchain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Services.Configuration.AutoMapperProfiles;
using System.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MainProfile));
// Add services to the container.


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbIp = Environment.GetEnvironmentVariable("DB_IP");
var connectionString = $"Server={dbIp},1433;Database=BlockchainDb;User ID=SA;Password=Admin123;encrypt=true;trustServerCertificate=true;";

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<ILeaderService, LeaderService>();
builder.Services.AddScoped<IVerifierService, VerifierService>();

builder.Services.AddControllers(); 


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//   app.UseSwagger();
//   app.UseSwaggerUI();
//}
app.UseSwagger(); //
app.UseSwaggerUI();//

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
