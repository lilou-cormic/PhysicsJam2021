using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const string WallTag = "Wall";

    public const string CeilingTag = "Ceiling";

    public const string FloorTag = "Floor";

    public const string SpikesTag = "Spikes";

    public static Levels Levels { get; private set; }

    [SerializeField] Player _Player = null;
    public static Player Player => Current._Player;

    private static GameManager Current { get; set; }

    private static int _CurrentLevelNumber = 0;
    public int CurrentLevelNumber
    {
        get => _CurrentLevelNumber;

        set
        {
            _CurrentLevelNumber = value;

            CurrentLevel = Levels.Get(_CurrentLevelNumber);
        }
    }

    public static LevelDef CurrentLevel { get; private set; } = null;

    private float _Gravity = 1;
    public static float Gravity => Current._Gravity;

    private bool _gameIsEnding = false;

    private void Awake()
    {
        Current = this;

        if (Levels == null)
            Levels = new Levels();

        if (CurrentLevel == null)
            CurrentLevelNumber = 1;
    }

    private void OnDestroy()
    {
        Current = null;
    }

    public static void SetGravity(float gravity)
    {
        Current._Gravity = gravity;

        EnemyPool.SetGravity(gravity);

        HealthPickupPool.SetGravity(gravity);

        Player.SetGravity(gravity);
    }

    public static void Win()
        => Current.StartCoroutine(Current.DoWin());

    private IEnumerator DoWin()
    {
        if (!_gameIsEnding)
        {
            _gameIsEnding = true;

            CurrentLevelNumber++;

            yield return new WaitForSeconds(2f);

            if (CurrentLevel != null)
                SceneManager.LoadScene("Win");
            else
                SceneManager.LoadScene("Congratulations");
        }
    }

    public static void GameOver()
        => Current.StartCoroutine(Current.DoGameOver());

    public IEnumerator DoGameOver()
    {
        if (!_gameIsEnding)
        {
            _gameIsEnding = true;

            CurrentLevelNumber = 1;

            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene("GameOver");
        }
    }
}