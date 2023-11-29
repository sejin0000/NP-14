using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SkillCheckMaster : MonoBehaviour
{
    int random;
    float angle;
    bool clickCheck;
    [HideInInspector] public A1205 target;
    private PlayerStatHandler playerStatHandler;
    private int movepower = 200;

    public RectTransform targetTime;
    public RectTransform targetZone;
    bool key;

    public GameObject nonePushBtn;
    public GameObject pushBtn;
    public float controlcheck;
    public float time;
    public bool btnControlBool;

    public float givePower;
    public float oldPower;
    private void OnEnable()
    {
        random = Random.Range(60, 345);
        targetZone.transform.rotation = Quaternion.Euler(0,0, random);
        angle = 0;
        key=true;
        controlcheck = 0.25f;
        time = 0f;
        btnControlBool = false;
        clickCheck = false;
        nonePushBtn.SetActive(true);
        pushBtn.SetActive(false);
    }
    public void Init(A1205 targetObj)
    {
        target = targetObj;
        playerStatHandler = target.gameObject.GetComponent<PlayerStatHandler>();
        oldPower = 0;
    }

    public void OnSkill(InputValue value)
    {
        if (key) 
        {
            nonePushBtn.SetActive(false);
            pushBtn.SetActive(true);
            clickCheck = true;
            if (angle >= random - 30 && angle <= random + 5)
            {
                PowerSet();
                target.endCall(givePower);


            }
            else
            {
                PowerSet();
                givePower = -givePower;
                target.endCall(givePower);
            }
            key = false;
        }

    }
    void Update()
    {
        if (!clickCheck)
        {
            angle += movepower * Time.deltaTime;
            time += Time.deltaTime;
            if (time >= controlcheck) 
            {
                time = 0f;
                BtnControl();
            }
        
            targetTime.transform.rotation = Quaternion.Euler(0, 0, angle);
            if (angle>= 359) 
            {
                clickCheck = true;
                PowerSet();
                givePower = -givePower;
                target.endCall(givePower);
                key = false;
            }

        }
    }
    private void BtnControl()
    {
        nonePushBtn.SetActive(btnControlBool);
        pushBtn.SetActive(!btnControlBool);
        btnControlBool = !btnControlBool;
    }
    private void PowerSet() 
    {
        playerStatHandler.ATK.added -= givePower;
        givePower = playerStatHandler.ATK.total * 0.2f;
    }
}
