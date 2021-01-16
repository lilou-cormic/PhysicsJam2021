using PurpleCable;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] Sprite NormalImage = null;
    [SerializeField] Sprite BeatenImage = null;

    [SerializeField] Color PreviousColor = new Color(1, 1, 1, 200 / 255f);
    [SerializeField] Color NextColor = new Color(1, 1, 1, 150 / 255f);

    [SerializeField] protected Image[] LevelUnitDisplays = null;

    [SerializeField] bool ShowScore = false;

    private void Start()
    {
        int count = (ShowScore ? ScoreManager.Score : GameManager.CurrentLevelNumber);

        for (int level = 1; level <= LevelUnitDisplays.Length; level++)
        {
            SetSize(level, 1);

            if (level < count)
                SetAsPrevious(level);
            else if (level == count)
                SetAsCurrent(level);
            else
                SetAsNext(level);
        }
    }

    public void SetSize(int level, float size)
    {
        var unit = LevelUnitDisplays[level - 1];

        unit.gameObject.transform.localScale = new Vector3(size, size, size);
    }

    public void SetAsPrevious(int level)
    {
        var unit = LevelUnitDisplays[level - 1];

        unit.color = PreviousColor;
        unit.sprite = BeatenImage;
    }

    public void SetAsCurrent(int level)
    {
        var unit = LevelUnitDisplays[level - 1];

        unit.color = Color.white;
        unit.sprite = NormalImage;
    }

    public void SetAsNext(int level)
    {
        var unit = LevelUnitDisplays[level - 1];

        unit.color = NextColor;
        unit.sprite = NormalImage;
    }
}