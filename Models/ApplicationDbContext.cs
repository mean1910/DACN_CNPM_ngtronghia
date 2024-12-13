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
        
        public DbSet<VocabQuestion> Questions { get; set; }
        public DbSet<VocabOption> Options { get; set; }
        public DbSet<Grammar> Grammars { get; set; }
        public DbSet<GrammarQuestion> GrammarQuestions { get; set; }
        public DbSet<GrammarOption> GrammarOptions { get; set; }
        public DbSet<ReadingSkill> ReadingSkills { get; set; }
        public DbSet<ReadingQuestion> ReadingQuestions { get; set; }
        public DbSet<ReadingOption> ReadingOptions { get; set; }
        public DbSet<Listening> Listenings { get; set; }
        public DbSet<ListeningQuestion> ListeningsQuestions { get; set; }
        public DbSet<ListeningOption> ListeningsOptions { get; set; }
        public DbSet<Writing> Writings { get; set; }
        public DbSet<UserWritingSubmission> Submissions { get; set; }
        public DbSet<Speaking> Speakings { get; set; }

    }
}
