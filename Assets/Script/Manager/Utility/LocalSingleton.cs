using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class LocalSingleton<T> : MonoBehaviour where T : MonoBehaviour
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
                }
                else
                {
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    public void Awake()
    {

    }
}
