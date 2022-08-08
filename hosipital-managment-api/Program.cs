using hosipital_managment_api.Data;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using hosipital_managment_api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
    .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters()); 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.SetDatabaseConnection(builder.Configuration);
builder.Services.AddIdentityCore<ApiUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultTokenProviders().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureAuthenthication(builder.Configuration);
builder.Services.ConfigureSwagger();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApiUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    SeedData.SeedRoles(roleManager);
    SeedData.SeedAdmin(roleManager, userManager, builder.Configuration["AdminPassword"].ToString());
}

app.Run();
