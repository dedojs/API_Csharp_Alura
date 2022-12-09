using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso a inserção seja feita com sucesso</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id}, filme);
    }

    /// <summary>
    /// Busca todos os filmes cadastrados no banco
    /// </summary>
    /// <param name="skip">Ignora a quantidade especificada de itens </param>
    /// <param name="take">Retorna a quantidade especificada de itens</param>
    /// <returns></returns>
    /// <response code="200">Retorna os filmes cadastrados no banco no banco</response>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperarFilmes([FromQuery] int skip = 0, [FromQuery] int take = 100)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Busca o filme especificado pelo ID
    /// </summary>
    /// <param name="id">Id do filme a ser buscado</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Retorna o filme especificado através do ID</response>
    /// <returns></returns>
    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }

    /// <summary>
    /// Atualiza todos os campos do filme que foi localizado através do ID informado
    /// </summary>
    /// <param name="id">Id do filme a ser atualizado</param>
    /// <param name="filmeDto">Objeto com os campos necessários para atualização de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Sem retorno</response>
    /// <returns></returns>
    [HttpPut("{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Atualiza campos especificados do filme que foi localizado através do ID informado
    /// Exemplo:
    /// "op": "replace",
    /// "path": "/titulo",
    /// "value": "novo nome"
    /// </summary>
    /// <param name="id">Id do filme a ser atualizado</param>
    /// <param name="patch">"Campo a ser modificado"</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Sem retorno</response>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public IActionResult AtualizarFilmeParcial(int id, [FromBody]  JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if(!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deleta o filme que for selecionado através do ID
    /// </summary>
    /// <param name="id">Id do filme a ser deletado</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Sem retorno</response>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
