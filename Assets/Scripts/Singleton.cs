using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly Lazy<T> lazy = new Lazy<T>(CreateInstance);
    public static T Instance
    {
        get
        {
            return lazy.Value;
        }
    }

    private static T CreateInstance()
    {
        if (Application.isPlaying)
        {
            T[] instances = FindObjectsOfType<T>();
            if (instances.Length > 0)
            {
                if (instances.Length > 1)
                {
                    Debug.LogWarning($"Warning! More than one instance of {typeof(T)} found!");
                }
                DontDestroyOnLoad(instances[0]);
                return instances[0];
            }
            else
            {
                // found no instances
                GameObject singletonObject = new GameObject();
                T instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T) + " (Singleton)";
                DontDestroyOnLoad(singletonObject);

                return instance;
            }
        }
        else
        {
            return null;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
