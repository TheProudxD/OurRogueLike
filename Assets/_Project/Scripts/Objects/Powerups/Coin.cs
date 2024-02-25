using Player;
using UnityEngine;

namespace Objects.Powerups
{
    public class Coin : Powerup
    {
        //private readonly int _amountToIncrease = 2;

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerAttacking player) && !other.isTrigger)
            {
                // TODO: zenject doesn't bind Inventory after instantiate!
                
                //Inventory.NumberOfCoins+=_amountToIncrease;
                //print(Inventory.NumberOfCoins);
                //Destroy(gameObject);
            }
        }
    }
}