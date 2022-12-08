using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "Título obrigatório")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "Gênero Obrigatório")]
    [MaxLength(50, ErrorMessage = "Não pode exceder 50 caracteres")]
    public string Genero { get; set; }
    [Required]
    [Range(70, 400, ErrorMessage = "A duração deve ter entre 70 e 400 minutos")]
    public int Duracao { get; set;}

}
