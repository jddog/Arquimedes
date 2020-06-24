using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQ.Infraestructura.BD.Context
{
    public ContextoConfiguracion(HttpContextAccessor.HttpContext.Items["_cadenaConexion"].ToString())
           : base(options)
    {
    }
    public ContextoConfiguracion(IConstruirSesion construirSesion)
        : base(construirSesion.ObtenerContextOptions(new DbContextOptionsBuilder<ContextoConfiguracion>()))
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
}
