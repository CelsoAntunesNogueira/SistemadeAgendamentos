using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalaoAPI.Data;

namespace SalaoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly SalaoContext _context;

    public HealthController(SalaoContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Verifica se a API está funcionando
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "API está funcionando! ",
            timestamp = DateTime.Now,
            version = "1.0.0"
        });
    }

    /// <summary>
    /// Testa a conexão com o banco de dados
    /// </summary>
    [HttpGet("database")]
    public async Task<IActionResult> TestDatabase()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();

            if (canConnect)
            {
                var clientesCount = await _context.Clientes.CountAsync();
                var profissionaisCount = await _context.Profissionais.CountAsync();
                var servicosCount = await _context.Servicos.CountAsync();

                return Ok(new
                {
                    status = "Conexão com banco de dados OK! ",
                    database = "salao_flordelis",
                    estatisticas = new
                    {
                        clientes = clientesCount,
                        profissionais = profissionaisCount,
                        servicos = servicosCount
                    }
                });
            }

            return StatusCode(500, new
            {
                status = "Erro ao conectar com o banco de dados ",
                message = "Verifique as configurações de conexão"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "Erro ao conectar com o banco de dados ",
                error = ex.Message
            });
        }
    }
}
