using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaoAPI.Models
{
    [Table("agendamentos")]
    public class Agendamento
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O cliente é obrigatório")]
        [Column("cliente_id")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O serviço é obrigatório")]
        [Column("servico_id")]
        public int ServicoId { get; set; }

        [Required(ErrorMessage = "O profissional é obrigatório")]
        [Column("profissional_id")]
        public int ProfissionalId { get; set; }

        [Required(ErrorMessage = "A data do agendamento é obrigatória")]
        [Column("data_hora")]
        public DateTime DataHora { get; set; }

        [Required]
        [StringLength(20)]
        [Column("status")]
        public string Status { get; set; } = "pendente";

        [Column("observacoes")]
        public string? Observacoes { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; } = DateTime.Now;

        [Column("atualizado_em")]
        public DateTime AtualizadoEm { get; set; } = DateTime.Now;

        // Navegação
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; } = null!;

        [ForeignKey("ServicoId")]
        public Servico Servico { get; set; } = null!;

        [ForeignKey("ProfissionalId")]
        public Profissional Profissional { get; set; } = null!;
    }
}