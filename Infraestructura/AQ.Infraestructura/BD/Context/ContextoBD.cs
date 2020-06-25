using AQ.Global;
using AQ.Utilidades.Sesion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace AQ.Infraestructura.BD.Context
{
    public class ContextoBD : DbContext
    {
        public IConstruirSesion construirSesion;

        public ContextoBD(IConstruirSesion _construirSesion)
            : base(_construirSesion.ObtenerContextOptions(new DbContextOptionsBuilder<ContextoBD>()))
        {
            construirSesion = _construirSesion;

            if ((Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                this.Database.Migrate();
            }
            else
            {
                throw new Exception("No se ha creado la base de datos");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
