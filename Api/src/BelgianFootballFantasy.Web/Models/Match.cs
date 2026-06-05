namespace Api.Models;

public class Match
{
    public Int Id { get; set; }
    public Int HomeTeamId { get; set; }
    public Team HomeTeam { get; set; } = null!;

    public Int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = null!;

    public DateTime Date { get; set; }
    public string Score { get; set; }
}
