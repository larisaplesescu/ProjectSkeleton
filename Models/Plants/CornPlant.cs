namespace TheAdventure.Models.Plants;

public class CornPlant : Plant
{
    public override string IngredientName => "Magic Corn";
    protected override string FolderName => "corn";
    protected override int TotalFrames => 20;

    public CornPlant() : base("Corn", growthTimeMs: 35000, wiltTimeMs: 20000, waterIntervalMs: 15000) { }

    public override (byte R, byte G, byte B) GetStageColor() => Stage switch
    {
        PlantStage.Seedling => (144, 238, 144),
        PlantStage.Growing => (255, 230, 50),
        PlantStage.ReadyToHarvest => (255, 215, 0),
        PlantStage.Wilted => (101, 67, 33),
        _ => (200, 200, 200)
    };
}