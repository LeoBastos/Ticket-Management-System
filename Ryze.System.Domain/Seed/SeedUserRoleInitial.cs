using Microsoft.AspNetCore.Identity;
using Ryze.System.Domain.Entity.Identity;

namespace Ryze.System.Domain.Seed
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoleInitial(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Cliente"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Cliente";
                role.NormalizedName = "CLIENTE";
                role.ConcurrencyStamp = Guid.NewGuid().ToString();

                IdentityResult roleResult = await _roleManager.CreateAsync(role);
            }

            if (!await _roleManager.RoleExistsAsync("Funcionario"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Funcionario";
                role.NormalizedName = "FUNCIONARIO";
                role.ConcurrencyStamp = Guid.NewGuid().ToString();

                IdentityResult roleResult = await _roleManager.CreateAsync(role);
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
                role.ConcurrencyStamp = Guid.NewGuid().ToString();
                IdentityResult roleResult = await _roleManager.CreateAsync(role);
            }

            if (!await _roleManager.RoleExistsAsync("Gerente"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Gerente";
                role.NormalizedName = "GERENTE";
                role.ConcurrencyStamp = Guid.NewGuid().ToString();
                IdentityResult roleResult = await _roleManager.CreateAsync(role);
            }

        }

        public async Task SeedUsersAsync()
        {
            if (await _userManager.FindByEmailAsync("cliente@localhost") == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "cliente@localhost";
                user.Email = "cliente@localhost";
                user.NormalizedUserName = "CLIENTE@LOCALHOST";
                user.NormalizedEmail = "CLIENTE@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.IsClient = true;
                user.FullName = "Cliente";
                user.Avatar = "~/images/noimage.png";
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = await _userManager.CreateAsync(user, "Numsey#2023");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Cliente");
                }


            }

            if (await _userManager.FindByEmailAsync("funcionario@localhost") == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "funcionario@localhost";
                user.Email = "funcionario@localhost";
                user.NormalizedUserName = "FUNCIONARIO@LOCALHOST";
                user.NormalizedEmail = "FUNCIONARIO@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.FullName = "Funcionario";
                user.Avatar = "~/images/noimage.png";
                user.IsClient = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = await _userManager.CreateAsync(user, "Numsey#2023");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Funcionario");
                }
            }

            if (await _userManager.FindByEmailAsync("admin@localhost") == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin@localhost";
                user.Email = "admin@localhost";
                user.NormalizedUserName = "ADMIN@LOCALHOST";
                user.NormalizedEmail = "ADMIN@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.FullName = "Administrador";
                user.Avatar = "~/images/noimage.png";
                user.IsClient = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = await _userManager.CreateAsync(user, "Numsey#2023");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            if (await _userManager.FindByEmailAsync("gerente@localhost") == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "gerente@localhost";
                user.Email = "gerente@localhost";
                user.NormalizedUserName = "GERENTE@LOCALHOST";
                user.NormalizedEmail = "GERENTE@LOCALHOST";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.FullName = "Gerente";
                user.Avatar = "~/images/noimage.png";
                user.IsClient = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = await _userManager.CreateAsync(user, "Numsey#2023");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Gerente");
                }

            }
        }
    }
}
