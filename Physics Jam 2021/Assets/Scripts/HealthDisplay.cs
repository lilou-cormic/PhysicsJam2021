using PurpleCable;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Health Health = null;

    [SerializeField] GameObject[] HealthUnitDisplays = null;

    private void Awake()
    {
        Health.HPChanged += Health_HPChanged;
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < HealthUnitDisplays.Length; i++)
        {
            HealthUnitDisplays[i].SetActive(Health.CurrentHP > i);
        }
    }

    private void Health_HPChanged(int amount)
    {
        UpdateDisplay();
    }
}
