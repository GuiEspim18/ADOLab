using MySql.Data.MySqlClient;
using System.Data;

public class ProfessorRepository : IRepository<Professor>
{
    public string ConnectionString { get; set; }

    public ProfessorRepository(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public void GarantirEsquema()
    {
        const string ddl = @"
        CREATE TABLE IF NOT EXISTS Professores (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            Nome VARCHAR(100) NOT NULL,
            Email VARCHAR(100) UNIQUE NOT NULL
        );";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(ddl, conn);
        cmd.ExecuteNonQuery();
    }

    public int Inserir(string nome, int idade, string email, DateTime dataNascimento)
    {
        // Ignorando idade e dataNascimento, que não existem no modelo de Professor
        const string sql = @"INSERT INTO Professores (Nome, Email) VALUES (@Nome, @Email);
                             SELECT LAST_INSERT_ID();";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@Email", email);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Professor> Listar()
    {
        var lista = new List<Professor>();
        const string sql = "SELECT Id, Nome, Email FROM Professores";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Professor
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString()!,
                Email = reader["Email"].ToString()!
            });
        }

        return lista;
    }

    public int Atualizar(int id, string nome, int idade, string email, DateTime dataNascimento)
    {
        // Ignorando idade e dataNascimento
        const string sql = "UPDATE Professores SET Nome=@Nome, Email=@Email WHERE Id=@Id";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@Email", email);

        return cmd.ExecuteNonQuery();
    }

    public int Excluir(int id)
    {
        const string sql = "DELETE FROM Professores WHERE Id=@Id";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        return cmd.ExecuteNonQuery();
    }

    public List<Professor> Buscar(string propriedade, object valor)
    {
        var lista = new List<Professor>();
        var sql = $"SELECT Id, Nome, Email FROM Professores WHERE {propriedade} = @Valor";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Valor", valor);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Professor
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString()!,
                Email = reader["Email"].ToString()!
            });
        }

        return lista;
    }
}
