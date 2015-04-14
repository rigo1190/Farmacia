using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace BusinessLogicLayer
{
    public class UnitOfWork : IDisposable
    {
        private Contexto contexto;

        public Contexto Contexto
        {
            get { return contexto; }            
        }
        private int usuarioId;
        private List<String> errors = new List<String>();
        private IBusinessLogic<Usuario> usuarioBusinessLogic;
        private IBusinessLogic<Ejercicio> ejercicioBusinessLogic;
        private IBusinessLogic<Municipio> municipioBusinessLogic;


        private IBusinessLogic<Proveedores> proveedoresBL;
        private IBusinessLogic<GruposPS> grupospsBL;
        private IBusinessLogic<Articulos> artiulosBL;
        private IBusinessLogic<UnidadesDeMedida> unidadesdemedidaBL;
        private IBusinessLogic<Servicios> serviciosBL;

        private IBusinessLogic<Pedidos> pedidosBL;
        private IBusinessLogic<PedidosArticulos> pedidosarticulosBL;
        private IBusinessLogic<PedidosServicios> pedidosserviciosBL;

        private IBusinessLogic<Clientes> clientesBL;
        private IBusinessLogic<Ventas> ventasBL;
        private IBusinessLogic<VentasArticulos> ventasarticulosBL;

        private IBusinessLogic<InventarioInicial> inventarioinicialBL;
        private IBusinessLogic<InventarioInicialArticulos> inventarioinicialarticulosBL;
        private IBusinessLogic<InventarioInicialArticulosCostos> inventarioinicialarticuloscostosBL;


        private IBusinessLogic<AlmacenEntradasGenericas> almacenentradasgenericasBL;
        private IBusinessLogic<AlmacenEntradasGenericasArticulos> almacenentradasgenericasarticulosBL;
        private IBusinessLogic<AlmacenSalidasGenericas> almacensalidasgenericasBL;
        private IBusinessLogic<AlmacenSalidasGenericasArticulos> almacensalidasgenericasarticulosBL;

        private IBusinessLogic<FacturasAlmacen> facturasalmacenBL;
        private IBusinessLogic<FacturasAlmacenArticulos> facturasalmacenarticulosBL;
        private IBusinessLogic<FacturasAlmacenArticulosTMP> facturasalmacenarticulostmpBL;

        private IBusinessLogic<ArticulosMovimientos> articulosmovimientosBL;
        private IBusinessLogic<ArticulosMovimientosEntradas> articulosmovimientosentradasBL;
        private IBusinessLogic<ArticulosMovimientosSalidas> articulosmovimientossalidasBL;



        private IBusinessLogic<Laboratorios> laboratoriosBL;
        private IBusinessLogic<Marcas> marcasBL;
        private IBusinessLogic<Lineas> lineasBL;
        private IBusinessLogic<Presentaciones> presentacionesBL;
        private IBusinessLogic<FPSfactores> fpsfactoresBL;
        private IBusinessLogic<Sexos> sexosBL;

        private IBusinessLogic<Recetas> recetasBL;
        private IBusinessLogic<RecetaArticulos> recetasArticulosBL;
        private IBusinessLogic<ArticuloVenta> articuloVentaBL;
        private IBusinessLogic<ArticuloSalidaGenerica> articuloSalidaGenericaBL;
        private IBusinessLogic<TipoSalida> tipoSalidaBL;

        private IBusinessLogic<Cotizaciones> cotizacionesBL;
        private IBusinessLogic<CotizacionesProveedores> cotizacionesproveedoresBL;


        public UnitOfWork()
        {
            this.contexto = new Contexto();
        }

        public UnitOfWork(string usuarioId)
        {           
            this.usuarioId = Utilerias.StrToInt(usuarioId);
            this.contexto = new Contexto();
        }

        public IBusinessLogic<TipoSalida> TipoSalidaBusinessLogic
        {
            get
            {
                if (this.tipoSalidaBL == null)
                {
                    this.tipoSalidaBL = new GenericBusinessLogic<TipoSalida>(contexto);
                }

                return tipoSalidaBL;
            }
        }

        public IBusinessLogic<ArticuloSalidaGenerica> ArticuloSalidaGenericaBusinessLogic
        {
            get
            {
                if (this.articuloSalidaGenericaBL == null)
                {
                    this.articuloSalidaGenericaBL = new GenericBusinessLogic<ArticuloSalidaGenerica>(contexto);
                }

                return articuloSalidaGenericaBL;
            }
        }

        public IBusinessLogic<ArticuloVenta> ArticuloVentaBusinessLogic
        {
            get
            {
                if (this.articuloVentaBL == null)
                {
                    this.articuloVentaBL = new GenericBusinessLogic<ArticuloVenta>(contexto);
                }

                return articuloVentaBL;
            }
        }

        public IBusinessLogic<RecetaArticulos> RecetasArticulosBusinessLogic
        {
            get
            {
                if (this.recetasArticulosBL == null)
                {
                    this.recetasArticulosBL = new GenericBusinessLogic<RecetaArticulos>(contexto);
                }

                return recetasArticulosBL;
            }
        }

        public IBusinessLogic<Recetas> RecetasBusinessLogic
        {
            get
            {
                if (this.recetasBL == null)
                {
                    this.recetasBL = new GenericBusinessLogic<Recetas>(contexto);
                }

                return recetasBL;
            }
        }

        

        public IBusinessLogic<Usuario> UsuarioBusinessLogic
        {
            get
            {
                if (this.usuarioBusinessLogic == null)
                {
                    this.usuarioBusinessLogic = new GenericBusinessLogic<Usuario>(contexto);
                }

                return usuarioBusinessLogic;
            }
        }

    



        public IBusinessLogic<Ejercicio> EjercicioBusinessLogic
        {
            get
            {
                if (this.ejercicioBusinessLogic == null)
                {
                    this.ejercicioBusinessLogic = new GenericBusinessLogic<Ejercicio>(contexto);
                }

                return ejercicioBusinessLogic;
            }
        }


        public IBusinessLogic<Municipio> MunicipioBusinessLogic
        {
            get
            {
                if (this.municipioBusinessLogic == null)
                {
                    this.municipioBusinessLogic = new GenericBusinessLogic<Municipio>(contexto);
                }

                return municipioBusinessLogic;
            }
        }





        public IBusinessLogic<Proveedores> ProveedoresBL
        {
            get
            {
                if (this.proveedoresBL == null)
                {
                    this.proveedoresBL = new GenericBusinessLogic<Proveedores>(contexto);
                }
                return this.proveedoresBL;
            }
        }

        public IBusinessLogic<GruposPS> GruposPSBL
        {
            get
            {
                if (this.grupospsBL == null)
                {
                    this.grupospsBL = new GenericBusinessLogic<GruposPS>(contexto);
                }
                return this.grupospsBL;
            }
        }

        public IBusinessLogic<Articulos> ArticulosBL
        {
            get
            {
                if (this.artiulosBL == null)
                {
                    this.artiulosBL = new GenericBusinessLogic<Articulos>(contexto);
                }
                return this.artiulosBL;
            }
        }

        public IBusinessLogic<UnidadesDeMedida> UnidadesDeMedidaBL
        {
            get
            {
                if (this.unidadesdemedidaBL == null)
                {
                    this.unidadesdemedidaBL = new GenericBusinessLogic<UnidadesDeMedida>(contexto);
                }
                return this.unidadesdemedidaBL;
            }
        }

        public IBusinessLogic<Servicios> ServiciosBL
        {
            get
            {
                if (this.serviciosBL == null)
                {
                    this.serviciosBL = new GenericBusinessLogic<Servicios>(contexto);
                }
                return this.serviciosBL;
            }
        }



        public IBusinessLogic<Pedidos> PedidosBL
        {
            get
            {
                if (this.pedidosBL == null)
                {
                    this.pedidosBL = new GenericBusinessLogic<Pedidos>(contexto);
                }
                return this.pedidosBL;
            }
        }


        public IBusinessLogic<PedidosArticulos> PedidosArticulosBL
        {
            get
            {
                if (this.pedidosarticulosBL == null)
                {
                    this.pedidosarticulosBL = new GenericBusinessLogic<PedidosArticulos>(contexto);
                }
                return this.pedidosarticulosBL;
            }
        }


        public IBusinessLogic<PedidosServicios> PedidosServiciosBL
        {
            get
            {
                if (this.pedidosserviciosBL == null)
                {
                    this.pedidosserviciosBL = new GenericBusinessLogic<PedidosServicios>(contexto);
                }
                return this.pedidosserviciosBL;
            }
        }


        public IBusinessLogic<Clientes> ClientesBL
        {
            get
            {
                if (this.clientesBL == null)
                {
                    this.clientesBL = new GenericBusinessLogic<Clientes>(contexto);
                }
                return this.clientesBL;
            }
        }



        public IBusinessLogic<Ventas> VentasBL
        {
            get
            {
                if (this.ventasBL == null)
                {
                    this.ventasBL = new GenericBusinessLogic<Ventas>(contexto);
                }
                return this.ventasBL;
            }
        }

        public IBusinessLogic<VentasArticulos> VentasArticulosBL
        {
            get
            {
                if (this.ventasarticulosBL == null)
                {
                    this.ventasarticulosBL = new GenericBusinessLogic<VentasArticulos>(contexto);
                }
                return this.ventasarticulosBL;
            }
        }




        public IBusinessLogic<InventarioInicial> InventarioInicialBL
        {
            get
            {
                if (this.inventarioinicialBL == null)
                {
                    this.inventarioinicialBL = new GenericBusinessLogic<InventarioInicial>(contexto);
                }
                return this.inventarioinicialBL;
            }
        }

        public IBusinessLogic<InventarioInicialArticulos> InventarioInicialArticulosBL
        {
            get
            {
                if (this.inventarioinicialarticulosBL == null)
                {
                    this.inventarioinicialarticulosBL = new GenericBusinessLogic<InventarioInicialArticulos>(contexto);
                }
                return this.inventarioinicialarticulosBL;
            }
        }

 
        public IBusinessLogic<InventarioInicialArticulosCostos> InventarioInicialArticulosCostosBL
        {
            get
            {
                if (this.inventarioinicialarticuloscostosBL == null)
                {
                    this.inventarioinicialarticuloscostosBL = new GenericBusinessLogic<InventarioInicialArticulosCostos>(contexto);
                }
                return this.inventarioinicialarticuloscostosBL;
            }
        }



        public IBusinessLogic<AlmacenEntradasGenericas> AlmacenEntradasGenericasBL
        {
            get
            {
                if (this.almacenentradasgenericasBL == null)
                {
                    this.almacenentradasgenericasBL = new GenericBusinessLogic<AlmacenEntradasGenericas>(contexto);
                }
                return this.almacenentradasgenericasBL;
            }
        }
        public IBusinessLogic<AlmacenEntradasGenericasArticulos> AlmacenEntradasGenericasArticulosBL
        {
            get
            {
                if (this.almacenentradasgenericasarticulosBL == null)
                {
                    this.almacenentradasgenericasarticulosBL = new GenericBusinessLogic<AlmacenEntradasGenericasArticulos>(contexto);
                }
                return this.almacenentradasgenericasarticulosBL;
            }
        }



        public IBusinessLogic<AlmacenSalidasGenericas> AlmacenSalidasGenericasBL
        {
            get
            {
                if (this.almacensalidasgenericasBL == null)
                {
                    this.almacensalidasgenericasBL = new GenericBusinessLogic<AlmacenSalidasGenericas>(contexto);
                }
                return this.almacensalidasgenericasBL;
            }
        }
        public IBusinessLogic<AlmacenSalidasGenericasArticulos> AlmacenSalidasGenericasArticulos
        {
            get
            {
                if (this.almacensalidasgenericasarticulosBL == null)
                {
                    this.almacensalidasgenericasarticulosBL = new GenericBusinessLogic<AlmacenSalidasGenericasArticulos>(contexto);
                }
                return this.almacensalidasgenericasarticulosBL;
            }
        }



        public IBusinessLogic<FacturasAlmacen> FacturasAlmacenBL
        {
            get
            {
                if (this.facturasalmacenBL == null)
                {
                    this.facturasalmacenBL = new GenericBusinessLogic<FacturasAlmacen>(contexto);
                }
                return this.facturasalmacenBL;
            }
        }
        public IBusinessLogic<FacturasAlmacenArticulos> FacturasAlmacenArticulosBL
        {
            get
            {
                if (this.facturasalmacenarticulosBL == null)
                {
                    this.facturasalmacenarticulosBL = new GenericBusinessLogic<FacturasAlmacenArticulos>(contexto);
                }
                return this.facturasalmacenarticulosBL;
            }
        }

        public IBusinessLogic<FacturasAlmacenArticulosTMP> FacturasAlmacenArticulosTMPBL
        {
            get
            {
                if(this.facturasalmacenarticulostmpBL==null){
                    this.facturasalmacenarticulostmpBL = new GenericBusinessLogic<FacturasAlmacenArticulosTMP>(contexto);
                }
                return this.facturasalmacenarticulostmpBL;
            }
        }


        public IBusinessLogic<ArticulosMovimientos> ArticulosMovimientosBL
        {
            get
            {
                if (this.articulosmovimientosBL == null)
                {
                    this.articulosmovimientosBL = new GenericBusinessLogic<ArticulosMovimientos>(contexto);
                }
                return this.articulosmovimientosBL;
            }
        }
        public IBusinessLogic<ArticulosMovimientosEntradas> ArticulosMovimientosEntradasBL
        {
            get
            {
                if (this.articulosmovimientosentradasBL == null)
                {
                    this.articulosmovimientosentradasBL = new GenericBusinessLogic<ArticulosMovimientosEntradas>(contexto);
                }
                return this.articulosmovimientosentradasBL;
            }
        }
        public IBusinessLogic<ArticulosMovimientosSalidas> ArticulosMovimientosSalidasBL
        {
            get
            {
                if (this.articulosmovimientossalidasBL == null)
                {
                    this.articulosmovimientossalidasBL = new GenericBusinessLogic<ArticulosMovimientosSalidas>(contexto);
                }
                return this.articulosmovimientossalidasBL;
            }
        }


        public IBusinessLogic<Laboratorios> LaboratoriosBL
        {
            get
            {
                if (this.laboratoriosBL == null)
                {
                    this.laboratoriosBL = new GenericBusinessLogic<Laboratorios>(contexto);
                }
                return this.laboratoriosBL;
            }
        }



        public IBusinessLogic<Marcas> MarcasBL
        {
            get
            {
                if (this.marcasBL == null)
                {
                    this.marcasBL = new GenericBusinessLogic<Marcas>(contexto);
                }
                return this.marcasBL;
            }
        }


        public IBusinessLogic<Lineas> LineasBL
        {
            get
            {
                if (this.lineasBL == null)
                {
                    this.lineasBL = new GenericBusinessLogic<Lineas>(contexto);
                }
                return this.lineasBL;
            }
        }


        public IBusinessLogic<Presentaciones> PresentacionesBL
        {
            get
            {
                if (this.presentacionesBL == null)
                {
                    this.presentacionesBL = new GenericBusinessLogic<Presentaciones>(contexto);
                }
                return this.presentacionesBL;
            }
        }


        public IBusinessLogic<FPSfactores> FPSfactoresBL
        {
            get
            {
                if (this.fpsfactoresBL == null)
                {
                    this.fpsfactoresBL = new GenericBusinessLogic<FPSfactores>(contexto);
                }
                return this.fpsfactoresBL;
            }
        }



        public IBusinessLogic<Sexos> SexosBL
        {

            get
            {
                if (this.sexosBL == null)
                {
                    this.sexosBL = new GenericBusinessLogic<Sexos>(contexto);
                }
                return this.sexosBL;
            }
        }




        public IBusinessLogic<Cotizaciones> CotizacionesBL
        {
            get
            {
                if (this.cotizacionesBL == null)
                {
                    this.cotizacionesBL = new GenericBusinessLogic<Cotizaciones>(contexto);
                }
                return this.cotizacionesBL;
            }
        }



        public IBusinessLogic<CotizacionesProveedores> CotizacionesProveedoresBL
        {
            get
            {
                if (this.cotizacionesproveedoresBL == null)
                {
                    this.cotizacionesproveedoresBL = new GenericBusinessLogic<CotizacionesProveedores>(contexto);
                }
                return this.cotizacionesproveedoresBL;
            }
        }

        public void SaveChanges()
        {
            try
            {
                errors.Clear();
                contexto.SaveChanges(usuarioId);
            }
            catch (DbEntityValidationException ex)
            {

                this.RollBack();

                foreach (var item in ex.EntityValidationErrors)
                {

                    errors.Add(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors", item.Entry.Entity.GetType().Name, item.Entry.State));

                    foreach (var error in item.ValidationErrors)
                    {
                        errors.Add(String.Format("Propiedad: \"{0}\", Error: \"{1}\"", error.PropertyName, error.ErrorMessage));
                    }


                }

            }
            catch (DbUpdateException ex)
            {
                this.RollBack();
                errors.Add(String.Format("{0}", ex.InnerException.InnerException.Message));
            }
            catch (System.InvalidOperationException ex)
            {
                this.RollBack();
                errors.Add(String.Format("{0}", ex.Message));
            }
            catch (Exception ex)
            {
                this.RollBack();
                errors.Add(String.Format("{0}\n{1}", ex.Message, ex.InnerException.Message));
            }
            
        }

        public void RollBack()
        {

            var changedEntries = contexto.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged);

            // ojo....... respetar el orden en que se evalua el atributo State
            // primero Deleted,segundo Modified, al final y nada mas que al final Added
            // debido a que una vez que asignamos el estado Detached a una entidad,
            // desasociamos a esta del contexto y el filtro changedEntries.Where(etc....) genera un error

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Deleted))
            {
                entry.State = EntityState.Unchanged;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Modified))
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Added))
            {
                entry.State = EntityState.Detached;
            }                       

        }

        public List<String> Errors 
        {
            get 
            {
                return errors;
            }
        }

        public object GetResult() 
        {
            if (errors.Count == 0) 
            {
                return new { OK = true };
            }

            return new  { OK = false, Errors = errors };
        }
        
        
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    contexto.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
    
}
