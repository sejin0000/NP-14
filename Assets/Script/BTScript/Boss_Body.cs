using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Body : MonoBehaviour
{
    private BossAI_Dragon owner;
    private void Awake()
    {
        owner = GetComponentInParent<BossAI_Dragon>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ȣ��Ʈ������ �浹 ó����
        if (!PhotonNetwork.IsMasterClient)
            return;

        Bullet playerBullet = collision.gameObject.GetComponent<Bullet>();


        if (collision.gameObject.tag == "Bullet" && playerBullet.targets.ContainsValue((int)BulletTarget.Enemy) && playerBullet.IsDamage)
        {
            float atk = collision.transform.GetComponent<Bullet>().ATK;
            //isChase = true;
            int ViewID = playerBullet.BulletOwner;
            //Debug.Log($"����̵� : {ViewID}");
            PhotonView PlayerPv = PhotonView.Find(ViewID);
            PlayerStatHandler player = PlayerPv.gameObject.GetComponent<PlayerStatHandler>();
            player.EnemyHitCall();


            if (playerBullet.fire)
            {
                Debuff.Instance.GiveFire(this.gameObject, atk, ViewID);
            }
            if (playerBullet.water)
            {
                Debuff.Instance.GiveWater(this.gameObject);
            }
            if (playerBullet.ice)
            {
                int random = UnityEngine.Random.Range(0, 100);
                if (random < 90)
                {
                    //isGroggy = true;
                    Debug.Log("����üũ");
                    Debuff.Instance.GiveIce(this.gameObject);
                }
            }
            if (playerBullet.burn)
            {
                GameObject firezone = PhotonNetwork.Instantiate("AugmentList/A0122", transform.localPosition, Quaternion.identity);
                firezone.GetComponent<A0122_1>().Init(playerBullet.BulletOwner, atk);
            }
            if (playerBullet.gravity)
            {
                int a = UnityEngine.Random.Range(0, 10);
                if (a >= 8)
                {
                    PhotonNetwork.Instantiate("AugmentList/A0218", transform.localPosition, Quaternion.identity);
                }
            }
            //��� �÷��̾�� ���� ���� ü�� ����ȭ
            owner.PV.RPC("DecreaseHP", RpcTarget.All, atk);



            //����� �ҷ� ��ò��� ���
            owner.lastAttackPlayer = playerBullet.BulletOwner;

            // ��ID�� ����Ͽ� ���� �÷��̾� ã��&�ش� �÷��̾�� Ÿ�� ����
            PhotonView photonView = PhotonView.Find(playerBullet.BulletOwner);
            if (photonView != null)
            {
                Transform playerTransform = photonView.transform;

                owner.currentTarget = playerTransform;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO�˹� �����ؼ� ���� ���� �� ����
        if ((collision.gameObject.tag != "player")
            && collision.gameObject.GetComponent<A0126>() == null
            && collision.gameObject.GetComponent<A3104>() == null)
            return;

        // ��� ����
        float damageCoeff = 0;

        if (collision.gameObject.GetComponent<A0126>() != null)
        {
            damageCoeff += collision.gameObject.GetComponent<A0126>().DamageCoeff;
            int viewID = collision.gameObject.GetPhotonView().ViewID;
            owner.PV.RPC("DecreaseHPByObject", RpcTarget.All, collision.transform.GetComponent<PlayerStatHandler>().HP.total * damageCoeff, viewID);
        }
        if (collision.gameObject.GetComponent<A3104>().isRoll)
        {
            damageCoeff += collision.gameObject.GetComponent<A3104>().DamageCoeff;
            int viewID = collision.gameObject.GetPhotonView().ViewID;
            owner.PV.RPC("DecreaseHPByObject", RpcTarget.All, collision.transform.GetComponent<PlayerStatHandler>().HP.total * damageCoeff, viewID);
        }
    }
}
