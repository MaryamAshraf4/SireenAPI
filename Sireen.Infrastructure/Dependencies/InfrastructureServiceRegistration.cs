using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Interfaces.Services;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Infrastructure.Persistence;
using Sireen.Infrastructure.Services;
using Sireen.Infrastructure.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Infrastructure.Dependencies
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("Default")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IAmenityService, AmenityService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    }
}
