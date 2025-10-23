using UnityEngine;

public static class LevelScores 
{
    public static void SetHighScore(string levelName, int score)
    {
        if (!PlayerPrefs.HasKey(levelName))
        {
            PlayerPrefs.SetInt(levelName, 0);
        }
        
        int oldScore = GetHighScore(levelName);
        if (score >= oldScore)
        {
            PlayerPrefs.SetInt(levelName, score);
        }
    }

    public static int GetHighScore(string levelName)
    {
        return PlayerPrefs.GetInt(levelName);
    }
}
