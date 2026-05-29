using System.Text.Json;
using TheAdventure.Exceptions;
using TheAdventure.Models.Data;
using TheAdventure.Models.Game;
using TheAdventure.Models.Shop;

namespace TheAdventure.Services;

public class SaveManager : IDisposable
{
    private const string SavePath = "save.json";
    private bool _disposed = false;

    public async Task SaveAsync(GameState state, Inventory<string> ingredients, List<Potion> potions)
    {
        var data = new SaveData
        {
            Gold = state.Gold,
            Day = state.Day,
            Score = state.Score,
            HighScore = state.HighScore,
            ShopLevel = state.ShopLevel,
            PotionsCrafted = state.PotionsCrafted,
            CustomersServed = state.CustomersServed,
            Ingredients = ingredients.Items.ToDictionary(k => k.Key, v => v.Value),
            PotionsInStock = potions.Select(p => p.Name).ToList()
        };

        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(SavePath, json);
    }

    public async Task<SaveData?> LoadAsync()
    {
        if (!File.Exists(SavePath)) return null;
        try
        {
            var json = await File.ReadAllTextAsync(SavePath);
            return JsonSerializer.Deserialize<SaveData>(json);
        }
        catch (Exception)
        {
            throw new SaveDataCorruptedException(SavePath);
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}