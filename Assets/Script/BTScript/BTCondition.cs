using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--
//EBH_Running 존재하지 않음 : EBH_Success or EBH_Failure
//Initialize(), Terminate() 사용 안함 : 왜? 의미 없는 함수라서(조건만 체크함)
//--

//시퀀스(Sequence)노드에서 가장 먼저 접하는 노드
//행동 트리에서 사용되는 조건(Condition) 노드를 정의하는 스크립트
//조건 노드는 주로 두 가지 상태, 성공 혹은 실패를 가지며
//특정 조건을 판단하고 그 결과에 따라 다음 노드로 진행한다.
namespace myBehaviourTree
{
    public class BTCondition : BTBehaviour
    {
        //노드 타입 지정(각 노드에 다 지정해야함)
        public BTCondition()
        {
            SetNodeType(NodeType.Condition);
        }

        //조건 노드의 주요 동작을 수행함
        public override Status Tick()
        {
            //노드 상태 업데이트
            SetStatus(Update());

            if (GetStatus() == Status.BT_Running)
            {
                Debug.Log("EBH_Running은 사용하지 않습니다. EBH_Success 혹은 EBH_Failure가 유효한 상태입니다.");
            }

            //★최적화 필요함 : 이전 Action node를 참조하는 필드 변수로 만들어 사용

            //상태 : EBH_Success인 경우 ==> 작동중인 Action 종료[다른 노드와의 충돌 방지/행동 트리거의 일관성 유지]
            if (GetStatus() == Status.BT_Success)
            {
                //이전에 존재한 다른 노드 값 초기화 - 반드시 들어가는 조건문 필요함
                EndRunningByAction();
            }

            return GetStatus();
        }

        //현재 조건 노드와 관련된 작동 중인 액션을 종료하는 메서드
        //루트 노드 탐색 -> 작동 중인 액션 탐색 ->
        //탐색된 노드가 현재 조건 노드와 같은 부모를 가지고 있지 않거나, 부모가 Sequence가 아닌 경우
        //==> 작동 중인 액션 종료
        public void EndRunningByAction()
        {
            //루트 노드용 변수
            BTBehaviour BTFindRoot = null;

            //오류 카운트 : 지정하는 이유? => 반복 작업 제한
            //행동 트리 구조가 잘못된 경우나 무한 루프에 빠질 때 프로그램이 뻗는걸 방지
            int ErrorCount = 0;

            //현재 조건 노드가 속한 행동 트리의 루트 노드를 찾는 과정 수행
            //행동 트리 : 계층 구조 // 루트 노드 : 트리의 최상위 노드 => 모든 노드는 루트 노드에서 파생됨


            //1. 현재 조건 노드의 부모 노드 할당
            BTFindRoot = GetParent();

            if (BTFindRoot != null)
            {
                //2. 최대 100번 반복 : 왜 100번? : 트리의 복잡도에 따라 개수가 달라질것(하위에서 최상위 노드까지 탐색해야 하므로)
                while (ErrorCount < 100)
                {
                    //3. 각 반복에서 BTFindRoot에 현재 BTFindRoot의 부모 노드를 할당.
                    //==>무슨 소리냐? : 계층 구조에 따라 반복하며 부모 노드를 찾아서 점점 올라감
                    BTFindRoot = BTFindRoot.GetParent();

                    //그렇게 올라가다가 부모 노드가 없는 경우가 되면?(최상위 노드 : 루트 노드에 도달)
                    if (BTFindRoot.GetParent() == null)
                    {
                        //반복 탈출
                        break;
                    }

                    ++ErrorCount;
                }
            }

            //위에서 찾은 루트 노드의 상태를 확인 후, 액션 노드를 찾아 처리
            //★위 아래 if문 순서 중요


            //루트 노드를 찾았는지 체크(찾은 경우 아래를 실행)
            if (BTFindRoot != null)
            {
                //루트 노드 상태 체크(행동 트리가 작동 중인지 확인)
                if (BTFindRoot.GetStatus() == Status.BT_Running)
                {
                    //현재 실행중인 액션 노드를 찾아서 BTRunningAction에 할당
                    //이때, 파라미터 값 ((BTRoot)BTFindRoot).GetChild()를 통해 
                    //루트 노드의 첫 자식 노드 부터 탐색하도록 함
                    BTBehaviour BTRunningAction = FindRunningAction(((BTRoot)BTFindRoot).GetChild());

                    //실행중인 '액션' 노드를 찾은 경우
                    if (BTRunningAction != null)
                    {
                        //만약 this Condition과 Running Action이 다른 부모를 가지거나
                        //현재 노드의 부모가 Sequence가 아니라면 Terminate 호출
                        if (GetParent() != BTRunningAction.GetParent() || GetParent().GetNodeType() != NodeType.Sequence)
                        {
                            //실행중인 액션을 종료
                            BTRunningAction.Terminate();
                        }

                    }
                }
            }
        }


        //실행 중인 액션 노드를 탐색[재귀적으로]
        //파라미터값으로 부터 실행 중인 액션 노드를 탐색한다.
        public BTBehaviour FindRunningAction(BTBehaviour BTChild)
        {
            //실행 중인 액션 노드 저장용
            BTBehaviour BTRunningAction = null;

            //파라미터 값이 null이 아닌 경우(노드 존재)
            if (BTChild != null)
            {
                //BTChild의 노트 타입이 Selector 거나, Sequence인 경우만 실행(복합(Composite) 노드인 경우)
                if (BTChild.GetNodeType() == NodeType.Selector || BTChild.GetNodeType() == NodeType.Sequence)
                {
                    //복합 노드의 모든 자식 노드를 체크(복합 노드 스크립트의 GetChildCount 사용)
                    for (int i = 0; i < ((BTComposite)BTChild).GetChildCount(); i++)
                    {
                        //재귀적으로 FindRunningAction 메서드를 호출하여, 현재 자식 노드에 대해
                        //실행 중인 액션 노드를 탐색하고, 그것을 BTRunningAction에 저장
                        //이것은 현재 조건 노드 아래의 모든 하위 노드에 대해 실행 중인 액션 노드를 찾아야하므로
                        //★재귀 호출이 수행된다.
                        BTRunningAction = FindRunningAction(((BTComposite)BTChild).GetChild(i));

                        //실행 중인 액션 노드를 찾은경우, 해당 노드를 반환하고 탐색 종료
                        if (BTRunningAction != null)
                            return BTRunningAction;
                    }
                }

                //실행 중인 액션 노드인지 판별 : 노드유형 Action & 상태 실행중
                if (BTChild.GetNodeType() == NodeType.Action && BTChild.GetStatus() == Status.BT_Running)
                {
                    //실행중인 액션 노드 반환 => 재귀호출 중단 => 실행 중인 액션 노드가 반환
                    return BTChild;
                }
            }

            //모든 자식 노드에서 실행 중인 액션을 찾지 못한 경우 ==> null 반환
            return BTRunningAction;
        }

        //재귀 구조 BTChild라는 객체를 파라미터로 얻어옴 null이라면 해당 서브 트리에는 실행 중인 액션 노드가 없으므로 null 반환 후종료
        //그 외의 경우 BTChild의 유형을 체크, 복합 노드라면 자식 노드에 대해서 재귀적으로 FindRunningAction를 호출함
        //=> 자식 노드 중에서 복합 노드가 아닌 경우, 액션 노드를 판별 받을것
    }

}