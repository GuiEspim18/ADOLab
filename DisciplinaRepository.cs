using MySql.Data.MySqlClient;
using System.Data;

public class DisciplinaRepository : IRepository<Disciplina>
{
    public string ConnectionString { get; set; }

    public DisciplinaRepository(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public void GarantirEsquema()
    {
        const string ddl = @"
        CREATE TABLE IF NOT EXISTS Disciplinas (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            Nome VARCHAR(100) NOT NULL,
            ProfessorId INT NOT NULL,
            FOREIGN KEY (ProfessorId) REFERENCES Professores(Id)
        );";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(ddl, conn);
        cmd.ExecuteNonQuery();
    }

    public int Inserir(string nome, int professorId)
    {
        const string sql = @"INSERT INTO Disciplinas (Nome, ProfessorId) VALUES (@Nome, @ProfessorId);
                             SELECT LAST_INSERT_ID();";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@ProfessorId", professorId);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Disciplina> Listar()
    {
        var lista = new List<Disciplina>();
        const string sql = "SELECT Id, Nome, ProfessorId FROM Disciplinas";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Disciplina
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString()!,
                ProfessorId = Convert.ToInt32(reader["ProfessorId"])
            });
        }

        return lista;
    }

    public int Atualizar(int id, string nome, int professorId)
    {
        const string sql = "UPDATE Disciplinas SET Nome=@Nome, ProfessorId=@ProfessorId WHERE Id=@Id";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Nome", nome);
        cmd.Parameters.AddWithValue("@ProfessorId", professorId);

        return cmd.ExecuteNonQuery();
    }

    public int Excluir(int id)
    {
        const string sql = "DELETE FROM Disciplinas WHERE Id=@Id";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        return cmd.ExecuteNonQuery();
    }

    public List<Disciplina> Buscar(string propriedade, object valor)
    {
        var lista = new List<Disciplina>();
        var sql = $"SELECT Id, Nome, ProfessorId FROM Disciplinas WHERE {propriedade} = @Valor";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Valor", valor);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Disciplina
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString()!,
                ProfessorId = Convert.ToInt32(reader["ProfessorId"])
            });
        }

        return lista;
    }

    public int Inserir(string nome, int idade, string email, DateTime dataNascimento)
    {
        throw new NotImplementedException();
    }

    public int Atualizar(int id, string nome, int idade, string email, DateTime dataNascimento)
    {
        throw new NotImplementedException();
    }
}
