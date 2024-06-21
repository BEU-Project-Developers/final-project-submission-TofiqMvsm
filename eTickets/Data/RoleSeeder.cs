using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using eTickets.Data.Static;
using System.Linq;

public class RoleSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<RoleSeeder> _logger;

    public RoleSeeder(RoleManager<IdentityRole> roleManager, ILogger<RoleSeeder> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedRolesAsync()
    {
        await CreateRoleIfNotExistsAsync(UserRoles.Admin);
        await CreateRoleIfNotExistsAsync(UserRoles.User);
    }

    private async Task CreateRoleIfNotExistsAsync(string roleName)
    {
        try
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Role '{roleName}' created successfully.");
                }
                else
                {
                    _logger.LogError($"Error creating role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception occurred while creating role '{roleName}'.");
        }
    }
}
