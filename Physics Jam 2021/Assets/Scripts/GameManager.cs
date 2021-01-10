using PurpleCable;
using System;
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

    [SerializeField] Player Player = null;

    [SerializeField] Enemy[] EnemyPrefabs = null;

    private static GameManager Current { get; set; }

    public static Enemy GetEnemyPrefab(EnemyType enemyType)
        => Current.EnemyPrefabs.FirstOrDefault(x => x.EnemyType == enemyType) ?? throw new NotImplementedException($"{enemyType} not found in GameManager.EnemyPrefabs");

    private int _CurrentLevelNumber = 0;
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

    public static void SetGravity(int gravity)
    {
        Current._Gravity = gravity;

        EnemyPool.SetGravity(Gravity);

        Current.Player.SetGravity(Gravity);
    }

    public static void Win()
        => Current.WinInternal();

    private void WinInternal()
    {
        CurrentLevelNumber++;

        if (CurrentLevel != null)
            SceneManager.LoadScene("Main");
        else
            SceneManager.LoadScene("Win");
    }

    public static void GameOver()
        => Current.GameOverInternal();

    public void GameOverInternal()
    {
        _CurrentLevelNumber = 1;

        SceneManager.LoadScene("GameOver");
    }
}