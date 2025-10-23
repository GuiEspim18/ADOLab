using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class ProfessoresController : ControllerBase
{
    private readonly EscolaContext _context;

    public ProfessoresController(EscolaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Professores.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var prof = await _context.Professores
            .Include(p => p.Disciplinas)
            .FirstOrDefaultAsync(p => p.Id == id);

        return prof == null ? NotFound() : Ok(prof);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Professor professor)
    {
        _context.Professores.Add(professor);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = professor.Id }, professor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Professor professor)
    {
        if (id != professor.Id) return BadRequest();

        _context.Entry(professor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var professor = await _context.Professores.FindAsync(id);
        if (professor == null) return NotFound();

        _context.Professores.Remove(professor);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
