using System.Collections.Generic;
using UnityEngine;

namespace Sources.Runtime.Core.ObjectPool
{
    public class ObjectPool<T> where T : Component
    {
        private readonly List<T> _pool = new List<T>();

        public IEnumerable<T> Pool => _pool;

        public T CreateInstance(T prefab, Transform parent = null, bool activeState = false)
        {
            T instance = Object.Instantiate(prefab, parent);

            instance.gameObject.SetActive(activeState);

            _pool.Add(instance);

            return instance;
        }
        public T Get()
        {
            foreach (var item in _pool)
            {
                if (item.gameObject.activeInHierarchy == false)
                {
                    item.gameObject.SetActive(true);

                    return item;
                }
            }

            return null;
        }
    }
}
