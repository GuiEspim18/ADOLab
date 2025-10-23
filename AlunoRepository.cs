using MySql.Data.MySqlClient;
using System.Data;

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
        CREATE TABLE IF NOT EXISTS Alunos (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            Nome VARCHAR(100) NOT NULL,
            Idade INT NOT NULL,
            Email VARCHAR(100) UNIQUE NOT NULL,
            DataNascimento DATETIME NOT NULL
        );";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(ddl, conn);
        cmd.ExecuteNonQuery();
    }

    public int Inserir(string nome, int idade, string email, DateTime dataNascimento)
    {
        const string sql = @"INSERT INTO Alunos (Nome, Idade, Email, DataNascimento) 
                             VALUES (@Nome, @Idade, @Email, @DataNascimento);
                             SELECT LAST_INSERT_ID();";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@Idade", idade);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@DataNascimento", dataNascimento);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Aluno> Listar()
    {
        var lista = new List<Aluno>();
        const string sql = "SELECT Id, Nome, Idade, Email, DataNascimento FROM Alunos";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Aluno
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString()!,
                Idade = Convert.ToInt32(reader["Idade"]),
                Email = reader["Email"].ToString()!,
                DataNascimento = Convert.ToDateTime(reader["DataNascimento"])
            });
        }

        return lista;
    }

    public int Atualizar(int id, string nome, int idade, string email, DateTime dataNascimento)
    {
        const string sql = "UPDATE Alunos SET Nome=@Nome, Idade=@Idade, Email=@Email, DataNascimento=@DataNascimento WHERE Id=@Id";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@Idade", idade);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@DataNascimento", dataNascimento);

        return cmd.ExecuteNonQuery();
    }

    public int Excluir(int id)
    {
        const string sql = "DELETE FROM Alunos WHERE Id=@Id";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        return cmd.ExecuteNonQuery();
    }

    public List<Aluno> Buscar(string propriedade, object valor)
    {
        var lista = new List<Aluno>();
        var sql = $"SELECT Id, Nome, Idade, Email, DataNascimento FROM Alunos WHERE {propriedade} = @Valor";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Valor", valor);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Aluno
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString()!,
                Idade = Convert.ToInt32(reader["Idade"]),
                Email = reader["Email"].ToString()!,
                DataNascimento = Convert.ToDateTime(reader["DataNascimento"])
            });
        }

        return lista;
    }
}
