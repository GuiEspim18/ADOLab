using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class AlunosController : ControllerBase
{
    private readonly EscolaContext _context;

    public AlunosController(EscolaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Alunos.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var aluno = await _context.Alunos
            .Include(a => a.Matriculas)
            .ThenInclude(m => m.Disciplina)
            .FirstOrDefaultAsync(a => a.Id == id);

        return aluno == null ? NotFound() : Ok(aluno);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Aluno aluno)
    {
        _context.Alunos.Add(aluno);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = aluno.Id }, aluno);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Aluno aluno)
    {
        if (id != aluno.Id) return BadRequest();

        _context.Entry(aluno).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var aluno = await _context.Alunos.FindAsync(id);
        if (aluno == null) return NotFound();

        _context.Alunos.Remove(aluno);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
