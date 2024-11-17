using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private T _prefab;
    private Queue<T> _objects;

    public Pool(T prafab, int countObject)
    {
        _prefab = prafab;
        _objects = new Queue<T>();

        for (int i = 0; i < countObject; i++)
        {
            T obj = GameObject.Instantiate(_prefab);
            obj.gameObject.SetActive(false);
            _objects.Enqueue(obj);
        }
    }

    public T GetObject()
    {
        if (_objects.TryDequeue(out T obj) == false)
        {
            obj = GameObject.Instantiate(_prefab);
        }

        obj.gameObject.SetActive(true);

        return obj;
    }

    public void ReleaseObject(T obj)
    {
        obj.gameObject.SetActive(false);
        _objects.Enqueue(obj);
    }
}