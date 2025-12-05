using Microsoft.Extensions.DependencyInjection;
using Sireen.Application.Interfaces.Services;
using Sireen.Application.Services;
using Sireen.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Dependencies
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
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
