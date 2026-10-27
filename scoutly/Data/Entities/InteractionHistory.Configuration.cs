using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class InteractionHistoryConfiguration : IEntityTypeConfiguration<InteractionHistory>
    {
        public void Configure(EntityTypeBuilder<InteractionHistory> builder)
        {
            builder.HasKey(e => e.Id).HasName("interaction_history_pkey");
            builder.ToTable("interaction_history");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.RecruiterCandidateId).HasColumnName("recruiter_candidate_id");
            builder.Property(e => e.AuthorUserId).HasColumnName("author_user_id");
            builder.Property(e => e.ActionType).HasMaxLength(50).HasColumnName("action_type");
            builder.Property(e => e.Notes).HasColumnName("notes");
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");

            builder.HasOne(d => d.AuthorUser).WithMany(p => p.InteractionHistories)
                .HasForeignKey(d => d.AuthorUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("interaction_history_author_user_id_fkey");

            builder.HasOne(d => d.RecruiterCandidate).WithMany(p => p.InteractionHistories)
                .HasForeignKey(d => d.RecruiterCandidateId)
                .HasConstraintName("interaction_history_recruiter_candidate_id_fkey");
        }
    }
}