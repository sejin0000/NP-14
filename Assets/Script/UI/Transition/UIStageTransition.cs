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
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject player;

    [Header("ETC")]
    [SerializeField] private Animator animator;
    [SerializeField] private float speed=0.1f;
    [SerializeField] private int maxFloor;

    private int currentFloor;
    private GameObject[] block;

    [SerializeField] private float spriteHeight = 100f;
    [SerializeField] private float spriteSpace = 50f;

    public override void Initialize()
    {
        Debug.Log("[UIStageTransition] initialized");

        CreateTower();
        SetupPlayer();

        MainGameManager.Instance.OnUIPlayingStateChanged += StartTransition;
    }

    // Tower ������ŭ ������ �������� ���� ����
    public void CreateTower()
    {
        block = new GameObject[maxFloor];
        for(int i=0; i<maxFloor;++i)
        {
            GameObject temp = Instantiate(blockPrefab, blockParents.transform);
            block[i] = temp;
            block[i].transform.SetParent(blockParents.transform);

            if (i > 0)
            {
                Vector3 pos = block[i - 1].transform.position;
                pos.y += (spriteHeight + spriteSpace);
                block[i].transform.position = pos;
            }
        }
    }

    // ������ ���� ��ġ �������� �÷��̾� ������Ʈ ��ġ �� ���� ���� 1�� ����
    public void SetupPlayer()
    {
        GameObject playerObj;
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            playerObj = TestGameManagerDohyun.Instance.InstantiatedPlayer;
        else
            playerObj = MainGameManager.Instance.InstantiatedPlayer;

        //player.GetComponent<SpriteResolver>().SetCategoryAndLabel("idle", "1");
        player.GetComponent<Image>().sprite = playerObj.transform.Find("MainSprite").GetComponent<SpriteRenderer>().sprite;

        Vector3 pos = block[0].transform.position;
        pos.y += spriteHeight;
        player.transform.position = pos;

        currentFloor = 0;
    }

    public void StartTransition()
    {
        Open();
        StartCoroutine(ClimeTower());
    }

    IEnumerator ClimeTower()
    {
        yield return new WaitForSecondsRealtime(1f);

        while (player.transform.position.y <= (block[currentFloor + 1].transform.position.y+spriteHeight/2))
        {
            animator.SetBool("isRun", true);

            blockParents.transform.Translate(new Vector3(0, -1, 0)*Time.deltaTime*speed);
            yield return new WaitForEndOfFrame();
        }

        animator.SetBool("isRun", false);
        yield return new WaitForSecondsRealtime(3f);
        ChangeMainGameState();
    }

    // UI ������ ������ ���� ���� �Ŵ����� ���� ����
    private void ChangeMainGameState()
    {
        StopCoroutine(ClimeTower());
        currentFloor += 1;

        MainGameManager.Instance.GameState = MainGameManager.GameStates.Start;
        Close();
    }

    public override void Open()
    {
        base.Open();
        this.gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        this.gameObject.SetActive(false);
    }
}