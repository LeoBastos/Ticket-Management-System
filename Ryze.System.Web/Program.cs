using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Ryze.System.Application.Mapping;
using Ryze.System.Application.Services.Tickets;
using Ryze.System.Application.Services.Users;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Domain.Interfaces.Accounts;
using Ryze.System.Domain.Interfaces.Tickets;
using Ryze.System.Domain.Interfaces.UnitOfWork;
using Ryze.System.Domain.Seed;
using Ryze.System.Infra.Context;
using Ryze.System.Infra.Identity;
using Ryze.System.Infra.Repositories.Tickets;
using Ryze.System.Infra.Repositories.Users;
using Ryze.System.Infra.UnitOfWork;
using Ryze.System.Web.helpers;
using Ryze.System.Web.Mapping;

namespace Ryze.System.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connection));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "AspNetCore.Cookies";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    options.SlidingExpiration = true;
                });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("RequireUserAdminGerenteRole",
            //         policy => policy.RequireRole("Cliente", "User", "Admin", "Gerente"));
            //});

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("IsAdminClaimAccess",
            //        policy => policy.RequireClaim("IsAdmin", "true"));

            //    options.AddPolicy("IsManagerClaimAccess",
            //        policy => policy.RequireClaim("IsManager", "true"));

            //    options.AddPolicy("IsEmployeeClaimAccess",
            //        policy => policy.RequireClaim("IsEmployee", "true"));

            //    options.AddPolicy("IsClientClaimAccess",
            //        policy => policy.RequireClaim("IsClient", "true"));
            //});

            builder.Services.AddSingleton<IEmailSender>(new EmailSender(
                smtpServer: "",
                smtpPort: 587,
                smtpUser: "",
                smtpPass: ""
            ));


            // Injection
            builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
            builder.Services.AddScoped< IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();

            builder.Services.AddScoped<IAuthenticate, AuthenticateService>();



            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();


            //Mapper
            builder.Services.AddAutoMapper(typeof(ApplicationServiceMappings), typeof(ViewModelToDto));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();



            CriarPerfisUsuariosAsync(app);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "MinhaArea",
                pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            async Task CriarPerfisUsuariosAsync(WebApplication app)
            {
                var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

                using (var scope = scopedFactory?.CreateScope())
                {
                    var service = scope?.ServiceProvider.GetService<ISeedUserRoleInitial>();
                    await service.SeedRolesAsync();
                    await service.SeedUsersAsync();
                }
            }


        }
    }
}
