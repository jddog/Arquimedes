using AQ.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace AQ.Utilidades.Sesion
{

    public interface IConstruirSesion
    {
        DbContextOptions ObtenerContextOptions(DbContextOptionsBuilder builder);
        Sesion ObtenerSesion();

    }

    public class ConstruirSesion : IConstruirSesion
    {
        readonly IConfiguration configuration;
        public ConstruirSesion(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public DbContextOptions ObtenerContextOptions(DbContextOptionsBuilder builder)
        {
            var sesion = ObtenerSesion();
            builder.UseSqlServer(
                sesion.cadenaConexion,
                x => x.MigrationsHistoryTable(ConstantesBD.DBMigrationHistoryTable, ConstantesBD.DBSchemaConfiguration)
            );
            return builder.Options;
        }

        public Sesion ObtenerSesion()
        {

            Sesion sesion = new Sesion()
            {
                cadenaConexion = configuration.GetConnectionString("DevelopConnection"),
            };
            return sesion;
        }
    }
}
