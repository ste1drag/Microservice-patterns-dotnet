using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;

namespace Payment.Infrastructure
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.Currency).IsRequired().HasConversion<int>();
                entity.Property(e => e.Status).IsRequired().HasConversion<int>();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
            });

            modelBuilder.Entity<Refund>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TransactionId).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasOne<Transaction>()
                      .WithMany()
                      .HasForeignKey(e => e.TransactionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OutboxMessage>()
                .HasKey(om => om.Id);

            modelBuilder.Entity<OutboxMessage>()
                .Property(om => om.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<OutboxMessage>()
                .Property(om => om.ProcessedAt);

            modelBuilder.Entity<OutboxMessage>()
                .Property(om => om.Type)
                .IsRequired();

            modelBuilder.Entity<OutboxMessage>()
                .Property(om => om.Payload)
                .IsRequired();

            modelBuilder.Entity<OutboxMessage>()
                .Property(om => om.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<OutboxMessage>()
                .Property(om => om.Error);
        }
    }
}
