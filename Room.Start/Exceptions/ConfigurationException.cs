namespace Room.Start.Exceptions;

public class ConfigurationException(string name) : Exception($"The configuration path \"{name}\" does not exist")
{
    public string Name { get; } = name;
}