using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StoneWall : MonoBehaviour
{
    //������ ���, ���� �޴°� ȣ��Ʈ��, �ı��� ����
    private float wallHP = 100f;
    private float currentWallHP;

    private void Awake()
    {
        currentWallHP = wallHP;
    }

    [PunRPC]
    public void DecreaseWallHP(float damage)
    {
        currentWallHP -= damage;
        if (currentWallHP <= 0)
        {
            DestroyWall();
        }
    }

    [PunRPC]
    public void DestroyWall()
    {
        Destroy(gameObject);
    }

}
