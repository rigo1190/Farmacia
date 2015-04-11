namespace DataAccessLayer.Migrations
{
    using DataAccessLayer.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccessLayer.Models.Contexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataAccessLayer.Models.Contexto context)
        {
            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            return;
  
            
            

            context.Usuarios.AddOrUpdate(

               new Usuario { Id = 1, Login = "admin", Password = "admin", Nombre = "Administrador", Activo = true },
               new Usuario { Id = 2, Login = "usuario", Password = "usuario", Nombre = "Usuario", Activo = true},
               new Usuario { Id = 3, Login = "qwe", Password = "qwe", Nombre = "usuario de pruebas", Activo = true }
               
            );

           
  

            context.Ejercicios.AddOrUpdate(              
               new Ejercicio { Id = 1, Año = 2015, FactorIva = 1.6M, Estatus = enumEstatusEjercicio.Activo  }
            );

        


        

            context.Municipios.AddOrUpdate(              
              new Municipio {Id=1,Clave="M001",Nombre="Acajete" },
              new Municipio {Id=2,Clave="M002",Nombre="Acatlán" },
              new Municipio {Id=3,Clave="M003",Nombre="Acayucan" }             
            );

            
                    

          context.SaveChanges();

          
           
        }


        private void CrearTriggers(Contexto contexto)
        {

            string sp001 = @" CREATE TRIGGER trgAsignarNumeroObra_POADetalle ON [dbo].[POADetalle] 
                                FOR INSERT
                                AS
	                               
									 declare @consecutivo int;
						             declare @UnidadPresupuestalClave varchar(9);
						             declare @anio int;
						             declare @poadetalleId int;
						             declare @poaId int;
						             declare @numeroObra varchar(100);

						             select @poaId=POAId,@poadetalleId=Id from inserted; 

                                     select

                                         @consecutivo=MAX(POADetalle.Consecutivo),							  
							             @UnidadPresupuestalClave=UnidadPresupuestal.Clave,
							             @anio=Ejercicio.Año							   

                                     from POADetalle 
                                     inner join POA
                                     on POA.Id=POADetalle.POAId
                                     inner join UnidadPresupuestal
                                     on UnidadPresupuestal.Id=POA.UnidadPresupuestalId
                                     inner join Ejercicio
                                     on Ejercicio.Id=POA.EjercicioId
                                     where POA.Id=@poaId
							         group by POA.Id,UnidadPresupuestal.Clave,Ejercicio.Año
                            
                            set @consecutivo=@consecutivo+1;                                     
							
							set @numeroObra= CAST(@UnidadPresupuestalClave AS varchar(9))  + CAST(@anio AS varchar(4)) + REPLACE(STR(@consecutivo, 3),SPACE(1),'0');

                            update POADetalle set Consecutivo=@consecutivo,Numero=@numeroObra where Id=@poadetalleId";



            contexto.Database.ExecuteSqlCommand(sp001);


            sp001 = @"CREATE TRIGGER trgAsignarNumeroObra_Obra ON [dbo].[Obra] 
                                FOR INSERT
                                AS	                               									 
						             declare @consecutivo int;
						             declare @obraId int;
						             declare @poaDetalleId int;
						             declare @numeroObra varchar(100);

						             select @poaDetalleId=POADetalleId,@obraId=Id from inserted; 

                                     select @consecutivo=Consecutivo,@numeroObra=Numero from POADetalle where Id=@poaDetalleId     
                        
                                update Obra set Consecutivo=@consecutivo,Numero=@numeroObra where Id=@obraId";



            contexto.Database.ExecuteSqlCommand(sp001);           




        } // Triggers






    }
}
