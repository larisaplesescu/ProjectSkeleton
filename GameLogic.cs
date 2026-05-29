
using TheAdventure.Exceptions;
using TheAdventure.Models.Game;
using TheAdventure.Models.Plants;
using TheAdventure.Models.Shop;
using TheAdventure.Services;

namespace TheAdventure;

public class GameLogic : IDisposable
{
    private readonly GameState _state = new();
    private readonly Inventory<string> _ingredients = new();
    private readonly List<Potion> _potionsInStock = new();
    private readonly List<Customer> _customers = new();
    private readonly SaveManager _saveManager = new();
    private bool _disposed = false;

    // Numele fișierului în care salvăm cel mai bun scor
    private const string HighScoreFile = "highscore.txt";

    public Plant?[] GardenBeds { get; } = new Plant?[4];

    private readonly Dictionary<string, int> _counts = new()
    {
        { "Magic Carrot", 0 },
        { "Magic Tomato", 0 },
        { "Magic Corn", 0 },
        { "Magic Cabbage", 0 }
    };

    private readonly Dictionary<string, int> _potionCounts = new()
    {
        { "Speed Potion", 0 },
        { "Fire Potion", 0 },
        { "Sun Potion", 0 },
        { "Nature Potion", 0 }
    };

    public List<Recipe> Recipes { get; } = new()
    {
        new Recipe("Speed Potion", "Made with carrots", 80, (255, 140, 0), new Ingredient("Magic Carrot", 2)),
        new Recipe("Fire Potion", "Made with tomatoes", 100, (255, 50, 50), new Ingredient("Magic Tomato", 2)),
        new Recipe("Sun Potion", "Made with corn", 90, (255, 215, 0), new Ingredient("Magic Corn", 2)),
        new Recipe("Nature Potion", "Made with cabbage", 70, (0, 200, 100), new Ingredient("Magic Cabbage", 2))
    };

    public GameState State => _state;
    public Inventory<string> Ingredients => _ingredients;
    public IReadOnlyList<Potion> PotionsInStock => _potionsInStock;
    public IReadOnlyList<Customer> Customers => _customers;

    public void InitializeGame()
    {
        _ingredients.Clear();
        foreach (var key in _counts.Keys.ToList()) _counts[key] = 0;
        foreach (var key in _potionCounts.Keys.ToList()) _potionCounts[key] = 0;
        _potionsInStock.Clear();

        // ÎNCĂRCARE HIGH SCORE: Citim cel mai bun scor salvat anterior, dacă fișierul există
        if (File.Exists(HighScoreFile))
        {
            try
            {
                string content = File.ReadAllText(HighScoreFile);
                if (int.TryParse(content, out int savedHighScore))
                {
                    _state.HighScore = savedHighScore;
                }
            }
            catch { }
        }

        _state.Phase = GamePhase.Playing;
        SpawnCustomer();
    }

    public void Update(double deltaMs)
    {
        if (_state.Phase != GamePhase.Playing) return;

        double acceleratedDelta = deltaMs * 4.0;
        foreach (var bed in GardenBeds)
        {
            bed?.Update(acceleratedDelta);
        }

        double slowedDelta = deltaMs / 3.0;
        for (int i = _customers.Count - 1; i >= 0; i--)
        {
            _customers[i].Update(slowedDelta);
        }

        if (_customers.Count < 3)
        {
            SpawnCustomer();
        }
    }

    private void SpawnCustomer()
    {
        string[] names = { "Alchemist John", "Wizard Gandalph", "Witch Morgana", "Pixie Lili", "Knight Arthur" };
        var rand = new Random();
        var name = names[rand.Next(names.Length)];

        string[] validPotions = { "Speed Potion", "Fire Potion", "Sun Potion", "Nature Potion" };
        var requested = validPotions[rand.Next(validPotions.Length)];
        int baseGold = requested switch { "Fire Potion" => 100, "Sun Potion" => 90, "Nature Potion" => 70, _ => 80 };

        _customers.Add(new Customer(name, requested, baseGold));
    }

    public string? HarvestBed(int bedIndex)
    {
        if (bedIndex < 0 || bedIndex >= GardenBeds.Length) return null;
        var p = GardenBeds[bedIndex];
        if (p == null || !p.IsReadyToHarvest) return null;

        string plantName = p.Name;
        string officialName = "Magic Carrot";

        if (plantName.Contains("Tomato", StringComparison.OrdinalIgnoreCase) || plantName.Contains("Tomat", StringComparison.OrdinalIgnoreCase))
            officialName = "Magic Tomato";
        else if (plantName.Contains("Corn", StringComparison.OrdinalIgnoreCase))
            officialName = "Magic Corn";
        else if (plantName.Contains("Cabbage", StringComparison.OrdinalIgnoreCase))
            officialName = "Magic Cabbage";

        _counts[officialName]++;

        try { ((dynamic)_ingredients).AddItem(officialName, 1); }
        catch { try { ((dynamic)_ingredients).Add(officialName, 1); } catch { } }

        GardenBeds[bedIndex] = null;
        return officialName;
    }

    public bool CraftPotion(int recipeIndex)
    {
        if (recipeIndex < 0 || recipeIndex >= Recipes.Count) return false;

        string officialName = recipeIndex switch
        {
            0 => "Magic Carrot",
            1 => "Magic Tomato",
            2 => "Magic Corn",
            _ => "Magic Cabbage"
        };

        int requiredIngredients = 2;
        if (_state.ShopLevel == 2) requiredIngredients = 3;
        else if (_state.ShopLevel >= 3) requiredIngredients = 4;

        if (_counts[officialName] < requiredIngredients) return false;

        _counts[officialName] -= requiredIngredients;

        try { ((dynamic)_ingredients).RemoveItem(officialName, requiredIngredients); }
        catch { try { ((dynamic)_ingredients).Remove(officialName, requiredIngredients); } catch { } }

        string basePotionName = recipeIndex switch { 0 => "Speed Potion", 1 => "Fire Potion", 2 => "Sun Potion", _ => "Nature Potion" };

        _potionCounts[basePotionName]++;
        RebuildPotionsInStockVisual();

        _state.PotionsCrafted++;
        _state.Score += 10;

        // Verificăm și salvăm dacă noul scor a bătut recordul
        CheckAndSaveHighScore();

        return true;
    }

    private void RebuildPotionsInStockVisual()
    {
        _potionsInStock.Clear();
        string[] order = { "Speed Potion", "Fire Potion", "Sun Potion", "Nature Potion" };

        for (int i = 0; i < order.Length; i++)
        {
            string name = order[i];
            int count = _potionCounts[name];
            if (count > 0)
            {
                var recipe = Recipes[i];
                string displayName = $"{name} x{count}";
                int price = i switch { 0 => 80, 1 => 100, 2 => 90, _ => 70 };

                try { _potionsInStock.Add(new Potion(displayName, "Description", price, recipe.Color)); }
                catch { _potionsInStock.Add((dynamic)new Potion(displayName, "Description", price, recipe.Color)); }
            }
        }
    }

    public bool HasPotionInStock(string potionName)
    {
        return _potionCounts.ContainsKey(potionName) && _potionCounts[potionName] > 0;
    }

    public int SellPotionGrouped(string requestedPotionName)
    {
        if (!_potionCounts.ContainsKey(requestedPotionName) || _potionCounts[requestedPotionName] <= 0) return 0;

        _potionCounts[requestedPotionName]--;
        RebuildPotionsInStockVisual();
        return 1;
    }

    public int SellPotion(int customerIndex)
    {
        if (customerIndex < 0 || customerIndex >= _customers.Count) return 0;
        var customer = _customers[customerIndex];

        if (!HasPotionInStock(customer.RequestedPotion)) return 0;

        SellPotionGrouped(customer.RequestedPotion);

        var gold = customer.Serve();
        _state.Gold += gold;
        _state.Score += 20;
        _state.CustomersServed++;
        _customers.RemoveAt(customerIndex);

        // Verificăm și salvăm dacă noul scor a bătut recordul
        CheckAndSaveHighScore();

        if (_state.Gold >= _state.UpgradeCost && _state.CanUpgradeShop)
            UpgradeShop();

        return gold;
    }

    // Funcție dedicată pentru a verifica și a scrie pe disk cel mai bun scor
    private void CheckAndSaveHighScore()
    {
        if (_state.Score > _state.HighScore)
        {
            _state.HighScore = _state.Score;
        }

        try
        {
            // Salvăm High Score-ul în fișier pentru a fi reținut permanent
            File.WriteAllText(HighScoreFile, _state.HighScore.ToString());
        }
        catch { }
    }

    public void UpgradeShop()
    {
        if (!_state.CanUpgradeShop) return;
        _state.Gold -= _state.UpgradeCost;
        _state.ShopLevel++;
        _state.Score += 100;

        // Verificăm și salvăm din nou scorul după bonusul de upgrade
        CheckAndSaveHighScore();

        if (_state.HasWon)
        {
            _state.Phase = GamePhase.Victory;
            // Forțăm salvarea recordului la momentul victoriei
            CheckAndSaveHighScore();
        }
    }

    public void PlantSeed(int bedIndex, int plantType)
    {
        if (bedIndex < 0 || bedIndex >= GardenBeds.Length) return;
        GardenBeds[bedIndex] = plantType switch
        {
            0 => new CarrotPlant(),
            1 => new TomatPlant(),
            2 => new CornPlant(),
            3 => new CabbagePlant(),
            _ => new CarrotPlant()
        };
    }

    public async Task SaveGameAsync()
    {
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
        }
    }
}
