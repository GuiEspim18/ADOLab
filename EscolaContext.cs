using Microsoft.EntityFrameworkCore;

public class EscolaContext : DbContext
{
    public EscolaContext(DbContextOptions<EscolaContext> options)
        : base(options) { }

    public DbSet<Aluno> Alunos { get; set; } = null!;
    public DbSet<Professor> Professores { get; set; } = null!;
    public DbSet<Disciplina> Disciplinas { get; set; } = null!;
    public DbSet<Matricula> Matriculas { get; set; } = null!;
}
