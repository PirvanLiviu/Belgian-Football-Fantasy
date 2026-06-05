namespace Api.Models;

public class Match
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public Team HomeTeam { get; set; } = null!;

    public int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = null!;

    public DateTime Date { get; set; }
    public string Score { get; set; }
}
