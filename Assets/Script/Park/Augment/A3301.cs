using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class A3301 : MonoBehaviourPun
{
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
            controller.SkillReset();//�����������
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
            controller.CallEndSkillEvent();
            int PvNum = shieldOBJ.GetPhotonView().ViewID;
            photonView.RPC("FindMaster", RpcTarget.All, PvNum);
            SkillEnd();
        }
    }
    [PunRPC]
    private void FindMaster(int num)
    {
        PhotonView a = PhotonView.Find(num);
        a.transform.SetParent(this.gameObject.transform);
        a.transform.localPosition = Vector3.zero;
        //Prefabs.transform.SetParent(targetPlayer.transform);
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
    public void SkillEnd()
    {
        if (photonView.IsMine)
        {
            //��ų�� ������ ��Ÿ���� ����ϰ� ��Ÿ���� ������  controller.playerStatHandler.CanSkill = ����; �� �ٲ���
            Debug.Log("��ų ����");
            controller.playerStatHandler.useSkill = false;
            if (controller.playerStatHandler.CurSkillStack > 0)
            {
                controller.playerStatHandler.CanSkill = true;
            }
            controller.CallEndSkillEvent();
        }
    }
}
