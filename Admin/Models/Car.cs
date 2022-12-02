using System.Collections.ObjectModel;

namespace Admin.Models;

public class Car
{
    public static ObservableCollection<Car> AllCars { get; set; }
    public int Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }
    public string Price { get; set; }
    public int isRented { get; set; }

    public int Page { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}