using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaoAPI.Models;

[Table("profissional_servico")]
public class ProfissionalServico
{
	[Key]
	[Column("id")]
	public int Id { get; set; }

	[Required]
	[Column("profissional_id")]
	public int ProfissionalId { get; set; }

	[Required]
	[Column("servico_id")]
	public int ServicoId { get; set; }

	// Navegação
	[ForeignKey("ProfissionalId")]
	public Profissional Profissional { get; set; } = null!;

	[ForeignKey("ServicoId")]
	public Servico Servico { get; set; } = null!;
}