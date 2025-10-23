using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class MatriculasController : ControllerBase
{
    private readonly EscolaContext _context;

    public MatriculasController(EscolaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Matriculas
        .Include(m => m.Aluno)
        .Include(m => m.Disciplina)
        .ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var matricula = await _context.Matriculas
            .Include(m => m.Aluno)
            .Include(m => m.Disciplina)
            .FirstOrDefaultAsync(m => m.Id == id);

        return matricula == null ? NotFound() : Ok(matricula);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Matricula matricula)
    {
        matricula.DataMatricula = DateTime.UtcNow;
        _context.Matriculas.Add(matricula);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = matricula.Id }, matricula);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var matricula = await _context.Matriculas.FindAsync(id);
        if (matricula == null) return NotFound();

        _context.Matriculas.Remove(matricula);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
