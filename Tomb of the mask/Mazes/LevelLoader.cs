using System.Text.Json;

public static class LevelLoader
{
    public static LevelData LoadLevelData(string projectPath, int number)
    {
        
        string levelsFolder = Path.Combine(projectPath, "Mazes\\Levels");
        string levelsFilePath = Path.Combine(levelsFolder, $"Level" + number.ToString() + ".JSON");
        string json = File.ReadAllText(levelsFilePath);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
        
        var levelData = JsonSerializer.Deserialize<LevelData>(json, options);
        
        if (levelData == null)
            throw new InvalidOperationException("Failed to load level data");
        
        return levelData;
    }
}

public record LevelData(
    int Width,
    int Height,
    char[][] Grid,
    Point Start,
    Point Exit);
    