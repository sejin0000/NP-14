using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2103 : MonoBehaviourPun
{
    List<GameObject> target = new List<GameObject>();
    PlayerStatHandler me;
    public GameObject Player;
    private TopDownCharacterController controller;

    public float AtkPower;
    public float AtkSpeedPower;
    public float BulletSpreadPower;
    public float SpeedPower;
    public float AtkOldPower;
    public float AtkspeedOldPower;
    public float BulletSpreadOldPower;
    public float SpeedOldPower;

    float setTime;
    int count;

    public void Init()
    {
        me = transform.parent.gameObject.GetComponent<PlayerStatHandler>();
        controller = GetComponent<TopDownCharacterController>();
        AtkPower = 0;
        AtkSpeedPower = 0;
        BulletSpreadPower = 0;
        SpeedPower = 0;
        AtkOldPower = 0;
        AtkspeedOldPower = 0;
        BulletSpreadOldPower = 0;
        SpeedOldPower = 0;
        count = 0;

    }
    private void Update()
    {
        if (photonView.IsMine) 
        {
            setTime += Time.deltaTime;
            if (setTime >= 1f && photonView.IsMine)
            {
                PowerSet();
                setTime = 0f;
            }
        }
    }

    private void PowerSet() 
    {
        if (target.Count>=1) 
        {
            count = target.Count;
            Debug.Log(count);
            me.ATK.added -= AtkOldPower;
            me.AtkSpeed.added -= AtkspeedOldPower;
            me.BulletSpread.added -= BulletSpreadOldPower;
            me.Speed.added -= SpeedOldPower;

            AtkPower = count * 1f;
            AtkSpeedPower = count * 0.1f;
            BulletSpreadPower = count * -0.1f;
            SpeedPower = count * 0.1f;

            me.ATK.added += AtkPower;
            me.AtkSpeed.added += AtkSpeedPower;
            me.BulletSpread.added += BulletSpreadPower;
            me.Speed.added += SpeedPower;

            AtkOldPower = AtkPower;
            AtkspeedOldPower = AtkSpeedPower;
            BulletSpreadOldPower = BulletSpreadPower;
            SpeedOldPower = SpeedPower;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && photonView.IsMine)
        {
            target.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && photonView.IsMine)
        {
            target.Remove(collision.gameObject);
        }
    }
}
