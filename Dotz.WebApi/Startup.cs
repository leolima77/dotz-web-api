using AutoMapper;
using Dotz.Common.Paging;
using Dotz.Common.Paging.Interface;
using Dotz.Common.UnitofWork;
using Dotz.Data.Context;
using Dotz.Data.Entities;
using Dotz.WebApi.Controllers;
using Dotz.WebApi.Interfaces;
using Dotz.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dotz.WebApi
{
    public class Startup
    {
        #region Variables

        private IConfiguration _config { get; }

        #endregion Variables

        #region Constructor

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        #endregion Constructor

        public void ConfigureServices(IServiceCollection services)
        {
            #region JwtTokenSection

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication()
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;

                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(5),
                    RoleClaimType = "Roles",
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Tokens:Issuer"],
                    ValidateIssuer = true,
                    ValidAudience = _config["Tokens:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                };
            });

            services.ConfigureApplicationCookie(options => options.LoginPath = "/api/Token");

            #endregion

            #region LoggingSection

            services.AddLogging(builder => builder.AddFile(options =>
            {
                options.FileName = _config["Logging:Options:FilePrefix"]; 
                options.LogDirectory = _config["Logging:Options:LogDirectory"]; 
                options.FileSizeLimit = int.Parse(_config["Logging:Options:FileSizeLimit"]); 
            }));

            #endregion LoggingSection

            #region ApplicationDbContextSection

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(_config["ConnectionStrings:DefaultConnection"]).UseLazyLoadingProxies();
            });
            services.AddScoped<DbContext, ApplicationDbContext>();

            #endregion ApplicationDbContextSection

            #region DependencyInjectionSection

            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddTransient(typeof(IPagingLinks<>), typeof(PagingLinks<>));
            services.AddScoped<ILanguageController, LanguageController>();
            services.AddScoped<IUserController, UserController>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()
                                           .ActionContext;
                return new UrlHelper(actionContext);
            });

            #endregion DependencyInjectionSection

            #region IdentitySection

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddRoleManager<RoleManager<ApplicationRole>>()
                    .AddUserManager<UserManager<ApplicationUser>>()
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            });

            #endregion IdentitySection

            #region AutoMapperSection

            services.AddAutoMapper();

            #endregion AutoMapperSection

            #region CorsSection

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    //.WithOrigins("http://localhost:4200")
                    .AllowAnyOrigin()
                    //.WithMethods("GET", "POST")
                    .AllowAnyMethod()
                    //.WithHeaders("accept", "content-type", "origin", "No-Auth")
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            #endregion CorsSection

            #region CacheSection
            services.AddMemoryCache();
            services.AddResponseCaching();
            #endregion

            #region MvcSection

            services.AddMvc();

            #endregion MvcSection

            #region SwaggerSection

            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\temp-keys\"))
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Dotz.WebApi",
                    Description = "App test for Dotz",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Leo Lima",
                        Email = "leolima77@gmail.com",
                        Url = "https://leolima77.com.br"
                    }
                });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.OperationFilter<HeaderFiltersForSwagger>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region EnvironmentSection

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #endregion EnvironmentSection

            #region IdentitySection

            app.UseAuthentication();

            #endregion IdentitySection

            #region CorsSection

            app.UseCors("CorsPolicy");

            #endregion CorsSection

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });


            #region MvcSection

            app.UseStatusCodePages();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller}/{action}");
            });

            #endregion MvcSection
        }
    }
}