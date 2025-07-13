using Catalog.Application.Commands;
using Catalog.Application.DTOs;
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
    public async Task<IActionResult> Cadastrar([FromBody] ProdutoInputDTO dto)
    {
        var sucesso = await _mediator.Send(new CadastrarProdutoCommand(dto));
        return sucesso ? Ok("Produto cadastrado com sucesso") : BadRequest("Erro ao cadastrar produto");
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Atualizar(long id, [FromBody] ProdutoInputDTO dto)
    {
        var sucesso = await _mediator.Send(new AtualizarProdutoCommand(id, dto));
        return sucesso ? Ok("Produto atualizado com sucesso") : NotFound("Produto não encontrado");
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var sucesso = await _mediator.Send(new RemoverProdutoCommand(id));
        return sucesso ? Ok("Produto removido com sucesso") : NotFound("Produto não encontrado");
    }

    [HttpPatch("{id:long}/disponibilidade")]
    public async Task<IActionResult> AlterarDisponibilidade(long id, [FromQuery] bool disponivel)
    {
        var sucesso = await _mediator.Send(new AlterarDisponibilidadeCommand(id, disponivel));
        return sucesso ? Ok("Disponibilidade alterada") : NotFound("Produto não encontrado");
    }
}
