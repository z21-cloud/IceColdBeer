using UnityEngine;

namespace IceColdBeer.Core
{
    public class BallDamageable : MonoBehaviour, IDamageable
    {
        public void TakeDamage()
        {
            Debug.Log("Player Lose");
        }
    }
}
