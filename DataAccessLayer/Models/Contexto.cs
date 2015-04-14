using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Contexto : DbContext
    {
        private int userId;
       
        public Contexto()
            : base("BD3SoftInventarios")
        {
            System.Diagnostics.Debug.Print(Database.Connection.ConnectionString);
        }            

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {          
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

         }

        public int SaveChanges(int userId)
        {

            var creados = this.ChangeTracker.Entries()
                            .Where(e => e.State == System.Data.Entity.EntityState.Added)
                            .Select(e => e.Entity).OfType<Generica>().ToList();

            foreach (var item in creados)
            {
                item.CreatedAt = DateTime.Now;
                item.CreatedById = (userId==0)?null:(int?)userId;
            }

            var modificados = this.ChangeTracker.Entries()
                            .Where(e => e.State == System.Data.Entity.EntityState.Modified)
                            .Select(e => e.Entity).OfType<Generica>().ToList();

            foreach (var item in modificados)
            {
                item.EditedAt = DateTime.Now;
                item.EditedById = (userId == 0) ? null : (int?)userId;
            }

            return SaveChanges();
            

        }

        public virtual DbSet<Ejercicio> Ejercicios { get; set; }
        
        public virtual DbSet<Municipio> Municipios { get; set; }
    
        public virtual DbSet<Usuario> Usuarios { get; set; }


        public virtual DbSet<Proveedores> DBSProveedores { get; set; }

        public virtual DbSet<GruposPS> DBSGruposPS { get; set; }

        public virtual DbSet<Articulos> DBSArticulos { get; set; }
        public virtual DbSet<UnidadesDeMedida> DBSUnidadesDeMedida { get; set; }

        public virtual DbSet<Servicios> DBSServicios { get; set; }


        public virtual DbSet<Pedidos> DBSPedidos { get; set; }
        public virtual DbSet<PedidosArticulos> DBSPedidosArticulos { get; set; }
        public virtual DbSet<PedidosServicios> DBSPedidosServicios { get; set; }


        public virtual DbSet<Clientes> DBSClientes { get; set; }
        public virtual DbSet<Ventas> DBSVentas { get; set; }
        public virtual DbSet<VentasArticulos> DBSVentasArticulos { get; set; }


        public virtual DbSet<InventarioInicial> DBSInventarioInicial { get; set; }
        public virtual DbSet<InventarioInicialArticulos> DBSInventarioInicialArticulos { get; set; }
        public virtual DbSet<InventarioInicialArticulosCostos> DBSInventarioInicialArticulosCostos { get; set; }

        public virtual DbSet<AlmacenEntradasGenericas> DBSAlmacenEntradasGenericas { get; set; }
        public virtual DbSet<AlmacenEntradasGenericasArticulos> DBSAlmacenEntradasGenericasArticulos { get; set; }
        public virtual DbSet<AlmacenSalidasGenericas> DBSAlmacenSalidasGenericas { get; set; }
        public virtual DbSet<AlmacenSalidasGenericasArticulos> DBSAlmacenSalidasGenericasArticulos { get; set; }



        public virtual DbSet<FacturasAlmacen> DBSFacturaAlmacen { get; set; }
        public virtual DbSet<FacturasAlmacenArticulos> DBSFacturaAlmacenArticulos { get; set; }
        public virtual DbSet<FacturasAlmacenArticulosTMP> DBSfacturasalmacenarticulosTMP { get; set; }
        

        public virtual DbSet<ArticulosMovimientos> DBSArticulosMovimientos { get; set; }
        public virtual DbSet<ArticulosMovimientosEntradas> DBSArticulosMovimientosEntradas { get; set; }
        public virtual DbSet<ArticulosMovimientosSalidas> DBSArticulosMovimientosSalidas { get; set; }



        public virtual DbSet<Laboratorios> DBSlaboratorios { get; set; }
        public virtual DbSet<Marcas> DBSmarcas { get; set; }
        public virtual DbSet<Lineas> DBSlineas { get; set; }
        public virtual DbSet<Presentaciones> DBSpresentaciones { get; set; }
        public virtual DbSet<FPSfactores> DBSfpsfactores { get; set; }
        public virtual DbSet<Sexos> DBSsexos { get; set; }

        public virtual DbSet<Recetas> DBSRecetas { get; set; }
        public virtual DbSet<RecetaArticulos> DBSRecetasArticulos { get; set; }
        public virtual DbSet<ArticuloVenta> DBSArticuloVenta { get; set; }
        public virtual DbSet<ArticuloSalidaGenerica> DBSArticuloSalidaGenerica { get; set; }
        public virtual DbSet<TipoSalida> DBSTipoSalida { get; set; }

        public virtual DbSet<Cotizaciones> DBScotizaciones { get; set; }
        public virtual DbSet<CotizacionesProveedores> DBScotizacionesproveedores { get; set; }



        }

}
