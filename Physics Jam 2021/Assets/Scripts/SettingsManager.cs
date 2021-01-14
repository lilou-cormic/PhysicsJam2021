using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] Toggle ScreenShakeToggle = null;

    private void Awake()
    {
        ScreenShakeToggle.isOn = !Settings.NoScreenShake;

        ScreenShakeToggle.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    private void OnValueChanged()
    {
        Settings.NoScreenShake = !ScreenShakeToggle.isOn;
    }
}