using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float _thrust = 4f;
    [SerializeField] private float _knockTime = 0.4f;
    [SerializeField] private float _damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy) && collision.isTrigger)
        {
            MoveEntity(enemy);
            enemy.TakeDamage(_damage);
            StartCoroutine(enemy.KnockCO(_knockTime));
        }
        else if (collision.TryGetComponent(out PlayerController player))
        {
            if (PlayerController.CurrentState != PlayerState.Stagger)
            {
                MoveEntity(player);
                StartCoroutine(player.KnockCO(_knockTime, _damage));
            }
        }
    }

    private void MoveEntity(MonoBehaviour entity)
    {
        var entityRB = entity.gameObject.GetComponent<Rigidbody2D>();

        Vector2 difference = entityRB.transform.position - transform.position;
        difference = difference.normalized * _thrust;
        entityRB.AddForce(difference, ForceMode2D.Impulse);
    }
}