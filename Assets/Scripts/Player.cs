using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Transform _shootPoint;

    private Weapon _currentWeapon;
    private int _currentWeaponNumber = 0;
    private int _currentHealth;
    private Animator _animator;
    private const string _axe = "Axe";
    private const string _attackAxe = "AttackAxe";
    private const string _ideAxe = "IdeAxe";
    private const string _pistol = "Pistol";
    private const string _attackPistol = "attackPistol";
    private const string _idePistol = "IdePistol";

    public int Money { get; private set; }
    public event UnityAction<int, int> HealthChanged;
    public event UnityAction<int> MoneyChanged;

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        HealthChanged?.Invoke(_currentHealth, _health);

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddMoney(int money)
    {
        Money += money;
        MoneyChanged?.Invoke(Money);
    }

    public void BuyWeapon(Weapon weapon)
    {
        Money -= weapon.Price;
        _weapons.Add(weapon);
        MoneyChanged?.Invoke(Money);
    }

    public void NextWeapon()
    {
        if (_currentWeaponNumber == _weapons.Count - 1)
        {
            _currentWeaponNumber = 0;
        }
        else
        {
            _currentWeaponNumber++;
        }

        ChangeWeapon(_weapons[_currentWeaponNumber]);
    }

    public void PreviousWeapon()
    {
        if (_currentWeaponNumber == 0)
        {
            _currentWeaponNumber = _weapons.Count - 1;
        }
        else
        {
            _currentWeaponNumber--;
        }

        ChangeWeapon(_weapons[_currentWeaponNumber]);
    }

    private void Start()
    {
        ChangeWeapon(_weapons[_currentWeaponNumber]);
        _currentWeapon = _weapons[0];
        _currentHealth = _health;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayAnimation();
            _currentWeapon.Shoot(_shootPoint);
        }
    }

    private void OnEnemyDied(int reward)
    {
        Money += reward;
    }

    private void ChangeWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;

        if (_currentWeapon.tag == _axe)
        {
            _animator.SetTrigger(_ideAxe);
        }
        //if (_currentWeapon.tag == _pistol)
        //{
        //    _animator.Play(_idePistol);
        //}
    }

    private void PlayAnimation()
    {
        if (_currentWeapon.tag == _axe)
        {
            _animator.SetTrigger(_attackAxe);
        }
        //if (_currentWeapon.tag == _pistol)
        //{
        //    _animator.Play(_attackPistol);
        //}
    }
}
