using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ButtonText = null;

    [SerializeField] Button Button = null;

    private float _coolDown = 0f;

    private void Awake()
    {
        SetText();
    }

    private void Update()
    {
        if (_coolDown > 0)
        {
            _coolDown -= Time.deltaTime;
            return;
        }

        if (FindObjectOfType<EventSystem>().currentSelectedGameObject == gameObject)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                SwitchDifficulty();

                _coolDown = 0.5f;
            }
        }
    }

    public void SwitchDifficulty()
    {
        if (Settings.Difficulty == Settings.DifficultyMode.Normal)
            Settings.Difficulty = Settings.DifficultyMode.Hard;
        else
            Settings.Difficulty = Settings.DifficultyMode.Normal;

        SetText();
    }

    private void SetText()
    {
        ButtonText.text = $"◄ {Settings.Difficulty} ►";
    }
}
