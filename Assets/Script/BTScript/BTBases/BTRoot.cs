using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myBehaviourTree //여러개의 스크립트를 하나로 묶을 수 있네?
{

    //루트 노드를 나타내는 스크립트[루트 노드 역할 수행&초기화]

    //루트 노드의 역할 : 다른 행동 트리의 노드를 자식으로 포함시킴

    //루트 노드의 시작점 역할을 수행함
    //다른 행동 트리 노드를 BTRoot 아래에 추가하면, 전체 행동 트리가 구성됨
    //BTRoot를 사용하여 행동 트리의 시작과 구조를 정의하고,
    //하위 노드를 통해 트리의 동작을 세부적으로 구현함.

    //★Tick메서드를 통해 행동 트리를 시작하고 실행함


    public class BTRoot : BTBehaviour
    {
        //현재 루트 노드의 자식 노드
        private BTBehaviour treeChild;

        //초기화 [해당 노드의 타입 설정&부모 노드 없음]
        public BTRoot()
        {
            SetNodeType(NodeType.Root);
            SetParent(null);
        }

        //자식 노드 추가 : 파라미터를 통해 전달된 BTBehaviour인스턴스를
        //현재 루트 노드(호출자)의 자식 노드에 할당
        //또한 현재 루트 노드(호출자)가 부모 노드가 됨.
        public void AddChild(BTBehaviour newChild)
        {
            treeChild = newChild;
            treeChild.SetParent(this);
        }

        //자식 노드 반환용
        public BTBehaviour GetChild()
        {
            return treeChild;
        }

        //부모 노드부터 자식 노드까지 싹다 종료
        public override void Terminate()
        {
            treeChild.Terminate(); // 자식 노드 종료 실행
            base.Terminate(); // 현재 루트 노드(부모) 종료 실행
        }

        //루트 노드에서 트리 실행이 시작됨
        public override Status Tick()
        {
            //자식 트리가 null이 아닌 경우
            if (treeChild == null)
            {
                //노드 상태 기본값
                return Status.BT_Invalid;
            }
            //자식 트리의 상태값이 기본인 경우
            else if(treeChild.GetStatus() == Status.BT_Invalid)
            {
                //노드초기화&노드 상태 작동중으로 변경
                treeChild.Initialize();
                treeChild.SetStatus(Status.BT_Running);
            }

            //자식 노드의 Update메서드 실행
            SetStatus(treeChild.Update());

            //결과를 현재 루트 노드 상태로 설정하고,
            //자식 노드의 상태 역시 현재 루트와 동일하게 설정함
            treeChild.SetStatus(GetStatus());
            

            //현재 루트 노드의 상태가 작동중이 아니라면?
            if(GetStatus() != Status.BT_Running)
            {
                //종료 작업 실행
                Terminate();
            }

            //현재 루트 노드의 상태를 반환
            return GetStatus();
        }

    }
}
