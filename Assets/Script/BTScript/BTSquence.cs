using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//복합(Composite) 노드 중 하나인 Squence를 정의하는 스크립트
//BTSquence는 자식 노드를 순차적으로 실행하며
//자식 노드 중 하나라도 실패하면 실패하고, 모든 자식 노드가 성공해야 성공하는 노드임
namespace myBehaviourTree
{
    public class BTSquence : BTComposite
    {
        //노드 유형 설정(모든 노드 다 함)
        public BTSquence()
        {
            SetNodeType(NodeType.Sequence);
        }

        //BTSquence 노드의 주요 동작 정의 : 자식 노드 중 하나라도 실패하면 실패하고, 모든 자식 노드가 성공해야 성공
        public override Status Update()
        {
            //현재 BTSquence 상태 저장(우선 초기화)
            Status currenStatus = Status.BT_Invalid;

            //자식 노드 순회
            for (int i = 0; i < GetChildCount(); i++)
            {
                //각 반복의 자식 노드 상태를 가져와 저장
                currenStatus = GetChild(i).GetStatus();

                //자식 노드 타입이 액션이 아니거나, 노드 Status가 성공 상태가 아닌 경우
                //이미 성공한 자식 노드는 다시 업데이트 안해도 괜찮으므로 이와같이 제한한다.
                //★액션 노드를 제한하는 이유는 액션 노드는 한번 실행되면 성공 혹은 실패로 고정되므로
                //별도의 상태 업데이트가 필요없기 때문이다.
                if (GetChild(i).GetNodeType() != NodeType.Action || GetChild(i).GetStatus() != Status.BT_Success)
                {
                    //자식 노드 업데이트                    
                    currenStatus = GetChild(i).Tick();
                }                   

                //업데이트 후 상태 확인

                //성공 상태가 아니라면
                if (currenStatus != Status.BT_Success)
                {
                    //실패 반환(하나라도 실패하면 BTSquence는 실패를 반환함)
                    return currenStatus;
                }                   
            }

            //위의 조건에 걸려 반환하지 않았다면 모두 성공했으므로
            //BTSquence 역시 성공을 반환하게 됨
            return Status.BT_Success;
        }
    }
}

