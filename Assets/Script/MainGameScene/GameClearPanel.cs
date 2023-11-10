using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : UIBase
{

    public override void Initialize()
    {
        // 특정 state에서 실행되는 이벤트를 추가하고 연결을 함..
        MainGameManager.Instance.OnEndStateChanged += Open;
    }
    
}
