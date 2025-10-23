using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly UsuarioRepository _userRepo;
    private readonly JwtService _jwt;

    public AuthController(IConfiguration config)
    {
        var connectionString = config.GetConnectionString("SqlServerConnection")!;
        _userRepo = new UsuarioRepository(connectionString);
        _userRepo.GarantirEsquema();
        _jwt = new JwtService(config);
    }

    /// <summary>
    /// Registra um novo usuário com hash de senha.
    /// </summary>
    [HttpPost("register")]
    public IActionResult Register([FromBody] Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.Nome) ||
            string.IsNullOrWhiteSpace(usuario.Email) ||
            string.IsNullOrWhiteSpace(usuario.SenhaHash))
        {
            return BadRequest(new { message = "Todos os campos são obrigatórios." });
        }

        // Registra aplicando hash internamente
        _userRepo.Registrar(usuario.Nome, usuario.Email, usuario.SenhaHash);

        return Ok(new { message = "Usuário registrado com sucesso." });
    }

    /// <summary>
    /// Autentica o usuário e retorna um token JWT.
    /// </summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] Usuario credenciais)
    {
        if (string.IsNullOrWhiteSpace(credenciais.Email) ||
            string.IsNullOrWhiteSpace(credenciais.SenhaHash))
        {
            return BadRequest(new { message = "Email e senha são obrigatórios." });
        }

        var user = _userRepo.Autenticar(credenciais.Email, credenciais.SenhaHash);
        if (user == null)
            return StatusCode(401, new { message = "Credenciais inválidas." });

        var token = _jwt.GerarToken(user);
        return Ok(new { token });
    }
}
