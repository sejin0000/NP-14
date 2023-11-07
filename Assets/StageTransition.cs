using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class UIStageTransition : UIBase
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject tower;
    [SerializeField] private int destinationFloor = 5;
    [SerializeField] private float speed=0.1f;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        MainGameManager.Instance.OnUIPlayingStateChanged += StartAnimation;
    }

    public void StartAnimation()
    {
        StartCoroutine(MoveTower());
    }

    IEnumerator MoveTower()
    {
        yield return new WaitForSecondsRealtime(6f);

        animator.SetBool("isRun", true);
        RectTransform towerPos = tower.transform as RectTransform; 
            //tower.transform.GetChild(destinationFloor - 1).transform as RectTransform;

        var destinationY = towerPos.position.y-120*destinationFloor;
        var direction = new Vector3(0, -1, 0);

        var delay = new WaitForFixedUpdate(); //new WaitForSeconds(0.1f);
        while (towerPos.position.y >= destinationY)
        {
            towerPos.Translate(direction*speed);
            yield return delay;
        }
        OnMovedTower();
        yield return new WaitForSecondsRealtime(3f);
        ChangeMainGameState();
    }

    private void OnMovedTower()
    {
        Debug.Log("Done");
        animator.SetBool("isRun", false);
    }

    private void ChangeMainGameState()
    {
        MainGameManager.Instance.GameState = MainGameManager.GameStates.Playing;
    }

    public void SetFloor(int floor)
    {
        destinationFloor = floor;
    }
}
