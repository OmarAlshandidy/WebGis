using Gis.DAL.Data.Configurations;
using Gis.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
#nullable enable

namespace Gis.DAL.Data;

public partial class GisDbContext : IdentityDbContext<AppUser>
{
    public GisDbContext()
    {

    }

    public GisDbContext(DbContextOptions<GisDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Market> Markets { get; set; }

    public virtual DbSet<Mosque> Mosques { get; set; }

    public virtual DbSet<Pharmacy> Pharmacies { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<StudentHousing> StudentHousings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        OnModelCreatingPartial(modelBuilder);
    }
   

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
