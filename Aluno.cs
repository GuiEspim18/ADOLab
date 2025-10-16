/// <summary>
/// Representa um usu�rio do sistema (para login/autentica��o).
/// </summary>
public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string SenhaHash { get; set; } = "";

    public Usuario() { }

    public Usuario(int id, string nome, string email, string senhaHash)
    {
        Id = id;
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
    }
}
