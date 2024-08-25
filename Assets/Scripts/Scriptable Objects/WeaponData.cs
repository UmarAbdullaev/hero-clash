using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "RPG/Weapon")]
public class WeaponData : ScriptableObject
{
    public enum Type
    {
        melee = 0,
        ranged = 1,
    }

    [SerializeField] private Type _type;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _changeTime;

    public Type type => _type;
    public float attackTime => _attackTime;
    public float changeTime => _changeTime;

    private void OnValidate()
    {
        _attackTime = Mathf.Max(_attackTime, 0f);
        _changeTime = Mathf.Max(_changeTime, 0f);
    }
}