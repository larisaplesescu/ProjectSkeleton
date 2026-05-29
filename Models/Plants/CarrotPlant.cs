namespace TheAdventure.Models.Plants;

public class CarrotPlant : Plant
{
    public override string IngredientName => "Magic Carrot";
    protected override string FolderName => "carrot";
    protected override int TotalFrames => 16;

    public CarrotPlant() : base("Carrot", growthTimeMs: 30000, wiltTimeMs: 20000, waterIntervalMs: 15000) { }

    public override (byte R, byte G, byte B) GetStageColor() => Stage switch
    {
        PlantStage.Seedling => (144, 238, 144),
        PlantStage.Growing => (255, 165, 0),
        PlantStage.ReadyToHarvest => (255, 100, 0),
        PlantStage.Wilted => (101, 67, 33),
        _ => (200, 200, 200)
    };
}