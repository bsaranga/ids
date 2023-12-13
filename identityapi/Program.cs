
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
                cfg.Cookie.Domain = "http://127.0.0.1:5173";
                cfg.Cookie.SameSite = SameSiteMode.None;
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

            app.UseCors(configurePolicy =>
            {
                configurePolicy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
            });

            app.UseAuthorization();

            app.MapIdentityApi<IdentityUser>();

            app.MapControllers();

            app.Run();
        }
    }
}
