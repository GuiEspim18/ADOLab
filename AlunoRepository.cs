using System.Data;
using Microsoft.Data.SqlClient;
public class AlunoRepository : IRepository<Aluno>
{
    public string ConnectionString { get; set; }
    public AlunoRepository(string connectionString)
    {
        ConnectionString = connectionString;
    }
    public void GarantirEsquema()
    {
        const string ddl = @"
       IF OBJECT_ID('dbo.Alunos', 'U') IS NULL
       BEGIN
           CREATE TABLE dbo.Alunos (
               Id INT IDENTITY(1,1) PRIMARY KEY,
               Nome NVARCHAR(100) NOT NULL,
               Idade INT NOT NULL,
               Email NVARCHAR(100) NOT NULL,
               DataNascimento DATE NOT NULL
           );
       END";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(ddl, conn) { CommandType = CommandType.Text };
        cmd.ExecuteNonQuery();
    }
    public int Inserir(string nome, int idade, string email, DateTime dataNascimento)
    {
        const string sql = @"
       INSERT INTO dbo.Alunos (Nome, Idade, Email, DataNascimento)
       OUTPUT INSERTED.Id
       VALUES (@Nome, @Idade, @Email, @DataNascimento)";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@Idade", idade);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@DataNascimento", dataNascimento);
        return (int)cmd.ExecuteScalar();
    }
    public List<Aluno> Listar()
    {
        var alunos = new List<Aluno>();
        const string sql = "SELECT Id, Nome, Idade, Email, DataNascimento FROM dbo.Alunos";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            alunos.Add(new Aluno(
                (int)reader["Id"],
                (string)reader["Nome"],
                (int)reader["Idade"],
                (string)reader["Email"],
                (DateTime)reader["DataNascimento"]
            ));
        }
        return alunos;
    }
    public int Atualizar(int id, string nome, int idade, string email, DateTime dataNascimento)
    {
        const string sql = @"
       UPDATE dbo.Alunos
       SET Nome = @Nome,
           Idade = @Idade,
           Email = @Email,
           DataNascimento = @DataNascimento
       WHERE Id = @Id";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@Idade", idade);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@DataNascimento", dataNascimento);
        return cmd.ExecuteNonQuery(); // número de linhas afetadas
    }
    public int Excluir(int id)
    {
        const string sql = "DELETE FROM dbo.Alunos WHERE Id = @Id";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        return cmd.ExecuteNonQuery();
    }
    public List<Aluno> Buscar(string propriedade, object valor)
    {
        var alunos = new List<Aluno>();
        string sql = $"SELECT Id, Nome, Idade, Email, DataNascimento FROM dbo.Alunos WHERE {propriedade} = @Valor";
        using var conn = new SqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Valor", valor);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            alunos.Add(new Aluno(
                (int)reader["Id"],
                (string)reader["Nome"],
                (int)reader["Idade"],
                (string)reader["Email"],
                (DateTime)reader["DataNascimento"]
            ));
        }
        return alunos;
    }
}