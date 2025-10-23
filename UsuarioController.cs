using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Authorize]
[Route("api/v2/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly EscolaContext _context;

    public UsuarioController(EscolaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Set<Usuario>().ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _context.Set<Usuario>().FindAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Usuario usuario)
    {
        // Faz hash da senha antes de salvar
        usuario.SenhaHash = HashSenha(usuario.SenhaHash);

        _context.Set<Usuario>().Add(usuario);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
    {
        if (id != usuario.Id) return BadRequest();

        // Atualiza hash se a senha foi informada
        if (!string.IsNullOrWhiteSpace(usuario.SenhaHash))
            usuario.SenhaHash = HashSenha(usuario.SenhaHash);

        _context.Entry(usuario).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Set<Usuario>().FindAsync(id);
        if (user == null) return NotFound();

        _context.Set<Usuario>().Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Função para gerar hash da senha (igual ao UsuarioRepository)
    private static string HashSenha(string senha)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(senha);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
