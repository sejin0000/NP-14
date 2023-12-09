using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class UIStageTransition : UIBase
{
    [Header("GameObject")]
    [SerializeField] private GameObject blockParents;
    [SerializeField] private GameObject[] blockPrefab;
    [SerializeField] private GameObject player;

    [Header("ETC")]
    [SerializeField] private Animator animator;
    [SerializeField] private float speed=0.1f;
    [SerializeField] private int maxFloor;

    private int currentFloor = -1;
    private GameObject[] block;

    private float spriteHeight;

    public override void Initialize()
    {
        Debug.Log("[UIStageTransition] initialized");
        CreateTower();

        GameManager.Instance.OnStageSettingEvent += StartTransition;
        GameManager.Instance.OnBossStageSettingEvent += StartTransition;
    }

    // Tower 층수만큼 일정한 간격으로 블럭 생성
    public void CreateTower()
    {
        spriteHeight = blockPrefab[0].GetComponent<RectTransform>().rect.height;

        block = new GameObject[maxFloor];
        for(int i=0; i<maxFloor;++i)
        {
            GameObject temp;
            if (i == maxFloor - 1) //Block_Head
                temp = Instantiate(blockPrefab[1], blockParents.transform);
            else //Block_Body
                temp = Instantiate(blockPrefab[0], blockParents.transform);

            block[i] = temp;
            
            if (i > 0)
            {
                Vector3 pos = block[i - 1].GetComponent<RectTransform>().anchoredPosition;
                pos.y += spriteHeight;
                block[i].GetComponent<RectTransform>().anchoredPosition = pos;
            }
        }
    }

    // 생성한 블럭 위치 기준으로 플레이어 오브젝트 배치 및 현재 층수 1층 설정
    public void SetupPlayer()
    {
        currentFloor = GameManager.Instance.curStage;

        Vector3 pos = block[0].transform.position;
        //pos.y -= (spriteHeight / 2);
        player.transform.position = pos;

        //player.GetComponent<SpriteResolver>().SetCategoryAndLabel("idle", "1");
    }

    public void StartTransition()
    {
        Open();
        StartCoroutine(ClimbTower());
    }

    IEnumerator ClimbTower()
    {
        yield return new WaitForSecondsRealtime(1f);
        //player.GetComponent<RectTransform>().rect.height/4);
        while (player.transform.position.y <= (block[currentFloor + 1].transform.position.y))
        {
            //player.GetComponent<SpriteResolver>().SetCategoryAndLabel("run", "1");
            animator.SetBool("isRun", true);

            blockParents.transform.Translate(new Vector3(0, -1, 0)*Time.deltaTime*speed);
            yield return new WaitForEndOfFrame();
        }

        animator.SetBool("isRun", false);
        yield return new WaitForSecondsRealtime(3f);
        OnClimeTower();
        Debug.Log("UIStageTransition[ClimbTower] : isTransitonPlayer = true");
        GameManager.Instance.isTransitionPlayed = true;
    }

    // UI 연출이 끝나면 메인 게임 매니저의 상태 변경
    private void OnClimeTower()
    {
        StopCoroutine(ClimbTower());
        currentFloor += 1;

        //MainGameManager.Instance.GameState = MainGameManager.GameStates.Start;
        Close();
        UIManager.Instance.Open<UIMainGame>();
    }

    private void Update()
    {
        player.GetComponent<Image>().sprite = player.GetComponent<SpriteRenderer>().sprite;
        UIManager.Instance.Close<UIMainGame>();
    }
}
