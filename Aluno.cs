public class Aluno
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public int Idade { get; set; }
    public string Email { get; set; } = "";
    public DateTime DataNascimento { get; set; }

    public List<Matricula> Matriculas { get; set; } = new();
}
