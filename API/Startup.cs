using Microsoft.EntityFrameworkCore;
using Entidades;
//using Controllers;
using API.DTOs;
using API.Servicios;
using API.Helpers;
using Serilog;
using NetTopologySuite.Geometries;
using NetTopologySuite;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using API.Helpers.Attributes;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            
            //Para hacer el mapeo a los DTOs
            services.AddAutoMapper(typeof(AutoMapperProfiles));

            //Para el servicio de guardar los archivos en el wwwroot         
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();

            services.AddHttpContextAccessor();

            //Servicios para poder usar GeomtryFactory con inyeccion de dependencia
            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
            services.AddSingleton(provider =>
            
                new MapperConfiguration(confi =>
                {
                    var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                    confi.AddProfile(new AutoMapperProfiles(geometryFactory));
                }).CreateMapper()

            );


           
     





            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaulConnection"),
                sqlServerOptions => sqlServerOptions.UseNetTopologySuite()
                ));




            services.AddControllers().AddNewtonsoftJson();

            

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                        ClockSkew = TimeSpan.Zero
                    }
                 );


            services.AddScoped<PrestamoExisteAttribute>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            //            services.AddSwaggerGen();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();



            //para poder ver las imagenes guardades mediante la ruta url en el navegados
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
                                 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

          


        }












    }
}
 