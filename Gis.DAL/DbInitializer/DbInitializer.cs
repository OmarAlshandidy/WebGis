using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Gis.DAL.Data;
using Gis.DAL.DbInitializer;
using Gis.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gis.DAL.DbInitializer
{
   
        public class DbInitializer : IDbInitializer
        {
            private readonly GisDbContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public DbInitializer(GisDbContext context,
                UserManager<AppUser> userManager,
                RoleManager<IdentityRole> roleManager
                )
            {
                _context = context;
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task IdintityInitializeAsync()
            {
                // Create DataBase If It Dosen`t Exists && Apply To Any Pending Migrations
                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }
                // Data Seeding Role 
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = "Admin"
                    });
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = "SuperAdmin"
                    });
                }

                // Data Seding  User 
                if (!_userManager.Users.Any())
                {
                    var superAdminUser = new AppUser()
                    {
                        
                        Email = "superAdmin@gmail.com",
                        UserName = "SuperAdmin",
                        IsAgreed = true

                    };
                    var adminUser = new AppUser()
                    {
                        Email = "Admin@gmail.com",
                        UserName = "Admin",
                        IsAgreed = true 
                    };

                    await _userManager.CreateAsync(superAdminUser, "P@ssW0rd");
                    await _userManager.CreateAsync(adminUser, "P@ssW0rd");
                    //Add User To Role 
                    await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                    await _userManager.AddToRoleAsync(adminUser, "Admin");

                }



            }

            public async Task InitializeAsync()
            {
                try
                {


                    // Create DataBase If It Dosen`t Exists && Apply To Any Pending Migrations
                    if (_context.Database.GetPendingMigrations().Any())
                    {
                        await _context.Database.MigrateAsync();
                    }
                // Data Seeding 
                // Seeding Pharmacies  From Json Files
                if (!_context.Pharmacies.Any())
                {
                    // 1.Read All Data From Types Json Files As String
                    var Pharmcy = await File.ReadAllTextAsync(@"..\Gis.DAL\Seeding\Pharmcy.json");
                    // 2. Transform string To C# Object [list<Pharmacies>]
                    var pharmacies = JsonSerializer.Deserialize<List<Pharmacy>>(Pharmcy);


                    // 3. Add   List<Pharmacies> TO DataBase  
                    if (pharmacies is not null && pharmacies.Any())
                    {
                        await _context.Pharmacies.AddRangeAsync(pharmacies);
                        await _context.SaveChangesAsync();
                    }

                }

                    // Seeding Markets  From Json Files
                    if (!_context.Markets.Any())
                    {
                        // 1.Read All Data From Types Json Files As String
                        var Market = await File.ReadAllTextAsync(@"..\Gis.DAL\Seeding\Markets.json");
                        // 2. Transform string To C# Object [list<Markets>]
                        var Markets = JsonSerializer.Deserialize<List<Market>>(Market);


                        // 3. Add   List<Markets> TO DataBase  
                        if (Markets is not null && Markets.Any())
                        {
                            await _context.Markets.AddRangeAsync(Markets);
                            await _context.SaveChangesAsync();
                        }

                    }


                // Seeding Mosques  From Json Files
                if (!_context.Mosques.Any())
                {
                    // 1.Read All Data From Types Json Files As String
                    var Mosque = await File.ReadAllTextAsync(@"..\Gis.DAL\Seeding\Mosques.json");
                    // 2. Transform string To C# Object [list<Mosques>]
                    var Mosques = JsonSerializer.Deserialize<List<Mosque>>(Mosque);


                    // 3. Add   List<Markets> TO DataBase  
                    if (Mosques is not null && Mosque.Any())
                    {
                        await _context.Mosques.AddRangeAsync(Mosques);
                        await _context.SaveChangesAsync();
                    }

                }

                // Seeding Restaurants  From Json Files
                if (!_context.Restaurants.Any())
                {
                    // 1.Read All Data From Types Json Files As String
                    var Restaurant = await File.ReadAllTextAsync(@"..\Gis.DAL\Seeding\Restaurants.json");
                    // 2. Transform string To C# Object [list<Restaurant>]
                    var Restaurants = JsonSerializer.Deserialize<List<Restaurant>>(Restaurant);


                    // 3. Add   List<Restaurant> TO DataBase  
                    if (Restaurants is not null && Restaurant.Any())
                    {
                        await _context.Restaurants.AddRangeAsync(Restaurants);
                        await _context.SaveChangesAsync();
                    }

                }

                // Seeding StudentHousings  From Json Files
                if (!_context.StudentHousings.Any())
                {
                    // 1.Read All Data From Types Json Files As String
                    var StudentHousing = await File.ReadAllTextAsync(@"..\Gis.DAL\Seeding\StudentHousings.json");
                    // 2. Transform string To C# Object [list<Restaurant>]
                    var StudentHousings = JsonSerializer.Deserialize<List<StudentHousing>>(StudentHousing);


                    // 3. Add   List<StudentHousing> TO DataBase  
                    if (StudentHousings is not null && StudentHousing.Any())
                    {
                        await _context.StudentHousings.AddRangeAsync(StudentHousings);
                        await _context.SaveChangesAsync();
                    }

                }






            }
            catch (Exception)
                {

                    throw;
                }

            }
        }
    }

