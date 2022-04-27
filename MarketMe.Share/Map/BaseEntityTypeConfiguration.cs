using MarketMe.Share.Basic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Share.Map
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasQueryFilter(m => m.IsDeleted == false);
            builder.Property(t => t.CreatedBy).HasMaxLength(150).IsUnicode(false);
            builder.Property(t => t.ModifiedBy).HasMaxLength(150).IsUnicode(false);
        }
    }
}
