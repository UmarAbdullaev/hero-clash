using UnityEngine;

public class UnitData : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _armour;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _attackPower;
    [SerializeField] private float _attackPreparationTime;

    public GameObject prefab => _prefab;
    public float armour => _armour;
    public float maxHealth => _maxHealth;
    public float attackPower => _attackPower;
    public float attackPreparationTime => _attackPreparationTime;

    /// <summary>
    /// Ensures that the unit's attributes are non-negative.
    /// </summary>
    protected virtual void OnValidate()
    {
        _armour = Mathf.Max(_armour, 0f);
        _maxHealth = Mathf.Max(_maxHealth, 0f);
        _attackPower = Mathf.Max(_attackPower, 0f);
        _attackPreparationTime = Mathf.Max(_attackPreparationTime, 0f);
    }
}

