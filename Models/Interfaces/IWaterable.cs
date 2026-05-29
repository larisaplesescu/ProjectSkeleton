namespace TheAdventure.Models.Interfaces;

public interface IWaterable
{
    bool NeedsWater { get; }
    void Water();
}