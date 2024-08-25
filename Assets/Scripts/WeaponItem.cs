using UnityEngine;

[System.Serializable]
public class WeaponItem
{
    [SerializeField] private GameObject _object;
    [SerializeField] private WeaponData _data;

    public WeaponData data => _data;

    public void Activity(bool value)
    {
        _object.SetActive(value);
    }
}
