using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaoAPI.Models;

[Table("servicos")]
public class Servico
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do serviço é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome do serviço não pode exceder 100 caracteres")]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("descricao")]
    public string? Descricao { get; set; }


    [Column("ativo")]
    public bool Ativo { get; set; } = true;

    [Column("data_cadastro")]
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    public ICollection<ProfissionalServico> ProfissionalServicos { get; set; } = new List<ProfissionalServico>();
}
