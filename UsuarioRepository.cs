using Microsoft.Data.SqlClient;
using System.Data;
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
        IF OBJECT_ID('dbo.Usuarios', 'U') IS NULL
        BEGIN
            CREATE TABLE dbo.Usuarios (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Nome NVARCHAR(100) NOT NULL,
                Email NVARCHAR(100) UNIQUE NOT NULL,
                SenhaHash NVARCHAR(256) NOT NULL
            );
        END";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(ddl, conn);
        cmd.ExecuteNonQuery();
    }

    public void Registrar(string nome, string email, string senha)
    {
        var hash = HashSenha(senha);
        const string sql = "INSERT INTO dbo.Usuarios (Nome, Email, SenhaHash) VALUES (@Nome, @Email, @SenhaHash)";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@SenhaHash", hash);
        cmd.ExecuteNonQuery();
    }

    public Usuario? Autenticar(string email, string senha)
    {
        const string sql = "SELECT Id, Nome, Email, SenhaHash FROM dbo.Usuarios WHERE Email = @Email";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Email", email);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        var hash = (string)reader["SenhaHash"];
        if (!VerificarSenha(senha, hash)) return null;

        return new Usuario((int)reader["Id"], (string)reader["Nome"], (string)reader["Email"], hash);
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
