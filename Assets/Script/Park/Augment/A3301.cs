using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3301 : Skill
{
    public float shieldSurvivalTime = 0.5f;
    private GameObject shieldOBJ;

    public override void SkillStart()
    {
        shieldOBJ = PhotonNetwork.Instantiate("AugmentList/A3301", transform.position, Quaternion.identity);
        A3301_1 shield = shieldOBJ.GetComponent<A3301_1>();
        shieldOBJ.transform.SetParent(gameObject.transform);
        Invoke("SkillEnd", shieldSurvivalTime+0.5f);
        base.SkillStart();
    }
    public override void SkillEnd()
    {
        base.SkillEnd();
    }
}
