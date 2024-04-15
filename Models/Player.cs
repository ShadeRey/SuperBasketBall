using System;

namespace SuperBasketBall.Models;

public class Player
{
    public int Id { get; set; }
    public string PlayerSurname { get; set; }
    public int Position { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public DateTimeOffset BirthDate { get; set; }
    public DateTimeOffset StartGameDate { get; set; }
    public int Team { get; set; }
    public string PositionName { get; set; }
    public string TeamName { get; set; }
}