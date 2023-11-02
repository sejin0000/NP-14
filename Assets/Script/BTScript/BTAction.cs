using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BT의 가장 하위 노드 중 하나 => 실제 게임 엔진에 연결된 액션을 수행
namespace myBehaviourTree
{
    public class BTAction : BTBehaviour
    {
        //--
        //Left Node
        //Actor의 상태를 처리하는 클래스
        //★해당 Action Node가 EBH_Success 상태고, 재방문 시 skip
        //--

        public BTAction()
        {
            SetNodeType(NodeType.Action);
        }

        //디폴트 상태 : 비워짐 => 필요한 경우에 ovrerride해서 사용

        //행동 노드 시작 시 작동(추가적인 작업이 필요한 경우 추가)
        public override void Initialize()
        {

        }
        //행동 노드 종료 시 작동(추가적인 작업이 필요한 경우 추가)
        public override void Terminate()
        {

        }

        //행동 노드의 상태를 초기화
        //행동 노드 재사용 시 이전 상태를 지워줄 때 호출
        //현재는 상태 초기화만 해놨음
        public override void Reset()
        {
            SetStatus(Status.BT_Invalid);
        }

        //행동 노드를 실행하고 상태를 반환함
        public override Status Tick()
        {
            //상태가 기본인 경우
            if (GetStatus() == Status.BT_Invalid)
            {
                //초기화 작업 수행
                Initialize();
                SetStatus(Status.BT_Running);
            }

            //실제 행동을 수행, 상태 설정
            SetStatus(Update());

            //작동중이 아니라면
            if (GetStatus() != Status.BT_Running)
            {
                //초기화
                Terminate();
            }

            //최종적으로 나온 노드 상태 반환
            return GetStatus();
        }
    }

}