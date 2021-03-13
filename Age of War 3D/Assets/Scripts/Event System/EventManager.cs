using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.ExecuteEvents;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private List<GameObject> _listeners;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (_listeners == null)
            {
                _listeners = new List<GameObject>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddListener(GameObject listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void RemoveListener(GameObject listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }

    public void ExecuteEvent<T>(EventFunction<T> functor) where T : IEventSystemHandler
    {
        foreach (GameObject gameObject in _listeners)
        {
            Execute(gameObject, null, functor);
        }
    }
}