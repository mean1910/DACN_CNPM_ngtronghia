using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace elearning_b1.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<VocabExercise> Exercises { get; set; }
        public DbSet<VocabQuestion> Questions { get; set; }
        public DbSet<VocabOption> Options { get; set; }
        public DbSet<GrammarLesson> GrammarLessons { get; set; }
    }
}
