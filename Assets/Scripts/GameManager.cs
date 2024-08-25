using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BattleController _battleController;

    private void Start()
    {
        _battleController = FindObjectOfType<BattleController>();
    }

    public void BeginBattle()
    {
        _battleController.Battle(true);
    }

    public void EndBattle()
    {
        _battleController.Battle(false);
    }

    public void ReplenishHealth()
    {
        _battleController.player.health.Heal(_battleController.player.health.GetMaxHealth());
    }

    public void SwitchWeapon()
    {
        _battleController.player.SwitchWeapon();
    }

    public Unit Spawn(UnitData unitData, Vector3 position, Quaternion rotation)
    {
        Unit unit = Instantiate(unitData.prefab, position, rotation)
            .GetComponent<Unit>();

        unit.Set(unitData);

        return unit;
    }
}
