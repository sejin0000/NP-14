using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpcRegistration : MonoBehaviour
{
    private void Awake()
    {
        // ���� ��Ʈ��ũ�� Ÿ���� ���
        PhotonPeer.RegisterType(typeof(CustomRectInt), 0, CustomRectInt.Serialize, CustomRectInt.Deserialize);

    }
}
