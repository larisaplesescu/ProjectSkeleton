namespace TheAdventure.Models.Data;

public class SaveData
{
	public int Gold { get; set; }
	public int Day { get; set; }
	public int Score { get; set; }
	public int HighScore { get; set; }
	public int ShopLevel { get; set; }
	public int PotionsCrafted { get; set; }
	public int CustomersServed { get; set; }
	public Dictionary<string, int> Ingredients { get; set; } = new();
	public List<string> PotionsInStock { get; set; } = new();
}