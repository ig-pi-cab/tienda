using Microsoft.EntityFrameworkCore;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Infraestructure.Data
{
    public class TiendaContexto : DbContext
    {
        //Objeto dbcontext options permite brindar cadena de conexion para BD
        //Se debe agregar al contenedor de servicios en program.
        public TiendaContexto(DbContextOptions<TiendaContexto> options) : base(options)
        {
           
        }
        
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //usando reflexion para obtener el elemtno que se esta ejecutando
            //Permite implementar la configuracion de todas las instancias que implementan identitytypeconfiguration. 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
