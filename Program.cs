using System.Text;
using MEUERP.API2.Config;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Necessário para Encoding.GetEncoding("ISO-8859-1")
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Configuração fortemente tipada + validação no startup
builder.Services
    .AddOptions<IntegracaoConfig>()
    .Bind(builder.Configuration.GetSection("Integracao"))
    .Validate(config =>
        !string.IsNullOrWhiteSpace(config.DbfPath) &&
        !string.IsNullOrWhiteSpace(config.TempPath),
        "A seção Integracao precisa definir DbfPath e TempPath.")
    .ValidateOnStart();

builder.Services.AddProblemDetails();

// Serviços
builder.Services.AddScoped<DbfReaderService>();

// Cliente 
builder.Services.AddScoped<ClienteIntegration>();
builder.Services.AddScoped<ClienteService>();

// Produto
builder.Services.AddScoped<ProdIntegration>();
builder.Services.AddScoped<ProdService>();

// Estoque
builder.Services.AddScoped<EstqIntegration>();
builder.Services.AddScoped<EstqService>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Tratamento global de exceções
app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var logger = context.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("GlobalExceptionHandler");

        if (exception is not null)
        {
            logger.LogError(exception, "Erro não tratado durante o processamento da requisição.");
        }

        var statusCode = exception switch
        {
            FileNotFoundException => StatusCodes.Status404NotFound,
            DirectoryNotFoundException => StatusCodes.Status404NotFound,
            ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode >= 500
                ? "Erro interno no servidor."
                : "Não foi possível processar a solicitação.",
            Detail = exception?.Message,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();