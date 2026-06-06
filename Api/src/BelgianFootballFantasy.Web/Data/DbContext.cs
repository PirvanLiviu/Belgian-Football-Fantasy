using Microsoft.EntityFrameworkCore;
using Api.Models;


namespace Api.Data;

public class FootballFantasyDb : DbContext
{
  public FootballFantasyDb(DbContextOptions<FootballFantasyDb> opts) : base(opts)
  {}

  public DbSet<User> Users { get; set; }
  public DbSet<Player> Players { get; set; }
  public DbSet<League> Leagues { get; set; }
  public DbSet<Match> Match { get; set; }
  public DbSet<Team> Teams { get; set; }
  public DbSet<Gameweek> Gameweeks { get; set; }

  public DbSet<PlayerStats> PlayerStats { get; set; }
  public DbSet<UserLeague> UserLeague { get; set; }
  public DbSet<UserPlayer> UserPlayer { get; set; }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserPlayer>()
            .HasKey(up => new { up.UserId, up.PlayerId });

        modelBuilder.Entity<PlayerStats>()
            .HasKey(ps => new { ps.PlayerId, ps.MatchId });

        modelBuilder.Entity<UserLeague>()
            .HasKey(ul => new { ul.UserId, ul.LeagueId });
    }
}
