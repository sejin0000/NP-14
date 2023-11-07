using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResultController : MonoBehaviour
{
    public AugmentManager augmentManager;
    public ResultManager resultManager;

    public void MakeManager() 
    {
        ResultManager Prefab = Resources.Load<ResultManager>("Prefabs/ResultManager");
        //없으면 로드하는걸로 리소스 로드 = 불러오기가 엄청무거움 =시작때 처음할것, 혹은 최소한만 플레이하게 장치를 해둘것
        //프리팹은 한번만 로드한다 스타트 혹은 어웨이크에서 로드해서 관리할것
        resultManager = Instantiate(Prefab);
        augmentManager = new AugmentManager();
    }
}
