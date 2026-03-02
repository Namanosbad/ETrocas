using Asp.Versioning;
using AutoMapper;
using ETrocas.Application.Interfaces;
using ETrocas.Application.Services.v1;
using Etrocas.Application.Mappings;
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

namespace ETrocas.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddCorsPolicy(configuration);
            services.AddAuthentication(configuration);
            services.AddApplicationServices();
            services.AddAutoMapper(typeof(PropostaMappingProfile).Assembly);
            services.AddApiVersioning(configuration);
            return services;
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DbConfig>(config => configuration.GetRequiredSection(nameof(DbConfig)).Bind(config));

            services.AddDbContext<ETrocasDbContext>((serviceProvider, options) =>
            {
                var config = serviceProvider.GetRequiredService<IOptions<DbConfig>>().Value;
                var connectionString = config.ConnectionString;
                options.UseSqlServer(connectionString);
            });

            return services;
        }

        private static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CorsConfig>(config =>
                configuration.GetSection(nameof(CorsConfig)).Bind(config));

            var allowedOrigins = configuration
                .GetSection($"{nameof(CorsConfig)}:{nameof(CorsConfig.AllowedOrigins)}")
                .Get<string[]>() ?? [];

            services.AddCors(options =>
            {
                options.AddPolicy("RestrictedCors", policy =>
                {
                    if (allowedOrigins.Length > 0)
                    {
                        policy.WithOrigins(allowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                });
            });

            return services;
        }

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
                var asciiKey = Encoding.ASCII.GetBytes(key!);

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(asciiKey),
                    ValidateIssuer = false
                };
            });

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IProdutoService, ProdutoService>();
            services.AddScoped(typeof(IPropostaRepository), typeof(PropostaRepository));
            services.AddScoped<IPropostaService, PropostaService>();
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

            return services;
        }

        public static IServiceCollection AddApiVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(o =>
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
