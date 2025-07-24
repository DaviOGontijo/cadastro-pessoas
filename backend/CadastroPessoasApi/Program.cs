using CadastroPessoasApi.Data;
using CadastroPessoasApi.Repositories;
using CadastroPessoasApi.Configurations;
using CadastroPessoasApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "cadastro.db");
var connectionString = $"Data Source={dbPath}";

// Configuração do DbContext com SQLite
builder.Services.AddDbContext<PessoaDbContext>(options =>
    options.UseSqlite(connectionString));

// Configuração do versionamento de API
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
});

// Configuração para explorar versões no Swagger
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Configuração do Swagger com inclusão de comentários XML (opcional)
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
    //Adicionar o campo "Authorization" no Swagger
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = JwtBearerDefaults.AuthenticationScheme,     // "Bearer"
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Insira apenas o token JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    { jwtSecurityScheme, Array.Empty<string>() }
});
});
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// Configuração do JWT

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Registro de serviços
builder.Services.AddControllers();
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<IPessoaFisicaServiceV1, PessoaFisicaServiceV1>();
builder.Services.AddScoped<IPessoaFisicaServiceV2, PessoaFisicaServiceV2>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

public class ConfigureSwaggerOptions : Microsoft.Extensions.Options.IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        var uniqueDescriptions = _provider.ApiVersionDescriptions
            .GroupBy(desc => desc.GroupName)
            .Select(group => group.First());

        foreach (var desc in uniqueDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, new OpenApiInfo
            {
                Title = $"Gerenciador de Pessoas {desc.GroupName.ToUpperInvariant()}",
                Version = desc.GroupName
            });
        }
    }
}
