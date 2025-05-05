using Gis.DAL.Data;
using Gis.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace Gis.DAL.Data.Configurations
{
    public partial class MarketConfiguration : IEntityTypeConfiguration<Market>
    {
        public void Configure(EntityTypeBuilder<Market> entity)
        {
            entity.HasKey(e => e.Objectid).HasName("PK__MARKETS__F4B70D851C07A19D");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Market> entity);
    }
}
