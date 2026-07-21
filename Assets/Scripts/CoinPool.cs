using IceColdBeer.Core;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    [SerializeField] private Coin coin;
    [SerializeField] private int poolSize;
    private ObjectPooling<Coin> pool;
    private void Awake() 
    {
        pool = new ObjectPooling<Coin>(coin, poolSize);
    }
}
