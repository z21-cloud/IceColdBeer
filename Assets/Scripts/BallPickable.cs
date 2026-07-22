using IceColdBeer.Core;
using Unity.VisualScripting;
using UnityEngine;

public class BallPickable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<IPickable>(out var pickable))
        {
            pickable.PickUp();
        }
    }
}
