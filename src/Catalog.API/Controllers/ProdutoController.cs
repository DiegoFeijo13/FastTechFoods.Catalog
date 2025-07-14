using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;
[Route("[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProdutoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] ProdutoInputDTO dto, [FromServices] IValidator<ProdutoInputDTO> validator)
    {
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => new { e.ErrorMessage }));

        var sucesso = await _mediator.Send(new CadastrarProdutoCommand(dto));
        return sucesso ? Ok("Produto cadastrado com sucesso") : BadRequest("Erro ao cadastrar produto");
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] ProdutoInputDTO dto, [FromServices] IValidator<ProdutoInputDTO> validator)
    {
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => new { e.ErrorMessage }));

        var sucesso = await _mediator.Send(new AtualizarProdutoCommand(id, dto));
        return sucesso ? Ok("Produto atualizado com sucesso") : NotFound("Produto não encontrado");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var sucesso = await _mediator.Send(new RemoverProdutoCommand(id));
        return sucesso ? Ok("Produto removido com sucesso") : NotFound("Produto não encontrado");
    }

    [HttpPatch("{id:guid}/disponibilidade")]
    public async Task<IActionResult> AlterarDisponibilidade(Guid id, [FromQuery] bool disponivel)
    {
        var sucesso = await _mediator.Send(new AlterarDisponibilidadeCommand(id, disponivel));
        return sucesso ? Ok("Disponibilidade alterada") : NotFound("Produto não encontrado");
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Produto>> GetById(Guid id)
    {
        var produto = await _mediator.Send(new ObterProdutoPorIdQuery(id));
        return produto is not null ? Ok(produto) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> GetAll()
    {
        var produtos = await _mediator.Send(new ObterTodosProdutosQuery());
        return Ok(produtos);
    }

    [HttpGet("busca")]
    public async Task<ActionResult<IEnumerable<Produto>>> Buscar([FromQuery] string termo)
    {
        var produtos = await _mediator.Send(new BuscarProdutoPorTermoQuery(termo));
        return Ok(produtos);
    }

    [HttpGet("categoria/{nome}")]
    public async Task<ActionResult<IEnumerable<Produto>>> GetByCategoria(string nome)
    {
        var produtos = await _mediator.Send(new ObterProdutosPorCategoriaQuery(nome));
        return Ok(produtos);
    }
}
