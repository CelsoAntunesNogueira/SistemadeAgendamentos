using System.ComponentModel.DataAnnotations;

namespace SalaoAPI.DTOs
{
    public class ClienteDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength =3, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "O telefone informado não é válido.")]
        [StringLength(20, ErrorMessage = "O telefone não pode exceder 20 caracteres.")]
        public string Telefone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        public string? Email { get; set; }
       
    }

    public class ClienteUpdateDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength =3, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Telefone inválido")]
        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres")]
        public string Telefone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        public string? Email { get; set; }

        public bool Ativo { get; set; } = true;
    }

    public class  ClienteResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
    }
}