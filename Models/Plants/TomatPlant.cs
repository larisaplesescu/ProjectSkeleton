namespace TheAdventure.Models.Plants;

public class TomatPlant : Plant
{
    public override string IngredientName => "Magic Tomato";
    protected override string FolderName => "tomat";
    protected override int TotalFrames => 20;

    public TomatPlant() : base("Tomato", growthTimeMs: 35000, wiltTimeMs: 20000, waterIntervalMs: 15000) { }

    public override (byte R, byte G, byte B) GetStageColor() => Stage switch
    {
        PlantStage.Seedling => (144, 238, 144),
        PlantStage.Growing => (200, 100, 100),
        PlantStage.ReadyToHarvest => (255, 50, 50),
        PlantStage.Wilted => (101, 67, 33),
        _ => (200, 200, 200)
    };
}