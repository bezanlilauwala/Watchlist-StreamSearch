using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI3.Identity;
using WebAPI3.Models;
using Microsoft.AspNetCore.Cors;

namespace WebAPI3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //configuration gives you access to the appsettings.json file
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add DB ConnectionString's
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("IdentityConnection"), providerOptions => providerOptions.EnableRetryOnFailure()));

            //.EnableRetryOnFailure()

            services.AddDbContext<WatchlistDBContext>(options => 
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"), providerOptions => providerOptions.EnableRetryOnFailure()));

            //AddIdentityCore configures the identity requirements (password must be alphnumeric, more than 6 characters, contain a digit & an uppercase character)
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            /*
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();
            */

            //Registers services required by authentication services 
            services.AddAuthentication(Opt =>
            {
                //These are part of the Jwt standard, Sets the default authentication scheme of the 
                Opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(Opt =>
                {
                    //TokenValidationParameters (validation token is given to the client after logging in and is used for every request)
                    Opt.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,     //This is the main one: typically in the config file

                        ValidIssuer = "http://localhost:5000",
                        ValidAudience = "http://localhost:5000",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Csun586@12:00PMS#cretKey"))
                    };
                });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI3", Version = "v1" });
                
                //Add JWT Token Security Scheme for Authorization
                OpenApiSecurityScheme jwtSecurityScheme = new()
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "PUT **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            //Adding CORS Service
            //services.AddCors();
            services.AddCors(OptionsBuilderConfigurationExtensions => 
              OptionsBuilderConfigurationExtensions.AddPolicy("AllowEverything", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            //SetIsOriginAllowed(_ => true)

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCors(options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());

            //Swagger is only being used if the development environment is Development
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI3 v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowEverything");
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
