using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class A1207 : MonoBehaviourPun
{
    private CollisionController collision;
    private PlayerStatHandler playerStatHandler;
    private Transform target;
    Vector2 position;

    private void Awake()
    // 포지션업데이트로 다른플레이어값 받아서
    //돌림 시작하자마자 플레이어 목숨 -1 그냥 죽은취급
    {
        if (photonView.IsMine)
        {
            collision= GetComponent<CollisionController>();
            playerStatHandler=GetComponent<PlayerStatHandler>();

            collision.footCollider.isTrigger = true;
            collision.rigidbody.bodyType = RigidbodyType2D.Kinematic;

            MainGameManager.Instance.OnGameStartedEvent += ImDie;
            //
            position = Vector2.zero;
            //target ??

        }
    }
    private void Update()
    {
        if (photonView.IsMine) 
        {
            transform.position = new Vector2(target.position.x + 0.2f, target.position.y + 0.2f);
        }

    }
    // Update is called once per frame
    void ImDie()
    {
        //MainGameManager.Instance.AddPartyDeathCount();
        MainGameManager.Instance.photonView.RPC("AddPartyDeathCount", RpcTarget.All);
        playerStatHandler.isDie = true;
        this.gameObject.layer = 13; //고스트플레이어레이어
    }
}
