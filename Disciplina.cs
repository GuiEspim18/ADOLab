public class Disciplina
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";

    public int ProfessorId { get; set; }
    public Professor Professor { get; set; } = null!;

    public List<Matricula> Matriculas { get; set; } = new();
}
