namespace TheAdventure.Models.Interfaces;

public interface IHarvestable
{
    string IngredientName { get; }
    bool IsReadyToHarvest { get; }
    string Harvest();
}