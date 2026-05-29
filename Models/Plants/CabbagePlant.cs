namespace TheAdventure.Models.Plants;

public class CabbagePlant : Plant
{
    public override string IngredientName => "Magic Cabbage";
    protected override string FolderName => "cabbage";
    protected override int TotalFrames => 20;

    public CabbagePlant() : base("Cabbage", growthTimeMs: 30000, wiltTimeMs: 20000, waterIntervalMs: 15000) { }

    public override (byte R, byte G, byte B) GetStageColor() => Stage switch
    {
        PlantStage.Seedling => (144, 238, 144),
        PlantStage.Growing => (0, 180, 80),
        PlantStage.ReadyToHarvest => (0, 255, 100),
        PlantStage.Wilted => (101, 67, 33),
        _ => (200, 200, 200)
    };
}