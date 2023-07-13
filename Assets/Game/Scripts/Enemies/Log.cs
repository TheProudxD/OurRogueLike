﻿using UnityEngine;

public class Log : Enemy
{
    private readonly float _attackRadius = 1.5f;
    private readonly float _chaseRadius = 4;
    private Animator _logAnimator;
    private Transform _target;

    private void Awake()
    {
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _logAnimator = GetComponent<Animator>();
        _target = FindObjectOfType<PlayerController>().transform;
    }

    private void FixedUpdate()
    {
        CheckDistance();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _chaseRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }

    private void CheckDistance()
    {
        var distance = Vector3.Distance(_target.position, transform.position);
        if (distance <= _chaseRadius && distance > _attackRadius)
        {
            if (CurrentState is EnemyState.Idle or EnemyState.Walk and not EnemyState.Idle)
            {
                var targetDirection = Vector3.MoveTowards(
                    transform.position,
                    _target.position,
                    _moveSpeed * Time.deltaTime);
                _enemyRigidbody.MovePosition(targetDirection);

                ChangeAnimation(targetDirection - transform.position);
                _logAnimator.SetBool(WAKEUP_STATE, true);

                ChangeState(EnemyState.Walk);
            }
        }
        else if (distance > _chaseRadius)
        {
            _logAnimator.SetBool(WAKEUP_STATE, false);
        }
    }

    private void SetAnimationFloat(Vector2 direction)
    {
        _logAnimator.SetFloat(XMOVE_STATE, direction.x);
        _logAnimator.SetFloat(YMOVE_STATE, direction.y);
    }

    private void ChangeAnimation(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            SetAnimationFloat(direction.x > 0 ? Vector2.right : Vector2.left);
        else
            SetAnimationFloat(direction.y > 0 ? Vector2.up : Vector2.down);
    }
}