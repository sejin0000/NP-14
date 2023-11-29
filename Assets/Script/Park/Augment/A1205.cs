using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class A1205 : MonoBehaviourPun//스킬 사용시 스킬체크를 하여 성공시 데미지/힐량을 증가 시키지만 실패시 데미지/힐량을 감소 시킵니다.
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private PlayerInput playerInput;

    private float tempPower;
    public float power;
    public GameObject skillCheckPrefab;
    public SkillCheckMaster skillCheckobj;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            playerInput = GetComponent<PlayerInput>();
            tempPower = 0;
            controller.OnSkillEvent += SkillCheck; // 중요한부분

            GameObject A0125Prefabs = Resources.Load<GameObject>("AugmentList/A1205SkillCheckMaster");
            A0125Prefabs.transform.SetSiblingIndex(0);
            skillCheckPrefab = Instantiate(A0125Prefabs);

            skillCheckobj = skillCheckPrefab.GetComponent<SkillCheckMaster>();


            skillCheckobj.Init(this);
            skillCheckPrefab.SetActive(false);
        }
    }
    // Update is called once per frame
    void SkillCheck()
    {
        Debug.Log("스킬체킁ㅇ");
        playerInput.actions.FindAction("Attack").Disable();
        skillCheckPrefab.SetActive(true);
    }
    public void endCall(float power) 
    {
        playerInput.actions.FindAction("Attack").Enable();
        playerStat.ATK.coefficient -= tempPower;
        playerStat.ATK.coefficient += power;
        tempPower = power;
        Invoke("objActiveControl", 0.5f);
    }
    void objActiveControl() 
    {
        skillCheckPrefab.SetActive(false);
    }
}
