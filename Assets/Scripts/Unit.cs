using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Health))]
public class Unit : MonoBehaviour, IDamageable
{
    [SerializeField] private UnitData _unitData;
    [SerializeField] private WeaponItem[] _weapons;

    [Space]
    [SerializeField] private Animator _animator;
    [SerializeField] private Renderer[] _renderers;

    private int _currentWeaponIndex = 0;
    private Unit _opponent;
    private Health _health;
    private bool _isAlive, _isActive;

    public UnityAction onAttack;
    public UnitData data => _unitData;
    public bool isAlive => _isAlive;
    public Health health => _health;

    private UnitState _currentState = UnitState.Idle;

    private void Start()
    {
        _health = GetComponent<Health>();

        _health.Set(data.maxHealth);
        _health.onDamageTaken += HandleDamageTaken;
        _health.onDeath += HandleDeath;

        _isAlive = _health.GetHealth() > 0;

        onAttack = () =>
        {
            if (_opponent != null & isAlive & _currentState != UnitState.SwitchWeapon & _weapons.Length > 0)
            {
                _opponent.TakeDamage(data.attackPower);
            }
        };

        // Start the attack routine
        StartCoroutine(AttackRoutine());
        SwitchWeapon();
    }

    public void Set(UnitData data)
    {
        _unitData = data;
    }

    public void Activity(bool value) => _isActive = value;

    public void SetOpponent(Unit opponent)
    {
        this._opponent = opponent;
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (!_isActive)
            {
                yield return new WaitForEndOfFrame();

                continue;
            }

            bool isAllowedToAttack = _currentState != UnitState.SwitchWeapon & _weapons.Length > 0;

            // Prepare for attack
            yield return StartCoroutine(Perform(isAllowedToAttack & isAlive, UnitState.PreparingAttack, data.attackPreparationTime));

            isAllowedToAttack = _currentState != UnitState.SwitchWeapon & _weapons.Length > 0;

            // Perform attack
            yield return StartCoroutine(Perform(isAllowedToAttack & _opponent.isAlive & isAlive, UnitState.Attack, _weapons[_currentWeaponIndex].data.attackTime, () => onAttack?.Invoke()));
        }
    }

    private IEnumerator Perform(bool isAllowed, UnitState state, float duration, UnityAction action = null)
    {
        if (!isAllowed) yield break;

        Debug.Log(state);
        _currentState = state;
        _animator.SetTrigger(state.ToString());

        yield return new WaitForSeconds(duration);

        action?.Invoke();

        _currentState = UnitState.Idle;
    }

    public void SwitchWeapon()
    {
        bool isAllowedToSwitch = _currentState != UnitState.Attack && _weapons.Length > 1;
        StartCoroutine(Perform(isAllowedToSwitch, UnitState.SwitchWeapon, _weapons[_currentWeaponIndex].data.changeTime, () =>
        {
            SetWeapon((_currentWeaponIndex + 1) % _weapons.Length);
        }));
    }

    private void SetWeapon(int index)
    {
        _currentWeaponIndex = index;

        _animator.SetInteger("Type", (int)_weapons[_currentWeaponIndex].data.type);
        
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].Activity(i == _currentWeaponIndex);
        }
    }

    private void PlayHitAnimation()
    {
        transform.DOKill();
        transform.DOScale(new Vector3(1.05f, .9f, 1.05f), .1f)
            .OnComplete(() => transform.DOScale(new Vector3(1, 1, 1), .1f));

        // Animate the material color to red and back to the original color
        foreach (var renderer in _renderers)
        {
            renderer.material.DOKill();
            renderer.material.DOColor(Color.red, 0.1f) // Turn red
                     .OnComplete(() => renderer.material.DOColor(Color.white, 0.5f)); // Return to original color
        }
    }

    public void TakeDamage(float power)
    {
        _health.TakeDamage(power);
    }

    private void HandleDamageTaken(float currentHealth)
    {
        PlayHitAnimation();
    }

    private void HandleDeath()
    {
        _isAlive = false;

        _animator.Play("Defeat");

        StartCoroutine(Perform(true, UnitState.Idle, 1, () => {
            transform.DOMoveY(-1f, 1f)
            .OnComplete(() => gameObject.SetActive(false));
        }));
    }
}
