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
        MainGameManager.Instance.OnUIPlayingStateChanged += ActiveIntroUI;
    }

    private void Initialize()
    {
        GameObject obj = GameObject.Find("UI_Root");
        foreach (var layer in layer)
        {
            layer.Close();
        }
    }

    private void ActiveIntroUI()
    {
        layer.First().Open();
        layer.First().gameObject.GetComponent<UIStageTransition>().StartAnimation();
    }



}
