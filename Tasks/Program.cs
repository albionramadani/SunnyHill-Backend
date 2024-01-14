// See https://aka.ms/new-console-template for more information


using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tasks;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt => 
opt.UseSqlServer("Server=(local)\\sqlexpress;Database=SunnyHill;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true")
);

IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<ApplicationUser>(
            options => options.SignIn.RequireConfirmedAccount = true)
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddSingleton<DatabaseInitializer>();

var app = builder.Build();

using(var scope =  app.Services.CreateScope())
{
    var dbInit = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await dbInit?.Init();
}

app.Run();