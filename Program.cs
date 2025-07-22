using CadastroPessoasApi.Data;
using CadastroPessoasApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext com SQLite
builder.Services.AddDbContext<PessoaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// Registro de serviços
builder.Services.AddControllers();
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();

builder.Services.AddScoped<IPessoaFisicaServiceV1, PessoaFisicaServiceV1>();
builder.Services.AddScoped<IPessoaFisicaServiceV2, PessoaFisicaServiceV2>();
builder.Services.AddScoped<IPessoaJuridicaServiceV1, PessoaJuridicaServiceV1>();
builder.Services.AddScoped<IPessoaJuridicaServiceV2, PessoaJuridicaServiceV2>();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.UseHttpsRedirection();
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
