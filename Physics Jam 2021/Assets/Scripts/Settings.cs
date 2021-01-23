using UnityEngine;

public static class Settings
{
    public static bool NoScreenShake { get => PlayerPrefs.GetInt("NoScreenShake") == 1; set => PlayerPrefs.SetInt("NoScreenShake", (value ? 1 : 0)); }

    public static DifficultyMode Difficulty { get => (DifficultyMode)PlayerPrefs.GetInt("Difficulty"); set => PlayerPrefs.SetInt("Difficulty", (int)value); }

    public enum DifficultyMode
    {
        Normal = 0,

        Hard = 1,
    }
}