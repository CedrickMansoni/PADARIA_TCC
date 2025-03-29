using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Padaria.Share.Hash_Password;
using Padaria.WebApi.Data;
using Padaria.WebApi.Repository.Funcionario;
using Padaria.WebApi.Repository.Produto;
using Padaria.WebApi.SalvarArquivos;
using Padaria.WebApi.Service.Funcionario;
using Padaria.WebApi.Service.Produto;
using Padaria.WebApi.SMS_Service.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddTransient<IFuncionarioService, FuncionarioService>();

builder.Services.AddTransient<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddTransient<ICategoriaService, CategoriaService>();

builder.Services.AddTransient<IProdutoRepository, ProdutoRepository>();
builder.Services.AddTransient<IProdutoService, ProdutoService>();

builder.Services.AddScoped<IArquivoService, ArquivoService>();

builder.Services.AddTransient<IHash_PWD, Hash_PWD>();
builder.Services.AddTransient<ISMS_enviar, SMS_enviar>();

string conexao = string.Empty;
#if DEBUG
conexao = builder.Configuration.GetConnectionString("LocalConnection")!;
#else 
    conexao = builder.Configuration.GetConnectionString("ProdutionConnection")!;
#endif

builder.Services.AddDbContext<AppDataContext>(options => options.UseNpgsql(conexao));

// Adiciona CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite qualquer origem
              .AllowAnyHeader()  // Permite qualquer cabeçalho
              .AllowAnyMethod(); // Permite qualquer método HTTP
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "PADARIA_APP"));
}

string storagePath = app.Environment.IsDevelopment() ?
"/home/ckm/Storage/Padaria" :
"/home/GSA_PROJECT/Storage/Padaria";

// Configurar middleware para servir arquivos de um diretório externo
app.UseStaticFiles(new StaticFileOptions
{
    //--- 
    FileProvider = new PhysicalFileProvider(storagePath),
    RequestPath = "/images", // Caminho acessível via URL
    ServeUnknownFileTypes = false, // Não servir tipos desconhecidos
    DefaultContentType = "image/jpeg", // Conteúdo padrão caso a extensão não seja reconhecida
    OnPrepareResponse = ctx =>
    {
        // Bloquear acesso a arquivos sem extensões de imagem
        if (!ctx.File.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".doc", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
    }
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
