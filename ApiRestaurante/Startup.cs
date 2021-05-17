using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRestaurante.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiRestaurante
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<UsuarioRepository>();
            services.AddScoped<PtoVtaRepository>();
            services.AddScoped<SucursalRepository>();
            services.AddScoped<BodegaRepository>();
            services.AddScoped<LineaRepository>();
            services.AddScoped<PisoRepository>();
            services.AddScoped<UsuarioPisoRepository>();
            services.AddScoped<MesaRepository>();
            services.AddScoped<ItemUnidadRepository>();
            services.AddScoped<DetPedidoRepository>();
            services.AddScoped<PedidoRepository>();
            services.AddScoped<ClienteRepository>();
            services.AddScoped<ImpresoraRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
