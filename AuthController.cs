using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v2/auth")]
public class AuthController : ControllerBase
{
    private readonly UsuarioRepository _userRepo;
    private readonly JwtService _jwt;

    public AuthController(IConfiguration config)
    {
        _userRepo = new UsuarioRepository(config.GetConnectionString("SqlServerConnection")!);
        _userRepo.GarantirEsquema();
        _jwt = new JwtService(config);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] Usuario usuario)
    {
        _userRepo.Registrar(usuario.Nome, usuario.Email, usuario.SenhaHash);
        return Ok(new { message = "Usuário registrado com sucesso." });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] Usuario credenciais)
    {
        var user = _userRepo.Autenticar(credenciais.Email, credenciais.SenhaHash);
        if (user == null)
            return Unauthorized(new { message = "Credenciais inválidas." });

        var token = _jwt.GerarToken(user);
        return Ok(new { token });
    }
}
