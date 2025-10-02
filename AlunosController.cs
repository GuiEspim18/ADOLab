[ApiController]
[Route("api/[controller]")]
public class AlunosController : ControllerBase
{
    private readonly AlunoRepository _repo;
    public AlunosController()
    {
        _repo = new AlunoRepository("sua_connection_string_aqui");
        _repo.GarantirEsquema();
    }
    [HttpGet]
    public IActionResult Get() => Ok(_repo.Listar());
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var aluno = _repo.Buscar("Id", id).FirstOrDefault();
        if (aluno == null) return NotFound();
        return Ok(aluno);
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
        if (linhas == 0) return NotFound();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var linhas = _repo.Excluir(id);
        if (linhas == 0) return NotFound();
        return NoContent();
    }
}