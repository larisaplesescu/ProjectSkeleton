namespace TheAdventure.Models.Shop;

public record Potion(string Name, string Description, int GoldValue, (byte R, byte G, byte B) Color);