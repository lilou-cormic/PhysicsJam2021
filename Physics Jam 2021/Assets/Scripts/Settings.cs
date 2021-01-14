using UnityEngine;

public static class Settings
{
    public static bool NoScreenShake { get => PlayerPrefs.GetInt("NoScreenShake") == 1; set => PlayerPrefs.SetInt("NoScreenShake", (value ? 1 : 0)); }
}