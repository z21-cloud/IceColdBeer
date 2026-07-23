using System.Collections.Generic;
using IceColdBeer.Factories;
using UnityEngine;

namespace IceColdBeer.Pools
{
    public class ObjectPooling<T> where T : MonoBehaviour
    {
        private IFactory<T> _factory;
        private Queue<T> _objects;

        public ObjectPooling(IFactory<T> factory, int size = 10)
        {
            _factory = factory;
            _objects = new Queue<T>(size);  

            for(int i = 0; i < size; i++)
            {
                CreateNewObject();
            }
        }

        private T CreateNewObject()
        {
            var obj = _factory.Create();
            obj.gameObject.SetActive(false);
            _objects.Enqueue(obj);
            return obj;
        }

        public T Get()
        {
            if(_objects.Count > 0)
            {
                var obj = _objects.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            Debug.LogWarning($"[ObjectPooling] Pool is empty!");
            return null;
        }

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
            _objects.Enqueue(obj);
        }
    }
}