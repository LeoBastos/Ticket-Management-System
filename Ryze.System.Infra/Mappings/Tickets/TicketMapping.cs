using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ryze.System.Domain.Entity.Tickets;

namespace Ryze.System.Infra.Mappings.Tickets
{
    public class TicketMapping : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Description).IsRequired();

            builder.Property(p => p.ClientImage)
                .HasMaxLength(250);

            builder.Property(p => p.OpeningDate).IsRequired();

            builder.Property(p => p.Resolution);

            builder.Property(p => p.UserImage)
               .HasMaxLength(250);

            builder.Property(p => p.Status)/*.HasConversion<string>()*/;

            builder.Property(p => p.Nivel);

            builder.Property(p => p.Priority);


            builder.Property(p => p.OpeningDate);

            builder.Property(p => p.IsActive);

            builder.HasOne(t => t.Client)
                   .WithMany()
                   .HasForeignKey(t => t.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.User)
                   .WithMany()
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
