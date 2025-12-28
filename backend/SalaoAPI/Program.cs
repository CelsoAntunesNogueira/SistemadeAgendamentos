using Microsoft.EntityFrameworkCore; 
using SalaoAPI.Data;

var builder = WebApplication.CreateBuilder(args);   // Cria o construtor do aplicativo

// Obtém a string de conexão do banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); 
builder.Services.AddDbContext<SalaoContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); 
// Configura o contexto do banco de dados com MySQL


builder.Services.AddControllers();   // Adiciona suporte a controladores

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer(); // Adiciona o explorador de endpoints para Swagger
builder.Services.AddSwaggerGen(c => // Configura o Swagger
{
    c.SwaggerDoc("v1", new() // Define a documentação Swagger
    {
        Title = "SalaoAPI",
        Version = "v1",
        Description = "API para gerenciamento de salão de beleza"
    });
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>(); // Obtém as origens permitidas do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>// Define a política CORS
    {
        policy.WithOrigins(allowedOrigins ?? new[] { "*" }) // Define as origens permitidas
              .AllowAnyHeader()          // Permite qualquer cabeçalho
              .AllowAnyMethod();         // Permite qualquer método HTTP
    });
});
var app = builder.Build();  // Constrói o aplicativo

//Pipeline HTTP
if (app.Environment.IsDevelopment()) // Verifica se o ambiente é de desenvolvimento
{
    app.UseSwagger();               // Habilita o Swagger
    app.UseSwaggerUI(c =>           // Configura a interface do Swagger
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalaoAPI v1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseHttpsRedirection(); // Redireciona para HTTPS
app.UseCors("AllowFrontend"); // Aplica a política CORS
app.UseAuthorization(); // Habilita a autorização
app.MapControllers(); // Mapeia os controladores

app.Run(); // Executa o aplicativo