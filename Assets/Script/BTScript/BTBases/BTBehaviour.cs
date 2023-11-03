using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BT의 기본구조 정의 스크립트
//구체적인 행동 트리 노드는 이 클래스를 상속하여 구현
//'Tick()'메서드를 호출하여 노드를 실행하고, 상태를 업데이트하는 것은
//행동 트리의 동작을 주도하는 핵심이다.


//네임 스페이스 선언 => 아래 내용(enum타입을 포함한 class까지)
//사용 시 원하는 클래스에서 using 네임스페이스이름 을 통해서 사용가능
//해당 네임스페이스 사용한 virtual 메서드에서 ovveride(재정의) 또한 가능
namespace myBehaviourTree
{
    //노드의 실행 결과 상태(각 노드의 실행 결과로 반환됨)
    public enum Status
    {
        BT_Invalid, // 방문 노드가 아닐 때 기본(최소) 상태값
        BT_Success, // 성공적으로 완료됨
        BT_Failure, // 완료하지 못함
        BT_Running, // 현재 진행중인 Action임
        BT_Aborted, // 중단됨
    };

    //행동 트리의 다양한 노드 유형 정의
    //각 노드는 이 유형 중 하나의 유형을 가짐
    public enum NodeType
    {
        Root,
        Selector,
        Sequence,
        Paraller,
        Decorator,
        Condition,
        Action,
    };

    public class BTBehaviour
    {
        //아래 두개가 중요함
        private Status status; // 초기 기본값 EBH_Invalid
        private NodeType nodeType; // 노드 타입
        private int index; // 노드 인덱스
        private BTBehaviour treeParent; // 부모 노드

        public BTBehaviour()
        {
            status = Status.BT_Invalid;
        }

        //노드의 종료 여부 반환 메서드(완료 혹은 실패 시 true)
        public bool IsTerminated()
        {
            //비트 or 연산자 | : 둘 중 하나라도 참이라면 true를 반환한다.
            return status == Status.BT_Success | status == Status.BT_Failure;
        }

        //노드의 작동중 여부 반환 메서드
        public bool IsRunning()
        {
            return status == Status.BT_Running;
        }



        //부모 노드 세팅&반환
        public void SetParent(BTBehaviour NewParrent)
        {
            treeParent = NewParrent;
        }
        public BTBehaviour GetParent()
        {
            return treeParent;
        }

        //상태 세팅&반환
        public void SetStatus(Status NewStatus)
        {
            status = NewStatus;
        }
        public Status GetStatus()
        {
            return status;
        }

        //역할 노드 세팅&반환
        public void SetNodeType(NodeType NewNodeType)
        {
            nodeType = NewNodeType;
        }
        public NodeType GetNodeType()
        {
            return nodeType;
        }

        //인덱스 세팅&반환
        public void SetIndex(int NewIndex)
        {
            index = NewIndex;
        }
        public int GetIndex()
        {
            return index;
        }


        //노드 상태 초기화
        public virtual void Reset()
        {
            status = Status.BT_Invalid;
        }
        

        //가상 메서드 = 노드를 초기화하는 경우 호출함(다른 곳에서 재정의)
        public virtual void Initialize()
        {
            
        }

        //가상 메서드 = 노드를 종료하는 경우 호출함(다른 곳에서 재정의)
        public virtual void Terminate()
        {

        }


        //상태 업데이트를 수행하고, 해당 상태를 반환(다른 클래스에서 재정의 가능)
        public virtual Status Update()
        {
            return Status.BT_Success;
        }

        //노드를 실행하고, 상태를 업데이트하고, 필요한 경우 초기화 및 종료를 수행
        //위의 Update()와 함께 실행됨
        public virtual Status Tick()
        {
            //현재 상태가 기본값이라면
            if(status == Status.BT_Invalid)
            {
                //초기화 실행
                Initialize();
                //노드 상태 작동중으로 변경
                status = Status.BT_Running;
            }

            //특수한 경우가 아니라면 상태 업데이트 메서드를 통해 현재 상태를 변경
            //status는 Enum타입이며, Update는 그 타입중 하나를 반환하므로 대입가능
            status = Update();


            //현재 상태가 작동중이 아니라면
            if(status != Status.BT_Running)
            {
                //종료 실행
                Terminate();
            }

            //결과적으로 나온 상태를 반환
            return status;
        }
    }
}
