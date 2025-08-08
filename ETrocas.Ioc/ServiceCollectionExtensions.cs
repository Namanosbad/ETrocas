using Asp.Versioning;
using ETrocas.Application.Interfaces;
using ETrocas.Application.Services.v1;
using ETrocas.Database;
using ETrocas.Database.Repository;
using ETrocas.Domain.Interfaces;
using ETrocas.Shared.Configuration;
using ETrocas.Shared.Interfaces;
using ETrocas.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

//Injeção de dependencia.
namespace ETrocas.Ioc
{
    public static class ServiceCollectionExtensions
    {
        //
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddAuthentication(configuration);
            services.AddApplicationServices();
            services.AddApiVersioning(configuration);
            return services;
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            //
            services.Configure<DbConfig>(config => configuration.GetRequiredSection(nameof(DbConfig)).Bind(config));

            //options 
            services.AddDbContext<ETrocasDbContext>((serviceProvider, options) =>
                {
                    var config = serviceProvider.GetRequiredService<IOptions<DbConfig>>().Value;
                    var connectionString = config.ConnectionString;
                    options.UseSqlServer(connectionString);
                });

            return services;
        }
        //
        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenConfig>(config => configuration.GetRequiredSection(nameof(TokenConfig)).Bind(config));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                var key = configuration["TokenConfig:Key"];
                var asciiKey = Encoding.ASCII.GetBytes(key);

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(asciiKey),
                    ValidateIssuer = false
                };
            });

            services.AddTransient<ITokenService, TokenService>();

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IProdutoService, ProdutoService>();
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

            return services;
        }

        public static IServiceCollection AddApiVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(o=>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                                     new QueryStringApiVersionReader(),
                                     new UrlSegmentApiVersionReader());

            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }
    }
}