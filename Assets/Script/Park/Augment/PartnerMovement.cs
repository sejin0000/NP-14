using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerMovement : MonoBehaviour
{
    private PlayerStatHandler playerStat;
    public void Awake()
    {
        playerStat = this.transform.parent.gameObject.GetComponent<PlayerStatHandler>();
    }
    public void Update()
    {
        this.transform.localPosition = new Vector3 (0.75f, 0.5f, 0f);
        if (playerStat.isDie)
        {
            this.GetComponent<PlayerStatHandler>().Damage(playerStat.HP.total);
        }
    }
}
