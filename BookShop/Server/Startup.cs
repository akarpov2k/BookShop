using BookShop.Server.Data;
using BookShop.Server.Models;
using BookShop.Server.SystemModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShop.Server
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services )
        {
            var appSettings = Configuration.GetSection( "AppSettings" ).Get<AppSettings>() ?? new AppSettings();
            services.AddSingleton(appSettings);
            services.AddDbContext<ApplicationDbContext>( options =>
                 options.UseSqlServer(
                     Configuration.GetConnectionString( "DefaultConnection" ) ) );

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>( options => options.SignIn.RequireConfirmedAccount = false )
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options => {
                    options.IdentityResources["openid"].UserClaims.Add( "name" );
                    options.ApiResources.Single().UserClaims.Add( "name" );
                    options.IdentityResources["openid"].UserClaims.Add( "role" );
                    options.ApiResources.Single().UserClaims.Add( "role" );
                } );
            services.AddTransient<IProfileService, ProfileService>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove( "role" );
            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler( "/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints( endpoints =>
             {
                 endpoints.MapRazorPages();
                 endpoints.MapControllers();
                 endpoints.MapFallbackToFile( "index.html" );
             } );

            CreateRoles( provider ).GetAwaiter().GetResult();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManger = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var isAdminExist = await roleManger.RoleExistsAsync( "Admin" );
            if( !isAdminExist )
            {
                var role = new IdentityRole( "Admin" );
                role.Id = "admin";
                if (role!= null )
                {
                    var result = await roleManger.CreateAsync( role );
                    if( result.Succeeded )
                    {
                        var claim = new Claim( "role", role.Name );
                        await roleManger.AddClaimAsync( role, claim );
                    }
                }
                
            }
        }
    }
}
