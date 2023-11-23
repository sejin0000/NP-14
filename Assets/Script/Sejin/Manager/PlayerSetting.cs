using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }


    public void InstantiatePlayer()
    {
        if (PV == null)
        {
            PV = GetComponent<PhotonView>();
        }

        string playerPrefabPath = "Pefabs/Player";
        GameManager.Instance.clientPlayer = PhotonNetwork.Instantiate(playerPrefabPath, Vector3.zero, Quaternion.identity);


        int viewID = GameManager.Instance.clientPlayer.GetPhotonView().ViewID;
        PV.RPC("PlayerInfoDictionarySetting", RpcTarget.AllBuffered, viewID);


        StartCoroutine(SendPlayerLocation());
    }

    [PunRPC]
    private void PlayerInfoDictionarySetting(int viewID)
    {
        GameObject clientPlayer = PhotonView.Find(viewID).gameObject;
        GameManager.Instance.playerInfoDictionary.Add(viewID, clientPlayer.transform);
    }

    [PunRPC]
    private void PunSendPlayerLocation(Vector2 playerPos)//호스트만 받음
    {
        Debug.Log("PunSendPlayerLocation");
        List<Node> _allRoomList = GameManager.Instance._mapGenerator.GetComponent<MapGenerator>().roomNodeInfo.allRoomList;

        for (int i = 0; i < _allRoomList.Count; i++)
        {
            _allRoomList[i].roomInPlayer = false;
            if (_allRoomList[i].roomRect.x < playerPos.x && _allRoomList[i].roomRect.x + _allRoomList[i].roomRect.width > playerPos.x &&
                _allRoomList[i].roomRect.y < playerPos.y && _allRoomList[i].roomRect.y + _allRoomList[i].roomRect.height > playerPos.y)
            {
                _allRoomList[i].roomInPlayer = true;
                if (_allRoomList[i].thisRoomClear == false)
                {
                    GameManager.Instance.CallRoomStartEvent();
                    PV.RPC("PlayerPositionSetting", RpcTarget.All,playerPos,new Vector2(_allRoomList[i].roomRect.x, _allRoomList[i].roomRect.y),new Vector2(_allRoomList[i].roomRect.width, _allRoomList[i].roomRect.height));
                }
            }
        }
    }

    [PunRPC]
    private void PlayerPositionSetting(Vector2 roomInPlayer, Vector2 rectPos,Vector2 widthHeight)
    {
        Vector2 player = GameManager.Instance.clientPlayer.transform.position;
        if (rectPos.x < player.x && rectPos.x + widthHeight.x > player.x &&
            rectPos.y < player.y && rectPos.y + widthHeight.y > player.y)
        {
            return;
        }
        else
        {
            int i = Random.Range(0, 2);
            int j = Random.Range(0, 2);


            Vector2 vector;

            if(i == 0)
            {
                vector = new Vector2(j,0);
            }
            else
            {
                vector = new Vector2(0, j);
            }

            if (rectPos.x < roomInPlayer.x + vector.x && rectPos.x + widthHeight.x > roomInPlayer.x + vector.x &&
                rectPos.y < roomInPlayer.y + vector.y && rectPos.y + widthHeight.y > roomInPlayer.y + vector.y)
            {
                GameManager.Instance.clientPlayer.transform.position = roomInPlayer += vector;
            }
            else
            {
                GameManager.Instance.clientPlayer.transform.position = roomInPlayer -= vector;

            }


        }
    }

    IEnumerator SendPlayerLocation()
    {
        while (true) 
        { 
            yield return new WaitForSeconds(1F);
            if (GameManager.Instance.clientPlayer != null)
            {
                PV.RPC("PunSendPlayerLocation", RpcTarget.MasterClient, (Vector2)GameManager.Instance.clientPlayer.transform.position);
            }
        }
    }

}
