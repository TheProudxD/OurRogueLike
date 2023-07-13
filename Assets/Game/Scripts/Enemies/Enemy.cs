using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Walk,
    Attack,
    Stagger
}

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    protected const string XMOVE_STATE = "moveX";
    protected const string YMOVE_STATE = "moveY";
    protected const string WAKEUP_STATE = "wakeUp";
    
    public EnemyState CurrentState;
    
    [SerializeField] protected float _health;
    [SerializeField] protected FloatValue _maxHealth;
    [SerializeField] protected string _enemyName;
    [SerializeField] protected int _baseAttack;
    [SerializeField] protected float _moveSpeed;
    
    protected Rigidbody2D _enemyRigidbody;

    protected void Start()
    {
        _health = _maxHealth.RuntimeValue;
        CurrentState = EnemyState.Idle;
    }

    public IEnumerator KnockCO(float knockTime)
    {
        CurrentState = EnemyState.Stagger;
        yield return new WaitForSeconds(knockTime);
        _enemyRigidbody.velocity = Vector2.zero;
        CurrentState = EnemyState.Idle;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0) gameObject.SetActive(false);
    }
    
    protected void ChangeState(EnemyState newState)
    {
        CurrentState = newState;
    }
}