using AQ.Api.Middleware;
using AQ.Infraestructura.BD.Context;
using AQ.Utilidades.Sesion;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace AQ.API
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Configura Cors
            services.AddCors(option => option.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            //Inicia MVC
            services.AddMvc();

            //Configura AutoFac para inyección de dependencias
            var builder = new ContainerBuilder();

            //Configura las principales clases
            services.AddScoped<IConstruirSesion, ConstruirSesion>();

            //Configura la inyección de contextos de base de datos
            try
            {
                builder.RegisterType<ContextoBD>();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al inicializar los contextos de bases de datos", ex);
            }

            //Configura inyección de interfaces
            try
            {
                builder.RegisterAssemblyTypes(Assembly.Load("AQ.Infraestructura"))
                    .Where(x => x.Namespace.Contains("Repositorios"))
                    .AsImplementedInterfaces();

                builder.RegisterAssemblyTypes(Assembly.Load("AQ.ServiciosAplicacion"))
                    .AsImplementedInterfaces();

                builder.Populate(services);
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al configurar la inyeción de dependencias", ex);
            }
           
            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //Aplica configuración de CORS
            app.UseCors("AllowAll");

            //Cargar middlawares para validacion de headers
            app.UseMiddleware(typeof(AuthenticationMilddleware));

            //Inicia API
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "api", template: "api/{controller}/{action}/{id?}");
            });



            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

        }
    }
}
