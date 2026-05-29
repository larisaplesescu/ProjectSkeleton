namespace TheAdventure.Models.Shop;

public enum CustomerMood { Happy, Neutral, Impatient }

public class Customer
{
    public string Name { get; }
    public string RequestedPotion { get; }
    public int OfferedGold { get; }
    public CustomerMood Mood { get; private set; } = CustomerMood.Neutral;
    public bool IsServed { get; private set; } = false;
    private double _waitTimeMs = 0;
    private const double MaxWaitMs = 30000;

    public Customer(string name, string requestedPotion, int offeredGold)
    {
        Name = name;
        RequestedPotion = requestedPotion;
        OfferedGold = offeredGold;
    }

    public void Update(double deltaMs)
    {
        if (IsServed) return;
        _waitTimeMs += deltaMs;
        Mood = _waitTimeMs switch
        {
            < MaxWaitMs * 0.5 => CustomerMood.Happy,
            < MaxWaitMs * 0.8 => CustomerMood.Neutral,
            _ => CustomerMood.Impatient
        };
    }

    public bool HasLeft => _waitTimeMs > MaxWaitMs && !IsServed;

    public int Serve()
    {
        IsServed = true;
        return Mood == CustomerMood.Happy ? (int)(OfferedGold * 1.2) :
               Mood == CustomerMood.Neutral ? OfferedGold :
               (int)(OfferedGold * 0.8);
    }
}