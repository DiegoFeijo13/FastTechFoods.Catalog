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
    public async Task<IActionResult> Post([FromBody] ProdutoInputDTO dto)
    {
        var sucesso = await _mediator.Send(new CadastrarProdutoCommand(dto));
        return sucesso ? Ok("Produto cadastrado com sucesso") : BadRequest("Erro ao cadastrar produto");
    }
}
