using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LevelDisplay))]
public class LevelBeatenDisplay : MonoBehaviour
{
    private LevelDisplay LevelDisplay = null;

    private const float BigSize = 1.2f;

    private void Awake()
    {
        LevelDisplay = GetComponent<LevelDisplay>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        int current = ScoreManager.Score;

        for (float size = 1; size < BigSize; size += 0.1f)
        {
            LevelDisplay.SetSize(current, size);

            yield return new WaitForSeconds(0.05f);
        }

        LevelDisplay.SetSize(current, BigSize);
        LevelDisplay.SetAsPrevious(current);

        yield return new WaitForSeconds(0.5f);

        for (float size = BigSize; size > 1; size -= 0.1f)
        {
            LevelDisplay.SetSize(current, size);

            yield return new WaitForSeconds(0.05f);
        }

        LevelDisplay.SetSize(current, 1);

        yield return new WaitForSeconds(0.5f);

        int next = ScoreManager.Score + 1;

        for (float size = 1; size < BigSize; size += 0.1f)
        {
            LevelDisplay.SetSize(next, size);

            yield return new WaitForSeconds(0.05f);
        }

        LevelDisplay.SetSize(next, BigSize);
        LevelDisplay.SetAsCurrent(next);
    }
}
