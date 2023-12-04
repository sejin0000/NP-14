using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public virtual void Initialize()
    {
    }

    public virtual void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void Foo()
    {
        Debug.Log("Foo! " + GetType().Name);
    }
}

