using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [Inject(Id = "static log")] private LogEntitySpecs _specs;
        [Inject(Id = "enemyDeathEffect")] private Animator _deathAnimator;

        [SerializeField] private Slider _healthBar;
        [SerializeField] private LootTable _lootTable;

        private float _health;

        private void Start()
        {
            _health = _specs.Hp;
            _healthBar.maxValue = _health;
            _healthBar.value = _health;
            _healthBar.gameObject.SetActive(false);
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            _healthBar.value = _health;
            _healthBar.gameObject.SetActive(true);
            if (_health <= 0)
                Die();
        }

        private void MakeLoot()
        {
            if (_lootTable != null)
            {
                var powerup = _lootTable.LootPowerup();
                if (powerup != null)
                    Instantiate(powerup.gameObject, transform.position, Quaternion.identity);
            }
        }

        private void Die()
        {
            // + sound
            MakeLoot();
            _deathAnimator.gameObject.transform.position = gameObject.transform.position;
            _deathAnimator.SetTrigger("death");
            _healthBar.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}