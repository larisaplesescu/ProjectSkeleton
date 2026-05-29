using TheAdventure.Models.Interfaces;

namespace TheAdventure.Models.Plants;

public enum PlantStage { Empty, Seedling, Growing, ReadyToHarvest, Wilted }

public abstract class Plant : IHarvestable, IWaterable
{
    public string Name { get; protected set; }
    public PlantStage Stage { get; protected set; } = PlantStage.Seedling;
    public abstract string IngredientName { get; }
    public bool IsReadyToHarvest => Stage == PlantStage.ReadyToHarvest;
    public bool NeedsWater { get; protected set; } = true; // începe cu nevoie de apă!

    protected double _growthProgress = 0.0;
    protected double _growthTimeMs;
    protected double _wiltTimeMs;
    protected double _timeSinceWatered = 0.0;
    protected double _waterIntervalMs;
    protected bool _isGrowing = false; // crește doar după ce e udat

    protected abstract string FolderName { get; }
    protected abstract int TotalFrames { get; }

    protected Plant(string name, double growthTimeMs, double wiltTimeMs, double waterIntervalMs)
    {
        Name = name;
        _growthTimeMs = growthTimeMs;
        _wiltTimeMs = wiltTimeMs;
        _waterIntervalMs = waterIntervalMs;
    }

    public virtual void Update(double deltaMs)
    {
        if (Stage == PlantStage.Wilted || Stage == PlantStage.ReadyToHarvest) return;

        if (!_isGrowing) return; // nu crește până nu e udat prima dată

        _timeSinceWatered += deltaMs;

        // Cere apă din nou după waterInterval
        if (_timeSinceWatered > _waterIntervalMs && !NeedsWater)
            NeedsWater = true;

        // Se ofilește dacă nu e udat la timp
        if (NeedsWater && _timeSinceWatered > _waterIntervalMs + _wiltTimeMs)
        {
            Stage = PlantStage.Wilted;
            _isGrowing = false;
            return;
        }

        // Crește doar când are apă
        if (!NeedsWater)
        {
            _growthProgress += deltaMs;

            if (_growthProgress >= _growthTimeMs * 0.4 && Stage == PlantStage.Seedling)
                Stage = PlantStage.Growing;

            if (_growthProgress >= _growthTimeMs)
                Stage = PlantStage.ReadyToHarvest;
        }
    }

    public void Water()
    {
        NeedsWater = false;
        _timeSinceWatered = 0;
        _isGrowing = true; // pornește creșterea la primul ud!
    }

    public virtual string Harvest()
    {
        if (!IsReadyToHarvest)
            throw new Exceptions.PotionGardenException($"{Name} is not ready to harvest!");
        Stage = PlantStage.Seedling;
        _growthProgress = 0;
        _timeSinceWatered = 0;
        NeedsWater = true;
        _isGrowing = false;
        return IngredientName;
    }

    public string? GetCurrentFramePath()
    {
        if (Stage == PlantStage.Wilted) return null;

        double progress = Math.Clamp(_growthProgress / _growthTimeMs, 0.0, 1.0);
        int frameIndex = (int)(progress * (TotalFrames - 1));
        frameIndex = Math.Clamp(frameIndex, 0, TotalFrames - 1);

        return Path.Combine("Assets", "Plants", FolderName, $"{FolderName}_{frameIndex + 1}.png");
    }

    public abstract (byte R, byte G, byte B) GetStageColor();
}