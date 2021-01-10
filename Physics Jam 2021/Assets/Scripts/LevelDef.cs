using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Game/Level", order = 2)]
public class LevelDef : ScriptableObject
{
    public int BossHP = 5;

    public EnemyType[] EnemyTypes;

    public bool GravityWave = false;
}