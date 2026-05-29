namespace TheAdventure.Models.Game;

public enum GamePhase { Playing, GameOver, Victory }

public class GameState
{
	public int Gold { get; set; } = 50;
	public int Day { get; set; } = 1;
	public int Score { get; set; } = 0;
	public int HighScore { get; set; } = 0;
	public int ShopLevel { get; set; } = 1;
	public GamePhase Phase { get; set; } = GamePhase.Playing;
	public int PotionsCrafted { get; set; } = 0;
	public int CustomersServed { get; set; } = 0;

	public bool CanUpgradeShop => Gold >= UpgradeCost && ShopLevel < 3;
	public int UpgradeCost => ShopLevel * 500;
	public bool HasWon => ShopLevel >= 3;
}