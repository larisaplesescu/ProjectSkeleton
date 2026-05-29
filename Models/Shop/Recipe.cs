using TheAdventure.Exceptions;

namespace TheAdventure.Models.Shop;

public record Ingredient(string Name, int Amount);

public class Recipe
{
    public string PotionName { get; }
    public string Description { get; }
    public int GoldValue { get; }
    public IReadOnlyList<Ingredient> Ingredients { get; }
    public (byte R, byte G, byte B) Color { get; }

    public Recipe(string potionName, string description, int goldValue,
        (byte R, byte G, byte B) color, params Ingredient[] ingredients)
    {
        PotionName = potionName;
        Description = description;
        GoldValue = goldValue;
        Color = color;
        Ingredients = ingredients;
    }

    public bool CanCraft(Inventory<string> inventory) =>
        Ingredients.All(i => inventory.Has(i.Name, i.Amount));

    public void Craft(Inventory<string> inventory)
    {
        if (!CanCraft(inventory))
            throw new InvalidRecipeException(PotionName);

        foreach (var ingredient in Ingredients)
        {
            if (!inventory.Remove(ingredient.Name, ingredient.Amount))
                throw new InsufficientIngredientsException(ingredient.Name);
        }
    }
}