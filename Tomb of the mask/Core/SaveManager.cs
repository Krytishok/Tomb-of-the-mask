public static class SaveManager
{
    private static readonly string SavePath = "Core\\highscore.dat";
    private static string _projectPath = new DirectoryInfo(
        Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();

    public static int LoadHighScore()
    {
        try
        {
            if (File.Exists(Path.Combine(_projectPath, SavePath)))
            {
                string text = File.ReadAllText(Path.Combine(_projectPath, SavePath));
                Console.WriteLine("Record " + text);
                return int.TryParse(text, out int score) ? score : 0;
            }
        }
        catch { /* Ошибка чтения - вернем 0 */ }
        return 0;
    }

    public static void SaveHighScore(int score)
    {
        try
        {
            File.WriteAllText(Path.Combine(_projectPath, SavePath), score.ToString());
        }
        catch { /* Не удалось сохранить */ }
    }
}