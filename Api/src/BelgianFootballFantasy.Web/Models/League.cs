using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class League
{
  [Key]
  public int Id { get; set; }
  public string Name { get; set; }
  public string Code { get; set; }
}
