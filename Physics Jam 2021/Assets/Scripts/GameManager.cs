using System;
using System.Collections;
using System.Linq;
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

    [SerializeField] Enemy[] EnemyPrefabs = null;

    [SerializeField] HealthPickup _HealthPickupPrefab = null;
    public static HealthPickup HealthPickupPrefab => Current._HealthPickupPrefab;

    private static GameManager Current { get; set; }

    public static Enemy GetEnemyPrefab(EnemyType enemyType)
        => Current.EnemyPrefabs.FirstOrDefault(x => x.EnemyType == enemyType) ?? throw new NotImplementedException($"{enemyType} not found in GameManager.EnemyPrefabs");

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

    private void Awake()
    {
        Current = this;

        if (Levels == null)
            Levels = new Levels();

        if (CurrentLevelNumber <= 0)
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
        CurrentLevelNumber++;

        yield return new WaitForSeconds(2f);

        if (CurrentLevel != null)
            SceneManager.LoadScene("Main");
        else
            SceneManager.LoadScene("Win");
    }

    public static void GameOver()
        => Current.StartCoroutine(Current.DoGameOver());

    public IEnumerator DoGameOver()
    {
        _CurrentLevelNumber = 1;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("GameOver");
    }
}