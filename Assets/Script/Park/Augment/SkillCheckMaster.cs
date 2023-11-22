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
    private int movepower = -120;

    public RectTransform targetTime;
    public RectTransform targetZone;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        random = Random.Range(45, 355);//90~ -270
        targetZone.transform.rotation = Quaternion.Euler(0,0, random);
        angle = 0;
        clickCheck = false;
    }
    public void Init(A1205 targetObj)
    {
        target = targetObj;
    }

    public void OnSkill(InputValue value) //90~ -270
    {
        clickCheck=true;
        if (angle >= random - 20 && angle <= random + 20)
        {
            target.endCall(5f);
        }
        else
        {
            target.endCall(-5f);
        }
    }
    // Update is called once per frame
    void Update()//시간가고 !!! 그뭐냐 돌아가고!!!! 빨간원 하고 !!!90~ -270
    {
        if (!clickCheck)
        {
            angle += movepower * Time.deltaTime;
            targetTime.transform.rotation = Quaternion.Euler(0, 0, angle);
            if (angle >= 359) //angle <= 270
            {
            target.endCall(-5f);
            }
        }
    }
}
