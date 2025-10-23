using MySql.Data.MySqlClient;
using System.Data;

public class MatriculaRepository : IRepository<Matricula>
{
    public string ConnectionString { get; set; }

    public MatriculaRepository(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public void GarantirEsquema()
    {
        const string ddl = @"
        CREATE TABLE IF NOT EXISTS Matriculas (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            AlunoId INT NOT NULL,
            DisciplinaId INT NOT NULL,
            DataMatricula DATETIME NOT NULL,
            FOREIGN KEY (AlunoId) REFERENCES Alunos(Id),
            FOREIGN KEY (DisciplinaId) REFERENCES Disciplinas(Id)
        );";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(ddl, conn);
        cmd.ExecuteNonQuery();
    }

    public int Inserir(int alunoId, int disciplinaId, DateTime dataMatricula)
    {
        const string sql = @"INSERT INTO Matriculas (AlunoId, DisciplinaId, DataMatricula) 
                             VALUES (@AlunoId, @DisciplinaId, @DataMatricula);
                             SELECT LAST_INSERT_ID();";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@AlunoId", alunoId);
        cmd.Parameters.AddWithValue("@DisciplinaId", disciplinaId);
        cmd.Parameters.AddWithValue("@DataMatricula", dataMatricula);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public List<Matricula> Listar()
    {
        var lista = new List<Matricula>();
        const string sql = "SELECT Id, AlunoId, DisciplinaId, DataMatricula FROM Matriculas";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Matricula
            {
                Id = Convert.ToInt32(reader["Id"]),
                AlunoId = Convert.ToInt32(reader["AlunoId"]),
                DisciplinaId = Convert.ToInt32(reader["DisciplinaId"]),
                DataMatricula = Convert.ToDateTime(reader["DataMatricula"])
            });
        }

        return lista;
    }

    public int Atualizar(int id, int alunoId, int disciplinaId, DateTime dataMatricula)
    {
        const string sql = "UPDATE Matriculas SET AlunoId=@AlunoId, DisciplinaId=@DisciplinaId, DataMatricula=@DataMatricula WHERE Id=@Id";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@AlunoId", alunoId);
        cmd.Parameters.AddWithValue("@DisciplinaId", disciplinaId);
        cmd.Parameters.AddWithValue("@DataMatricula", dataMatricula);

        return cmd.ExecuteNonQuery();
    }

    public int Excluir(int id)
    {
        const string sql = "DELETE FROM Matriculas WHERE Id=@Id";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        return cmd.ExecuteNonQuery();
    }

    public List<Matricula> Buscar(string propriedade, object valor)
    {
        var lista = new List<Matricula>();
        var sql = $"SELECT Id, AlunoId, DisciplinaId, DataMatricula FROM Matriculas WHERE {propriedade} = @Valor";

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Valor", valor);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Matricula
            {
                Id = Convert.ToInt32(reader["Id"]),
                AlunoId = Convert.ToInt32(reader["AlunoId"]),
                DisciplinaId = Convert.ToInt32(reader["DisciplinaId"]),
                DataMatricula = Convert.ToDateTime(reader["DataMatricula"])
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
