using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

//[Root 노드] => 왜 액션과 다르게 상속 안받음?
//==>특정 AI 동작과 상태에 맞게 유연하게 조정하기 위해서
//대신 BTRoot 객체를 생성하고, 그 아래에 복합 노드와 액션 노드를 추가해서 트리를 구성함
//EnemyAI : AI의 상태와 동작의 구조를 정의하고, 시작함
public class EnemyAI : MonoBehaviour
{
    private BTRoot TreeAIState;
    
    void Awake()
    {
        //게임 오브젝트 활성화 시, 행동 트리 생성
        CreateTreeATState();
    }
    void Update()
    {
        //AI트리의 노드 상태를 매 프레임 마다 얻어옴
        TreeAIState.Tick();
    }

    //행동 트리 실제 생성
    void CreateTreeATState()
    {
        //초기화&루트 노드로 설정
        TreeAIState = new BTRoot();

        //BTSelector와 BTSquence 생성 : 트리 구조 정의
        BTSelector BTMainSelector = new BTSelector();



        //순찰(시퀀스 : 하나라도 실패하면 실패반환)
        //웨이포인트 지정 -> 이동
        BTSquence BTPatrol = new BTSquence();
        //순찰 액션을 Squence의 자식으로 추가
        EnemyState_Patrol_WayPoint state_Patrol_WayPoint = new EnemyState_Patrol_WayPoint(gameObject);
        BTPatrol.AddChild(state_Patrol_WayPoint);
        EnemyState_Patrol_Rotation statePatrol_Rotation = new EnemyState_Patrol_Rotation(gameObject);
        BTPatrol.AddChild(statePatrol_Rotation);



        //추적
        //컨디션 체크(플레이어가 탐색 범위 내 존재) -> 플레이어 방향으로 바라봄 -> 플레이어 추적
        BTSquence BTChase = new BTSquence();
        //추적 액션&컨디션을 Squence의 자식으로 추가
        EnemyState_Chase_ChaseCondition chaseCondition = new EnemyState_Chase_ChaseCondition(gameObject);
        BTChase.AddChild(chaseCondition);
        EnemyState_Chase_LookAt state_Chase_LookAt = new EnemyState_Chase_LookAt(gameObject);
        BTChase.AddChild (state_Chase_LookAt);
        EnemyState_Chase_Chase stateChase_Chase = new EnemyState_Chase_Chase(gameObject);
        BTChase.AddChild(stateChase_Chase);

        //추적 스퀀스 그대로 사용
        //공격 -> 컨디션 체크(플레이어가 공격 범위 내 존재) -> 실제 공격 -> 컨디션 체크2


        //피격(상시 체크)
        //사망(상시 체크)


        //메인 셀렉터 : Squence를 Selector의 자식으로 추가(자식 순서 중요함)
        BTMainSelector.AddChild(BTPatrol);
        BTMainSelector.AddChild(BTChase); // 조건 노드 O      

        //작업이 끝난 Selector를 루트 노드에 붙이기
        TreeAIState.AddChild(BTMainSelector);
    }
}
