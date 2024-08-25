using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "RPG/Enemy")]
public class EnemyData : UnitData
{
    [Space]
    [SerializeField] private float _spawnChance;

    public float spawnChance => _spawnChance;

    /// <summary>
    /// Ensures that the enemy's attributes are non-negative.
    /// </summary>
    protected override void OnValidate()
    {
        _spawnChance = Mathf.Max(_spawnChance, 0f);

        base.OnValidate();
    }
}
