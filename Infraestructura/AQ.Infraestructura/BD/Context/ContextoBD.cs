using AQ.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace AQ.Infraestructura.BD.Context
{
    public class ContextoBD : DbContext
    {

        //readonly IConfiguration configuration;

        public ContextoBD()
            : base(ObtenerContextOptions(new DbContextOptionsBuilder<ContextoBD>()))
        {
            if ((Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                this.Database.Migrate();
            }
            else
            {
                throw new Exception("No se ha creado la base de datos");
            }
        }

        private static DbContextOptions ObtenerContextOptions(DbContextOptionsBuilder builder)
        {
            string cadenaConexion = "";//configuration.GetConnectionString("DevelopConnection");
            builder.UseSqlServer(
               cadenaConexion,
               x => x.MigrationsHistoryTable(ConstantesBD.DBMigrationHistoryTable, ConstantesBD.DBSchemaConfiguration)
           );
            return builder.Options;
        }
    }
}
