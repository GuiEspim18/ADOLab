using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/v2/[controller]")]
public class AlunosController : ControllerBase
{
    private readonly AlunoRepository _repo;

    public AlunosController(IConfiguration config)
    {
        _repo = new AlunoRepository(config.GetConnectionString("SqlServerConnection")!);
        _repo.GarantirEsquema();
    }

    [HttpGet]
    public IActionResult Get() => Ok(_repo.Listar());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var aluno = _repo.Buscar("Id", id).FirstOrDefault();
        return aluno == null ? NotFound() : Ok(aluno);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Aluno aluno)
    {
        var novoId = _repo.Inserir(aluno.Nome, aluno.Idade, aluno.Email, aluno.DataNascimento);
        aluno.Id = novoId;
        return CreatedAtAction(nameof(GetById), new { id = novoId }, aluno);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Aluno aluno)
    {
        var linhas = _repo.Atualizar(id, aluno.Nome, aluno.Idade, aluno.Email, aluno.DataNascimento);
        return linhas == 0 ? NotFound() : NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var linhas = _repo.Excluir(id);
        return linhas == 0 ? NotFound() : NoContent();
    }
}
