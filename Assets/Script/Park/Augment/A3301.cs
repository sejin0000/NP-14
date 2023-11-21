using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class A3301 : MonoBehaviourPun
{
    public float shieldSurvivalTime = 0.5f;
    private GameObject shieldOBJ;
    private TopDownCharacterController controller;
    bool isLink;
    int viewid;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            controller.OnSkillEvent += SkillStart;
            controller.SkillReset();//여기부터참고
            controller.SkillMinusEvent += SkillLinkOff;
            viewid=gameObject.GetPhotonView().ViewID;
            isLink = true;
        }
    }
    public void SkillStart()
    {
        if (photonView.IsMine) 
        {
            shieldOBJ = PhotonNetwork.Instantiate("AugmentList/A3301", transform.position, Quaternion.identity);
            shieldOBJ.transform.SetParent(gameObject.transform);
            shieldOBJ.GetComponent<A3301_1>().Init(viewid);
            Invoke("SkillEnd", shieldSurvivalTime + 0.5f);
            controller.CallEndSkillEvent();
        }
    }
    public void SkillLinkOff()
    {
        if (photonView.IsMine)
        {
            if (isLink)
            {
                controller.OnSkillEvent -= SkillStart;
                isLink = false;
            }
        }
    }
}
