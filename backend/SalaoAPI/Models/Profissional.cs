using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaoAPI.Models;

[Table("profissionais")]
public class Profissional
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres")]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("especialidades")]
    public string? Especialidades { get; set; }

    [StringLength(20, ErrorMessage = "O telefone não pode exceder 20 caracteres")]
    [Column("telefone")]
    public string? Telefone { get; set; }

    [Column("ativo")]
    public bool Ativo { get; set; } = true;

    [Column("data_cadastro")]
    public DateTime DataCadastro { get; set; } = DateTime.Now;
   


    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    public ICollection<ProfissionalServico> ProfissionalServicos { get; set; } = new List<ProfissionalServico>();
}