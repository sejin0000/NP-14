using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpcRegistration : MonoBehaviour
{
    private void Awake()
    {
        // 포톤 네트워크에 타입을 등록
        PhotonPeer.RegisterType(typeof(CustomRectInt), 0, CustomRectInt.Serialize, CustomRectInt.Deserialize);

    }
}
