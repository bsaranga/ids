
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace identityapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("AppDb");
                options.EnableDetailedErrors();
            });

            builder.Services.AddAuthorization();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.Cookie.SameSite = SameSiteMode.Strict;
            });

            builder.Services.AddIdentityApiEndpoints<IdentityUser>()
                            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapIdentityApi<IdentityUser>();

            app.MapControllers();

            app.Run();
        }
    }
}
