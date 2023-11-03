using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//복합(Composite) 노드 중 하나인 Selector를 정의하는 스크립트
//BTSelector는 자식 노드를 순차적으로 실행하며
//'첫 번째'로 성공한 자식 노드를 찾으면 '나머지 자식 노드를 실행하지 않음'
namespace myBehaviourTree
{
    //복합 노드에 속하므로 Composite를 상속한다.(자식 노드 관리 & 복합 노드 기본 동작)
    public class BTSelector : BTComposite
{
        //노드 유형 설정(모든 노드 다 함)
        public BTSelector()
        {
            SetNodeType(NodeType.Selector);
        }

        //BTSelector의 주요 동작 정의 : 첫 성공 자식을 제외한 나머지를 실행하지 않음
        public override Status Update()
        {
            //자식 노드를 순차적으로 실행
            //전위 연산자
            for (int i = 0; i < GetChildCount(); i++)
            {
                //해당 자식노드의 현재 작동 상태를 반환
                Status currentStatus = GetChild(i).Tick();

                //실행 실패 상태가 아닌경우 => 성공 상태인 경우
                //성공 상태가 아닌 경우도 포함하게 됨 왜???
                //=> 실행 중이거나 중단 상태일 때, 노드는 여전히 진행중이며 미래에 성공할 수 있다고 가정
                //행동 트리에서 진행중이거나 중단된 상태를 목표로 정할 수 있게 됨
                if(currentStatus != Status.BT_Failure)
                {
                    //i(성공 자식 노드) 제외한 모든 자식노드 초기화
                    ClearChild(i);
                    //해당 자식 노드의 상태(Status)를 반환
                    return currentStatus;
                }
            }

            //모든 자식 노드 실패or자식 노드 없음
            return Status.BT_Failure;
        }

        //파라미터 index를 제외한 모든 자식노드를 재설정(초기화, Reset)
        protected void ClearChild(int skipIndex)
        {
            //자식 노드 수만큼 반복
            for (int i = 0; i < GetChildCount(); ++i)
            {
                //성공 자식은 제외시킴
                if(i != skipIndex)
                {
                    //각 노드 리셋
                    GetChild(i).Reset();
                }
            }
        }
    }
}
