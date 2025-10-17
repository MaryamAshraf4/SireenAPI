using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sireen.Application.Interfaces.Services;
using Sireen.Application.Services;
using Sireen.Domain.Interfaces.Services;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Domain.Models;
using Sireen.Infrastructure.Persistence;
using Sireen.Infrastructure.UnitOfWork;

string txt = "DevCors";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddCors( option =>
{
    option.AddPolicy(txt, 
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });
});

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IRoleService,RoleService>();
builder.Services.AddScoped<IAppUserService,AppUserService>();
builder.Services.AddScoped<IHotelService,HotelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(txt);

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
