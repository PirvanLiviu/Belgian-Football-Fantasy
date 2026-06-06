namespace Api.Models;

public class Player {
  public int Id { get; set; }
  public string Name { get; set; }
  public string Nationality { get; set; }
  public int TeamId { get; set; }
  public Team Team { get; set; } = null!;
  public string Face { get; set; }
  public float Price { get; set; }
  public string Position { get; set; }
}
