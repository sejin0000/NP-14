using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

//[Action 노드]
//BTAction 상속받는 이유?
//==>실제 게임 오브젝트의 행동을 정의하기 때문.
//override Status Update 메서드를 구현하여 실제 행동을 정의함
public class EnemyState_Patrol_Rotation : BTAction
{
    private GameObject owner;

    private float intervalTime = 1.0f;
    private Vector3 direction;

    public EnemyState_Patrol_Rotation(GameObject currentOwner)
    {
        owner = currentOwner;
    }

    //행동 노드가 처음 실행 될 때 : 초기화 작업 (노드의 Start())
    public override void Initialize()
    {
        Debug.Log("실제로 움직임");
        //무작위 방향설정        
        SetStateColor();
        InitDirection();
    }


    //종료는 그대로
    public override void Terminate()
    {
        base.Terminate();
    }

    //실제 업데이트 할 상태
    public override Status Update()
    {
        Debug.Log("업데이드 됨");
        //실제 동작
        OnRotationByDir();
        //상태 반환
        return Status.BT_Running;
    }

    //상태에 따라 스프라이트 색상을 변경(그냥 임시 : 행동이 시각적으로 잘 보이게)
    private void SetStateColor()
    {
        owner.GetComponent<SpriteRenderer>().color = Color.green;
    }

    //방향을 무작위로 설정&정규화 (벡터를)
    private void InitDirection()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);

        direction = new Vector3(x, y, 0);
        direction.Normalize();
    }

    //intervalTime을 사용하여 일정 간격으로 방향을 번경&회전
    //새로운 방향도 무작위로 선택 
    //Quaternion.Slerp을 사용하여 부드럽게 회전
    public void OnRotationByDir()
    {
        intervalTime += Time.deltaTime;

        if(intervalTime < 0.0f)
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);

            //예외처리(랜덤 목표 지점이 0,0으로 설정됨)
            if(x == 0.0f && y == 0.0f)
            {
                x = 1.0f;
            }

            direction = new Vector3(x, y, 0);

            //interval 재설정
            intervalTime = Random.Range(0.5f, 3.0f);
        }

        //방향 전환 시 부드럽게 회전
        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);
    }
}