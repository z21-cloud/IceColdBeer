using UnityEngine;

namespace IceColdBeer.Factories
{
    public interface IFactory<T>
    {
        T Create();
    }
}
