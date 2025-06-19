using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain_Layer.Models; // Add this using statement

namespace Infrastructure_Layer.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel> // Change IdentityUser to UserModel
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Register Domain Models as DbSets
        public DbSet<IdeaSession> IdeaSessions { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<StepTemplate> StepTemplates { get; set; }
        public DbSet<MvpPlan> MvpPlans { get; set; }
        public DbSet<TechRecommendation> TechRecommendations { get; set; }
        public DbSet<UserStory> UserStories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // IdeaSession relationships
            builder.Entity<IdeaSession>()
                .HasMany(isess => isess.Steps)
                .WithOne(s => s.IdeaSession)
                .HasForeignKey(s => s.IdeaSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdeaSession>()
                .HasMany(isess => isess.MvpPlans)
                .WithOne(mp => mp.IdeaSession)
                .HasForeignKey(mp => mp.IdeaSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdeaSession>()
                .HasMany(isess => isess.TechRecommendations)
                .WithOne(tr => tr.IdeaSession)
                .HasForeignKey(tr => tr.IdeaSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // StepTemplate - Step relationship
            builder.Entity<StepTemplate>()
                .HasMany(st => st.Steps)
                .WithOne(s => s.StepTemplate)
                .HasForeignKey(s => s.StepTemplateId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete if StepTemplate is shared

            // MvpPlan - UserStory relationship
            builder.Entity<MvpPlan>()
                .HasMany(mp => mp.UserStories)
                .WithOne(us => us.MvpPlan)
                .HasForeignKey(us => us.MvpPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
