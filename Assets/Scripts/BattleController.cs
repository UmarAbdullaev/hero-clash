using UnityEngine;

public class BattleController : MonoBehaviour
{
    private Unit _player, _enemy;

    public Unit player => _player;
    public Unit enemy => _enemy;

    private bool _battle;

    public void SetPlayer(Unit player)
    {
        this._player = player;
        if (_enemy != null)
        {
            player.SetOpponent(_enemy);
            _enemy.SetOpponent(player);
        }

        _player.Activity(_battle);
    }

    public void SetEnemy(Unit enemy)
    {
        this._enemy = enemy;
        if (_player != null)
        {
            enemy.SetOpponent(_player);
            _player.SetOpponent(enemy);
        }

        _enemy.Activity(_battle);
    }

    public void Battle(bool value)
    {
        _battle = value;

        _player.Activity(_battle);
        _enemy.Activity(_battle);
    }
}
