namespace TheAdventure.Exceptions;

public class PotionGardenException : Exception
{
    public PotionGardenException(string message) : base(message) { }
    public PotionGardenException(string message, Exception inner) : base(message, inner) { }
}

public class InvalidRecipeException : PotionGardenException
{
    public InvalidRecipeException(string recipeName)
        : base($"Recipe '{recipeName}' is invalid or ingredients don't match.") { }
}

public class InsufficientIngredientsException : PotionGardenException
{
    public InsufficientIngredientsException(string ingredient)
        : base($"Not enough '{ingredient}' in inventory.") { }
}

public class SaveDataCorruptedException : PotionGardenException
{
    public SaveDataCorruptedException(string path)
        : base($"Save file at '{path}' is corrupted or unreadable.") { }
}