namespace Films.Infrastructure.Web.Models.Settings;

public class AllowsViewModel
{
    public AllowsViewModel(bool beep, bool scream, bool change)
    {
        Beep = beep;
        Scream = scream;
        Change = change;
    }

    public bool Beep { get; }
    public bool Scream { get; }
    public bool Change { get; }
}