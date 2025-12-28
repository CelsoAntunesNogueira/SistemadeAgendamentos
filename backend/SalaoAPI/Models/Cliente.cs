using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaoAPI.Models;

[Table("clientes")]
public class Cliente
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório")]
    [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres")]
    [Column("telefone")]
    public string Telefone { get; set; } = string.Empty;



    [Column("data_cadastro")]
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    [Column("ativo")]
    public bool Ativo { get; set; } = true;

    // Relacionamento: Um cliente pode ter vários agendamentos
    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}