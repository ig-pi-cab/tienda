using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Data.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        //Definicion clase entidad producto

        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Producto");
            builder.Property( p => p.Id ).IsRequired();

            builder.Property(p=>p.Nombre ).IsRequired().HasMaxLength(100);

            builder.Property(p => p.Precio).IsRequired().HasColumnType("decimal(18,2)");    

            //Relacion con marca y categoria
            builder.HasOne( p => p.Marca).WithMany( p=>p.Productos).HasForeignKey(p=>p.MarcaId);

            builder.HasOne(p => p.Categoria).WithMany(p => p.Productos).HasForeignKey(p => p.CategoriaId);
        }
    }
}
