using Microsoft.Extensions.Logging;
using CsvHelper; 
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;

namespace Infraestructure.Data.Csvs
{
    public class TiendaContextSeed
    {
        public static async Task SeedAsync(TiendaContexto context, ILoggerFactory loggerFactory)
        {
			try
			{
				var ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

				if (!context.Marcas.Any())
				{
					using (var readerMarcas = new StreamReader(ruta + @"/Data/Csvs/marcas.csv"))
					{
						using (var csvMarcas = new CsvReader(readerMarcas, CultureInfo.InvariantCulture))
						{
							var marcas = csvMarcas.GetRecords<Marca>();
							context.Marcas.AddRange(marcas);
							await context.SaveChangesAsync();
						}
					}
				}
                if (!context.Categorias.Any())
                {
                    using (var readerCategorias = new StreamReader(ruta + @"/Data/Csvs/categorias.csv"))
                    {
                        using (var csvCategorias = new CsvReader(readerCategorias, CultureInfo.InvariantCulture))
                        {
                            var categorias = csvCategorias.GetRecords<Categoria>();
                            context.Categorias.AddRange(categorias);
                            await context.SaveChangesAsync();
                        }
                    }
                }
               

                if (!context.Productos.Any())
                {
                    using (var readerProductos = new StreamReader(ruta + @"/Data/Csvs/productos.csv"))
                    {
                        using (var csvProductos = new CsvReader(readerProductos, CultureInfo.InvariantCulture))
                        {
                            var listadoProductosCsv = csvProductos.GetRecords<Producto>();

                            List<Producto> productos = new List<Producto>();
                            foreach (var item in listadoProductosCsv)
                            {
                                productos.Add(new Producto
                                {
                                    Id= item.Id,
                                    Nombre = item.Nombre,
                                    Precio = item.Precio, 
                                    FechaCreacion = item.FechaCreacion,
                                    CategoriaId = item.CategoriaId,
                                    MarcaId =   item.MarcaId,
                                });
                            }

                            context.Productos.AddRange(productos);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
			catch (Exception ex)
			{

				var logger = loggerFactory.CreateLogger<TiendaContextSeed>();
				logger.LogError(ex.Message);
			}
        }
        
        public static async Task SeedRolesAsync(TiendaContexto context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Roles.Any())
                {
                    var roles = new List<Rol>()
                    {
                        new Rol {Id = 1, Nombre = "Administrador"},
                        new Rol {Id = 2,Nombre = "Gerente"},
                        new Rol {Id = 3, Nombre = "Empleado"}
                    };

                    context.Roles.AddRange(roles);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<TiendaContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
