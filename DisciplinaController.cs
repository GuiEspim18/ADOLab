using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class DisciplinasController : ControllerBase
{
    private readonly EscolaContext _context;

    public DisciplinasController(EscolaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Disciplinas.Include(d => d.Professor).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var disciplina = await _context.Disciplinas
            .Include(d => d.Professor)
            .Include(d => d.Matriculas)
            .ThenInclude(m => m.Aluno)
            .FirstOrDefaultAsync(d => d.Id == id);

        return disciplina == null ? NotFound() : Ok(disciplina);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Disciplina disciplina)
    {
        _context.Disciplinas.Add(disciplina);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = disciplina.Id }, disciplina);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Disciplina disciplina)
    {
        if (id != disciplina.Id) return BadRequest();

        _context.Entry(disciplina).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var disciplina = await _context.Disciplinas.FindAsync(id);
        if (disciplina == null) return NotFound();

        _context.Disciplinas.Remove(disciplina);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
