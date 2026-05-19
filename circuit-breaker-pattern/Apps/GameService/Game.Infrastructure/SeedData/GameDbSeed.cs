using Game.Domain.Entities;
using Game.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace Game.Infrastructure.SeedData
{
    public static class GameDbSeed
    {
        public static async Task SeedAsync(GameDbContext db)
        {
            if (await db.Stadiums.AnyAsync()) return; // idempotent
            var homeStadium = new Stadium { Id = Guid.NewGuid(), Name = "Marakana", City = "Belgrade", Capacity = 55000 };
            var awayStadium = new Stadium { Id = Guid.NewGuid(), Name = "Partizan Stadium", City = "Belgrade", Capacity = 32000 };
            var home = new Team { Id = Guid.NewGuid(), Name = "Crvena Zvezda", City = "Belgrade", StadiumId = homeStadium.Id };
            var away = new Team { Id = Guid.NewGuid(), Name = "Partizan", City = "Belgrade", StadiumId = awayStadium.Id };
            var seats = Enumerable.Range(1, 20)
                .Select(n => new StadiumSeat { Id = Guid.NewGuid(), StadiumId = homeStadium.Id, Level = 1, SeatNumber = n })
                .ToList();

            var game = new Game.Domain.Entities.Game
            {
                Id = Guid.NewGuid(),
                HomeTeamId = home.Id,
                AwayTeamId = away.Id,
                StadiumId = homeStadium.Id,
                Date = DateTime.UtcNow.AddDays(7)
            };

            db.AddRange(homeStadium, awayStadium, home, away);
            var tickets = seats.Select(s => new GameTicket { Id = Guid.NewGuid(), GameId = game.Id, SeatId = s.Id, Price = 1500, Status = TicketStatus.Available }).ToList();
            db.AddRange(seats);
            db.Add(game);
            db.AddRange(tickets);
            await db.SaveChangesAsync();
        }
    }
}
