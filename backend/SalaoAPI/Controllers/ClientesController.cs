using Microsoft.AspNetCore.Mvc; 
using SalaoAPI.Models;
using Microsoft.EntityFrameworkCore;
using SalaoAPI.Data;

namespace SalaoAPI.Controllers 
{
    [ApiController] // Anotação que indica que esta classe é um controlador de API
    [Route("api/[controller]")] // Define a rota base para este controlador

    public class ClientesController : ControllerBase
    {
        private readonly SalaoContext _context;   // Injeção de dependência do contexto do banco de dados

        public ClientesController(SalaoContext context)
        {
            _context = context;
        }
        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes() // Método para obter todos os clientes ativos
        {
            var clientes = await _context.Clientes
                .Where(c => c.Ativo)
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return Ok(clientes);
        }
        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado"});
            }
            return Ok(cliente);
        }

        [HttpGet("telefone/{telefone}")]
        public async Task<ActionResult<Cliente>> GetClientePorTelefone(string telefone)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Telefone == telefone);
            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado" });
            }
            return Ok(cliente);
        }


        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            var clienteExistente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Telefone == cliente.Telefone);

            if (clienteExistente != null)
            {
               return BadRequest(new { message = "Já existe um cliente com este telefone." });
            }

            cliente.DataCadastro = DateTime.Now;
            cliente.Ativo = true;

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCliente), new { id = cliente.Id }, cliente);
        }
        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest(new { message = "ID do cliente não corresponde"});
            }

            var clienteExistente = await _context.Clientes.FindAsync(id);
            if (clienteExistente == null)
            {
                return NotFound(new { message = "Cliente não encontrado"});
            }

            var telefoneEmUso = await _context.Clientes
                .AnyAsync(c => c.Telefone == cliente.Telefone && c.Id != id);
            if (telefoneEmUso)
            {
                return BadRequest(new { message = "Já existe um cliente com este telefone." });
            }

            clienteExistente.Nome = cliente.Nome;
            clienteExistente.Telefone = cliente.Telefone;
            clienteExistente.Ativo = cliente.Ativo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound( new {message = "Cliente não encontrado"});
                }
                    throw;
                
            }
            return Ok(new {message = "Cliente atualizado com sucesso", cliente = clienteExistente});
        }
        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado" });
            }

            var temAgendamentosFuturos = await _context.Agendamentos
                .AnyAsync(a => a.ClienteId == id &&
                                a.DataHora >= DateTime.Now &&
                                a.Status != "cancelado");

            if (temAgendamentosFuturos)
            {
                return BadRequest(new { message = "Não é possível excluir o cliente porque ele possui agendamentos futuros." });
            }

            cliente.Ativo = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cliente desativado com sucesso" });
        }
        [HttpDelete("{id}/permanente")]
        public async Task<IActionResult> DeleteClientePermanente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado" });
            }

            // Verifica se tem agendamentos
            var temAgendamentos = await _context.Agendamentos
                .AnyAsync(a => a.ClienteId == id);

            if (temAgendamentos)
            {
                return BadRequest(new
                {
                    message = "Não é possível excluir permanentemente um cliente com histórico de agendamentos"
                });
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cliente excluído permanentemente" });
        }

        /// <summary>
        /// Reativa um cliente desativado
        /// </summary>
        [HttpPatch("{id}/reativar")]
        public async Task<IActionResult> ReativarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado" });
            }

            cliente.Ativo = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cliente reativado com sucesso", cliente });
        }

        /// <summary>
        /// Busca clientes por nome (pesquisa parcial)
        /// </summary>
        [HttpGet("buscar/{termo}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> BuscarClientes(string termo)
        {
            var clientes = await _context.Clientes
                .Where(c => c.Ativo && c.Nome.Contains(termo))
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return Ok(clientes);
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}