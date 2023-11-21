using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : LocalSingleton<UIManager>
{
    [SerializeField] private List<UIBase> layer;

    public List<UIBase> Layer
    {
        get { return layer; }
    }

    private void Awake()
    {
        base.Awake();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

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
}
