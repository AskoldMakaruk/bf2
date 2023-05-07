using Geolocation;

namespace EconomicSimulator.Lib.Properties;

public readonly record struct Location(double Latitude, double Longitude)
{
    public static implicit operator Coordinate(Location location) => new Coordinate(location.Latitude, location.Longitude);
    public static implicit operator Location(Coordinate coordinate) => new Location(coordinate.Latitude, coordinate.Longitude);
}