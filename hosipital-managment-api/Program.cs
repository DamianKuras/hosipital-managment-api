using hosipital_managment_api.Data;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connection = builder.Configuration.GetConnectionString("AppDbConnection") 
    + ";password=" + builder.Configuration["DbPassword"].ToString();
builder.Services.AddDbContext<AppDbContext>(options => options
.UseNpgsql(connection));
builder.Services.AddAuthentication();
builder.Services.AddIdentityCore<ApiUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
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
