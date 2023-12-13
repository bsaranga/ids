
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace identityapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? connectionString = builder.Configuration.GetConnectionString("IdentityDb");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (connectionString != null)
                    options.UseNpgsql(connectionString);

                options.EnableDetailedErrors();
            });

            builder.Services.AddAuthorization();

            builder.Services.ConfigureApplicationCookie(cfg =>
            {
                cfg.Cookie.SameSite = SameSiteMode.Strict;
                cfg.Cookie.HttpOnly = true;
            });

            builder.Services.AddIdentityApiEndpoints<IdentityUser>()
                            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllers();
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

            app.MapGroup("/id").MapIdentityApi<IdentityUser>();

            app.MapControllers();

            app.Run();
        }
    }
}
