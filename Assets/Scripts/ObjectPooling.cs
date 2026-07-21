using System.Collections.Generic;
using UnityEngine;

namespace IceColdBeer.Core
{
    public class ObjectPooling<T> where T : MonoBehaviour
    {
        private T _prefab;
        private List<T> _objects;

        public ObjectPooling(T prefab, int size = 10)
        {
            _prefab = prefab;
            _objects = new List<T>();

            for(int i = 0; i < size; i++)
            {
                var obj = GameObject.Instantiate(_prefab);
                obj.gameObject.SetActive(false);
                _objects.Add(obj);
            }
        }

        public T Get()
        {
            for(int i = 0; i < _objects.Count; i++)
            {
                if(!_objects[i].gameObject.activeInHierarchy)
                {
                    _objects[i].gameObject.SetActive(true);
                    return _objects[i];
                }
            }

            var oldestObj = _objects[0];
            _objects.RemoveAt(0);
            _objects.Add(oldestObj);

            oldestObj.gameObject.SetActive(false);
            oldestObj.gameObject.SetActive(true);
            return oldestObj;
        }

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}