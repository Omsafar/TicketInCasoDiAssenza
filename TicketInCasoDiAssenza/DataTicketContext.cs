using Microsoft.EntityFrameworkCore;
using TicketingApp.Models;

namespace TicketingApp.Data
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options) { }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);

            // Fai s� che l'entit� Ticket venga letta da dbo.Ticket (singolare)
            modelBuilder.Entity<Ticket>()
                .ToTable("Ticket");           // <� nome tabella esatto nel DB

            // (Gi� presente) imposta l'identity seed a 82
            modelBuilder.Entity<Ticket>()
                .Property(t => t.TicketId)
                .UseIdentityColumn(seed: 82, increment: 1);

            modelBuilder.Entity<Ticket>()
            .Property(t => t.GraphMessageId)
            .HasMaxLength(200)
            .IsRequired();
             modelBuilder.Entity<Ticket>()
            .Property(t => t.ConversationId)
            .HasMaxLength(200)
            .IsRequired();

            modelBuilder.Entity<Ticket>()
                .HasIndex(t => t.GraphMessageId)
                .IsUnique();
            modelBuilder.Entity<Ticket>()
                .HasIndex(t => t.ConversationId)
                .IsUnique();

        }

    }
}

