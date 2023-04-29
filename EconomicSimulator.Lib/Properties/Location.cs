using Geolocation;

namespace EconomicSimulator.Lib.Properties;

public class Location
{
    public Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public static implicit operator Coordinate(Location location) => new Coordinate(location.Latitude, location.Longitude);
    public static implicit operator Location(Coordinate coordinate) => new Location(coordinate.Latitude, coordinate.Longitude);
}