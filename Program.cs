using System.Text;
using MEUERP.API2.Config;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services
    .AddOptions<IntegracaoConfig>()
    .Bind(builder.Configuration.GetSection("Integracao"))
    .Validate(config =>
        !string.IsNullOrWhiteSpace(config.DbfPath) &&
        !string.IsNullOrWhiteSpace(config.TempPath),
        "A seção Integracao precisa definir DbfPath e TempPath.")
    .Validate(config => config.SyncBatchSize > 0 && config.SyncBatchSize <= 5000,
        "Integracao:SyncBatchSize deve estar entre 1 e 5000.")
    .ValidateOnStart();

builder.Services.AddProblemDetails();
builder.Services.AddScoped<DbfReaderService>();
builder.Services.AddScoped<PostgresUpsertService>();

builder.Services.AddScoped<ClienteIntegration>();
builder.Services.AddScoped<ClienteService>();

builder.Services.AddScoped<ProdIntegration>();
builder.Services.AddScoped<ProdService>();

builder.Services.AddScoped<EstqIntegration>();
builder.Services.AddScoped<EstqService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var logger = context.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("GlobalExceptionHandler");

        if (exception is not null)
            logger.LogError(exception, "Erro não tratado durante o processamento da requisição.");

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
            Title = statusCode >= 500 ? "Erro interno no servidor." : "Não foi possível processar a solicitação.",
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
