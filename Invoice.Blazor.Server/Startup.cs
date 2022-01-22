using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Security;
using DevExpress.Blazor.Reporting;
using DevExpress.ExpressApp.ReportsV2.Blazor;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Invoice.Blazor.Server.Services;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.Identity.Web;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DevExpress.ExpressApp.Dashboards.Blazor;
using DevExpress.ExpressApp.Office.Blazor;

namespace Invoice.Blazor.Server
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
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();
            services.AddSingleton<XpoDataStoreProviderAccessor>();
            services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
            services.AddXaf<InvoiceBlazorApplication>(Configuration);
            services.AddXafReporting();
            services.AddXafDashboards();
            services.AddXafOffice();
            services.AddXafSecurity(options =>
            {
                options.RoleType = typeof(PermissionPolicyRole);
                options.UserType = typeof(Module.BusinessObjects.ApplicationUser);
                options.UserLoginInfoType = typeof(Module.BusinessObjects.ApplicationUserLoginInfo);
                options.Events.OnSecurityStrategyCreated = securityStrategy => ((SecurityStrategy)securityStrategy).RegisterXPOAdapterProviders();
                options.SupportNavigationPermissionsForTypes = false;
            })
            .AddAuthenticationStandard(options =>
            {
                options.IsSupportChangePassword = true;
            })
            .AddExternalAuthentication<HttpContextPrincipalProvider>();
            var authentication = services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
            authentication
                .AddCookie(options =>
                {
                    options.LoginPath = "/LoginPage";
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Authentication:Jwt:Issuer"],
                        ValidAudience = Configuration["Authentication:Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:Jwt:IssuerSigningKey"]))
                    };
                });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DXApplication1 API",
                    Version = "v1",
                    Description = @"Use AddXafWebApi(options) in the DXApplication1.Blazor.Server\Startup.cs file to make Business Objects available in the Web API."
                });
                c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.Http,
                    Name = "Bearer",
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme() {
                                Reference = new OpenApiReference() {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "JWT"
                                }
                            },
                            new string[0]
                        },
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DXApplication1 WebApi v1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseXaf();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllers();
            });
        }
    }
}
