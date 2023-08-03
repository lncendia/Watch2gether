namespace Overoom.Application.Abstractions.Rooms.DTOs;

public class AllowsDto
{
    public AllowsDto(bool beep, bool scream, bool change)
    {
        Beep = beep;
        Scream = scream;
        Change = change;
    }

    public bool Beep { get; }
    public bool Scream { get; }
    public bool Change { get; }
}