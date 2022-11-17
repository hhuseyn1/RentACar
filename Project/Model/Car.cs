using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Project.Model;

public class Car
{
    public int Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }
    public string Price { get; set; }

    public int Page { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}