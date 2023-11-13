using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class A0120 : MonoBehaviourPun
{
        private TopDownCharacterController controller;
        private PlayerStatHandler playerStat;
        private void Awake()
        {
            if (photonView.IsMine)
            {
                controller = GetComponent<TopDownCharacterController>();
                playerStat = GetComponent<PlayerStatHandler>();
                controller.OnSkillEvent += aa; // 중요한부분
            }
        }
        // Update is called once per frame
        void aa()
        {
            playerStat.HP.added += 0.01f;
        }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine) 
        {
            if (collision.gameObject.tag == "Enemy") 
            {
                GameObject target = collision.gameObject;
                EnemyAI a = target.GetComponent<EnemyAI>();
            }
        }
    }
}
