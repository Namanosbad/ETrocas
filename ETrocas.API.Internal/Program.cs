using ETrocas.API.Internal.Middleware;
using ETrocas.Ioc;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ETrocas",
        Description = "API para gestão de trocas, produtos, propostas e autenticação de usuários.",
        TermsOfService = new Uri("https://github.com/Namanosbad"),
        Contact = new OpenApiContact
        {
            Name = "Matheus Lima",
            Email = "matheus.limamst@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/matheuslimamst/"),
        }
    });

    var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
    if (File.Exists(apiXmlPath))
    {
        c.IncludeXmlComments(apiXmlPath, includeControllerXmlComments: true);
    }

    var applicationXmlFile = "ETrocas.Application.xml";
    var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXmlFile);
    if (File.Exists(applicationXmlPath))
    {
        c.IncludeXmlComments(applicationXmlPath);
    }

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT no formato: Bearer {seu_token}."
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddServices(builder.Configuration);


var app = builder.Build();

app.UseApiExceptionHandling();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ETrocas");
    });

app.UseHttpsRedirection();

app.UseCors("RestrictedCors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
