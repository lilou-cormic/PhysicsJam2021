using PurpleCable;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CameraShaker))]
public class GameManager : MonoBehaviour
{
    public const string WallTag = "Wall";

    public const string CeilingTag = "Ceiling";

    public const string FloorTag = "Floor";

    public const string SpikesTag = "Spikes";

    public static Levels Levels { get; private set; }

    [SerializeField] Player _Player = null;
    public static Player Player => Current._Player;

    [SerializeField] MainMenu MainMenu = null;

    private CameraShaker CameraShaker = null;

    private static GameManager Current { get; set; }

    public static int CurrentLevelNumber { get; private set; } = 0;

    public int LevelNumber
    {
        get => CurrentLevelNumber;

        set
        {
            CurrentLevelNumber = value;

            CurrentLevel = Levels.Get(CurrentLevelNumber);
        }
    }

    public static LevelDef CurrentLevel { get; private set; } = null;

    private float _Gravity = 1;
    public static float Gravity => Current._Gravity;

    private bool _gameIsEnding = false;

    private void Awake()
    {
        Current = this;

        CameraShaker = GetComponent<CameraShaker>();

        if (Levels == null)
            Levels = new Levels();

        if (CurrentLevel == null)
            LevelNumber = 1;

        ScoreManager.ResetScore();
    }

    private void Start()
    {
        MusicManager.PlayMainMusic();
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

    public static void ShakeCamera()
    {
        if (!Settings.NoScreenShake)
            Current.CameraShaker.Shake();
    }

    public static void Win()
        => Current.StartCoroutine(Current.DoWin());

    private IEnumerator DoWin()
    {
        if (!_gameIsEnding)
        {
            _gameIsEnding = true;

            MusicManager.PlayWinJingle();

            ScoreManager.AddPoints(LevelNumber);
            ScoreManager.SetHighScore();

            LevelNumber++;

            yield return new WaitForSeconds(2f);

            if (CurrentLevel != null)
                MainMenu.StartCoroutine(MainMenu.GoToScene("Win"));
            else
                MainMenu.StartCoroutine(MainMenu.GoToScene("Congratulations"));
        }
    }

    public static void GameOver()
        => Current.StartCoroutine(Current.DoGameOver());

    public IEnumerator DoGameOver()
    {
        if (!_gameIsEnding)
        {
            _gameIsEnding = true;

            MusicManager.PlayLoseJingle();

            ScoreManager.AddPoints(LevelNumber - 1);
            ScoreManager.SetHighScore();

            LevelNumber = 1;

            yield return new WaitForSeconds(2f);

            MainMenu.StartCoroutine(MainMenu.GoToScene("GameOver"));
        }
    }
}