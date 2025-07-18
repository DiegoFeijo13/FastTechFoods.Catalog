using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    /// Cadastra novo produto no catálogo
    /// </summary>
    /// <returns>Status da operação</returns>
    /// <response code="200">Cadastrado com sucesso</response>
    /// <response code="400">Falha no processo</response> 
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="403">Funcionário não autorizado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpPost]
    [Authorize(Roles = "gerente")]
    public async Task<IActionResult> Cadastrar([FromBody] ProdutoInputDTO dto, [FromServices] IValidator<ProdutoInputDTO> validator)
    {
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => new { e.ErrorMessage }));

        var sucesso = await _mediator.Send(new CadastrarProdutoCommand(dto));
        return sucesso ? Ok("Produto cadastrado com sucesso") : BadRequest("Erro ao cadastrar produto");
    }

    /// <summary>
    /// Atualiza informações de produto do catálogo
    /// </summary>
    /// <returns>Status da operação</returns>
    /// <response code="200">Alteração realizada com sucesso</response>
    /// <response code="400">Falha no processo</response> 
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="403">Funcionário não autorizado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "gerente")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] ProdutoInputDTO dto, [FromServices] IValidator<ProdutoInputDTO> validator)
    {
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => new { e.ErrorMessage }));

        var sucesso = await _mediator.Send(new AtualizarProdutoCommand(id, dto));
        return sucesso ? Ok("Produto atualizado com sucesso") : NotFound("Produto não encontrado");
    }

    /// <summary>
    /// Remove produto do catálogo
    /// </summary>
    /// <returns>Status da operação</returns>
    /// <response code="200">Removido com sucesso</response>
    /// <response code="400">Falha no processo</response> 
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="403">Funcionário não autorizado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "gerente")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var sucesso = await _mediator.Send(new RemoverProdutoCommand(id));
        return sucesso ? Ok("Produto removido com sucesso") : NotFound("Produto não encontrado");
    }

    /// <summary>
    /// Altera disponibilidade de um produto do catálogo
    /// </summary>
    /// <returns>Status da operação</returns>
    /// <response code="200">Alteração realizada com sucesso</response>
    /// <response code="400">Falha no processo</response> 
    /// <response code="404">Não encontrado</response> 
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="403">Funcionário não autorizado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpPatch("{id:guid}/disponibilidade")]
    [Authorize(Roles = "gerente")]
    public async Task<IActionResult> AlterarDisponibilidade(Guid id, [FromQuery] bool disponivel)
    {
        var sucesso = await _mediator.Send(new AlterarDisponibilidadeCommand(id, disponivel));
        return sucesso ? Ok("Disponibilidade alterada") : NotFound("Produto não encontrado");
    }

    /// <summary>
    /// Busca produto do catálogo por ID
    /// </summary>
    /// <returns>Produto</returns>
    /// <response code="200">Produto encontrado</response>
    /// <response code="400">Falha no processo</response> 
    /// <response code="404">Não encontrado</response> 
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="403">Funcionário não autorizado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "cliente,gerente,atendente")]
    public async Task<ActionResult<Produto>> GetById(Guid id)
    {
        var produto = await _mediator.Send(new ObterProdutoPorIdQuery(id));
        return produto is not null ? Ok(produto) : NotFound();
    }

    /// <summary>
    /// Busca todos produto do catálogo
    /// </summary>
    /// <returns>Lista de produtos</returns>
    /// <response code="200">Produtos cadastrados</response>
    /// <response code="400">Falha no processo</response> 
    /// <response code="404">Não encontrado</response> 
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="403">Funcionário não autorizado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpGet]
    [Authorize(Roles = "cliente,gerente,atendente")]
    public async Task<ActionResult<IEnumerable<Produto>>> GetAll()
    {
        var produtos = await _mediator.Send(new ObterTodosProdutosQuery());
        return Ok(produtos);
    }

    /// <summary>
    /// Busca produtos por filtro
    /// </summary>
    /// <returns>Lista de produtos</returns>
    /// <response code="200">Produtos cadastrados</response>
    /// <response code="400">Falha no processo</response> 
    /// <response code="404">Não encontrado</response> 
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="403">Funcionário não autorizado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpGet("busca")]
    [Authorize(Roles = "cliente,gerente,atendente")]
    public async Task<ActionResult<IEnumerable<Produto>>> Buscar([FromQuery] string termo)
    {
        var produtos = await _mediator.Send(new BuscarProdutoPorTermoQuery(termo));
        return Ok(produtos);
    }

    /// <summary>
    /// Busca produtos por categoria
    /// </summary>
    /// <returns>Lista de produtos</returns>
    /// <response code="200">Produtos cadastrados</response>
    /// <response code="400">Falha no processo</response> 
    /// <response code="404">Não encontrado</response> 
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="403">Funcionário não autorizado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpGet("categoria/{nome}")]
    [Authorize(Roles = "cliente,gerente,atendente")]
    public async Task<ActionResult<IEnumerable<Produto>>> GetByCategoria(string nome)
    {
        var produtos = await _mediator.Send(new ObterProdutosPorCategoriaQuery(nome));
        return Ok(produtos);
    }
}
