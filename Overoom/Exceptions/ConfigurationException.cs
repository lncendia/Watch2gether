namespace Overoom.Exceptions;

public class ConfigurationException : Exception
{
    public string Name { get; }

    public ConfigurationException(string name) : base($"The configuration path \"{name}\" does not exist")
    {
        Name = name;
    }
}