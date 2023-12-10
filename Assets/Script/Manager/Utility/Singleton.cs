using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj;
                obj = GameObject.Find(typeof(T).Name);
                if (obj==null)
                {
                    obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
                else
                {
                    Debug.Log("[Singleton<T>] Already created " + typeof(T).Name);
                }
            }
            return instance;
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public virtual void Initialize()
    {

    }
}
