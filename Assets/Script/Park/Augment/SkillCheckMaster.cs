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
    public int movepower = 200;

    public RectTransform targetTime;
    public RectTransform targetZone;
    bool key;

    private void OnEnable()
    {
        random = Random.Range(45, 345);
        targetZone.transform.rotation = Quaternion.Euler(0,0, random);
        angle = 0;
        key=true;
        clickCheck = false;
    }
    public void Init(A1205 targetObj)
    {
        target = targetObj;
    }

    public void OnSkill(InputValue value)
    {
        if (key) 
        {
            clickCheck = true;
            if (angle >= random - 20 && angle <= random + 20)
            {
                target.endCall(5f);
            }
            else
            {
                target.endCall(-5f);
            }
            key = false;
        }

    }
    void Update()
    {
        if (!clickCheck)
        {
            angle += movepower * Time.deltaTime;
            targetTime.transform.rotation = Quaternion.Euler(0, 0, angle);
            if (angle >= random - 20 && angle <= random + 20)
            {
                Debug.Log("지금이야");
            }
            if (angle>= 359) 
            {
                clickCheck = true;
                target.endCall(-5f);
                key = false;
            }

        }
    }
}
