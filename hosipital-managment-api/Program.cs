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
using Serilog;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
builder.Services.AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
    .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters()); 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.SetDatabaseConnection(builder.Configuration);
builder.Services.AddIdentityCore<ApiUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultTokenProviders().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureAuthenthication(builder.Configuration);
builder.Services.InstallApiVersioning();
builder.Services.AddEndpointsApiExplorer();
builder.Services.InstallVersionedApiExplorer();
builder.Services.InstallSwagger();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();
app.ConfigureExceptionHandler2();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
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
Log.CloseAndFlush();
