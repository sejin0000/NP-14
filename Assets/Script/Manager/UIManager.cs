using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : LocalSingleton<UIManager>
{
    public GameObject overPanel;
    public GameObject clearPanel;


    [SerializeField] private List<UIBase> layer;

    public List<UIBase> Layer
    {
        get { return layer; }
    }

    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Initialize();
    }
    
    // Start is called before the first frame update

    private void Initialize()
    {
        foreach (var layer in layer)
        {
            layer.Initialize();
            layer.Close();
        }
    }

    public void StartIntro()
    {
        Debug.Log("[UIManager] Start Intro");
        //MainGameManager.Instance.GameState = MainGameManager.GameStates.UIPlaying;
    }

    public T GetUIComponent<T>() where T : MonoBehaviour
    {
        Debug.Log("[UIManager] Find Start: " + typeof(T).ToString());
        foreach (var layer in layer)
        {
            if (layer.GetComponent<T>() != null)
            {
                Debug.Log("[UIManager] Find Success: " + typeof(T).ToString());
                return layer.GetComponent<T>();
            }
        }

        Debug.Log("[UIManager] Find Fail: " + typeof(T).ToString());
        return null;
    }

    public GameObject GetUIObject(string name)
    {
        Debug.Log("[UIManager] Find Start: " + name);
        foreach (var layer in layer)
        {
            if (layer.name == name)
            {
                Debug.Log("[UIManager] Find Success: " + name);
                return layer.gameObject;
            }
        }

        Debug.Log("[UIManager] Find Fail. Check parameter. " + name);
        return null;
    }
}
