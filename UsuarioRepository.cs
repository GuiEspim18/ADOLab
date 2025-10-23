using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

public class UsuarioRepository
{
    public string ConnectionString { get; }

    public UsuarioRepository(string conn)
    {
        ConnectionString = conn;
    }

    public void GarantirEsquema()
    {
        const string ddl = @"
        CREATE TABLE IF NOT EXISTS Usuarios (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            Nome VARCHAR(100) NOT NULL,
            Email VARCHAR(100) UNIQUE NOT NULL,
            SenhaHash VARCHAR(256) NOT NULL
        );";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(ddl, conn);
        cmd.ExecuteNonQuery();
    }

    public void Registrar(string nome, string email, string senha)
    {
        var hash = HashSenha(senha);
        const string sql = @"INSERT INTO Usuarios (Nome, Email, SenhaHash) VALUES (@Nome, @Email, @SenhaHash);";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@SenhaHash", hash);
        cmd.ExecuteNonQuery();
    }

    public Usuario? Autenticar(string email, string senha)
    {
        const string sql = "SELECT Id, Nome, Email, SenhaHash FROM Usuarios WHERE Email = @Email";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Email", email);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        var hash = reader["SenhaHash"].ToString()!;
        if (!VerificarSenha(senha, hash)) return null;

        return new Usuario(
            Convert.ToInt32(reader["Id"]),
            reader["Nome"].ToString()!,
            reader["Email"].ToString()!,
            hash
        );
    }

    private static string HashSenha(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerificarSenha(string senha, string hash)
    {
        return HashSenha(senha) == hash;
    }
}
