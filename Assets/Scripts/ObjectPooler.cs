using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPooler<T>
{
    private Func<T> _factoryMethod;
    private List<T> _currentStock = new List<T>();
    private Action<T> _turnOnCallback;
    private Action<T> _turnOffCallback;

    public ObjectPooler(Func<T> factorymethod, Action<T> turnOn, Action<T> turnOff, int initialStock)
    {
        _factoryMethod = factorymethod;
        _turnOnCallback = turnOn;
        _turnOffCallback = turnOff;

        for (int i = 0; i < initialStock; i++)
        {
            var obj = _factoryMethod();
            _turnOffCallback(obj);
            _currentStock.Add(obj);
        }
    }

    public T GetObject()
    {
        var result = default(T);
        if (_currentStock.Count > 0)
        {
            result = _currentStock[0];
            _currentStock.RemoveAt(0);
        }
        else
        {
            result = _factoryMethod();
        }

        _turnOnCallback(result);
        return result;
    }

    public void ReturnObject(T obj)
    {
        _turnOffCallback(obj);
        _currentStock.Add(obj);
    }

}
