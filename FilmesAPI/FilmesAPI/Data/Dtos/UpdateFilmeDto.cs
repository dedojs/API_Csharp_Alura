using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

public class UpdateFilmeDto
{
    [Required(ErrorMessage = "Título obrigatório")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "Gênero Obrigatório")]
    [StringLength(50, ErrorMessage = "Não pode exceder 50 caracteres")]
    public string Genero { get; set; }
    [Required]
    [Range(70, 400, ErrorMessage = "A duração deve ter entre 70 e 400 minutos")]
    public int Duracao { get; set; }
}
