using PurpleCable;
using UnityEngine;

public class BossBeatenDisplay : MonoBehaviour
{
    [SerializeField] GameObject[] BossBeatenUnitDisplays = null;

    [SerializeField] bool ShowScore = false;

    private void Start()
    {
        int count = (ShowScore ? ScoreManager.Score : GameManager.CurrentLevelNumber - 1);

        for (int i = 0; i < BossBeatenUnitDisplays.Length; i++)
        {
            BossBeatenUnitDisplays[i].SetActive(count > i);
        }
    }
}
