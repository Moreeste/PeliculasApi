using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasApi.Helpers;
using PeliculasApi.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeliculasApi
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
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped(Factorias.AlmacenadorArchivosService);

            services.AddHttpContextAccessor();

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
            sqlServerOptions => sqlServerOptions.UseNetTopologySuite()));

            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

            services.AddScoped<PeliculaExisteAttribute>();

            services.AddSingleton(provider =>

               new MapperConfiguration(config =>
               {
                   var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                   config.AddProfile(new AutoMapperProfiles(geometryFactory));
               }).CreateMapper()
           );

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(FiltroErrores));
            })
                .AddNewtonsoftJson();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["JWT:key"])),
                    ClockSkew = TimeSpan.Zero
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class Factorias
    {
        public static IAlmacenadorArchivos AlmacenadorArchivosService(IServiceProvider serviceProvider)
        {
            var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();

            if (env.IsDevelopment())
            {
                var httpContextAccesor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                return new AlmacenadorArchivosLocal(env, httpContextAccesor);
            }
            else
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                return new AlmacenadorArchivosAzure(configuration);
            }
        }
    }
}
