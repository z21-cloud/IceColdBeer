using UnityEngine;

namespace IceColdBeer.Core
{
    public class LoseHole : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other) 
        {
            if(other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage();
            }
        }
    }
}
