using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Api_Wathsapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api_Wathsapp.Util.Implementation;
using Api_Wathsapp.Util.Interfaces;

namespace Api_Wathsapp
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
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            services.AddCors();

            string Motor = Configuration.GetConnectionString("motor");
            string cn = "";
            int timeout = 60;
            if (Motor == "MSSQL")
            {
                cn = Configuration.GetConnectionString("cnMsSQL");
                timeout = int.Parse(Configuration.GetConnectionString("TimeOut"));
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                    cn,
                    sqlServerOptions => sqlServerOptions.CommandTimeout(timeout))
                );
            }
            else if (Motor == "MYSQL")
            {
                //var serverVersion = new MySqlServerVersion(new Version(8, 0, 23)); // Get the value from SELECT VERSION()
                //cn = Configuration.GetConnectionString("cnMySQL");
                //services.AddDbContext<RecrutingProdContext>(options =>
                //options.UseMySql(cn, serverVersion)
                //);
            }
            else if (Motor == "ORACLE")
            {
                //cn = Configuration.GetConnectionString("cnORACLE");
                //services.AddDbContext<RecrutingProdContext>(options =>
                //options.UseOracle(cn)
                //);
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = "yourdomain.com",
                      ValidAudience = "yourdomain.com",
                      IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(Configuration["Llave_super_secreta"])),
                      ClockSkew = TimeSpan.Zero
                  });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api_Wathsapp", Version = "v1" });
            });

            //Registramos las interfaces
            services.AddScoped<ILoginService, LoginService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api_Wathsapp v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
