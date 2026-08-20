using Game.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.Infrastructure
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Team> Teams { get; set; }
        public DbSet<Domain.Entities.Stadium> Stadiums { get; set; }
        public DbSet<Domain.Entities.Game> Games { get; set; }
        public DbSet<Domain.Entities.StadiumSeat> StadiumSeats { get; set; }
        public DbSet<Domain.Entities.GameTicket> GameTickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            modelBuilder.Entity<StadiumSeat>()
                .HasOne(s => s.Stadium)
                .WithMany(st => st.Seats)
                .HasForeignKey(s => s.StadiumId);

            modelBuilder.Entity<Domain.Entities.Team>()
                .HasOne(t => t.HomeStadium)
                .WithOne(s => s.HomeTeam)
                .HasForeignKey<Domain.Entities.Team>(t => t.StadiumId);

            modelBuilder.Entity<Domain.Entities.Game>()
                                .HasOne(g => g.GamePlace)
                                .WithMany(s => s.Games )
                                .HasForeignKey(g => g.StadiumId)
                                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Domain.Entities.Game>()
                .HasOne(g => g.HomeTeam)
                .WithMany(t => t.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Domain.Entities.Game>()
                .HasOne(g => g.AwayTeam)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameTicket>()
                .HasOne(gt => gt.Game)
                .WithMany(g => g.Tickets)
                .HasForeignKey(gt => gt.GameId);

            modelBuilder.Entity<GameTicket>()
                .HasOne(gt => gt.Seat)
                .WithOne(s => s.Ticket)
                .HasForeignKey<GameTicket>(gt => gt.SeatId);

            modelBuilder.Entity<GameTicket>()
                .Property(gt => gt.Status)
                .HasConversion<int>()
                .IsRequired();
        }
    }
}
