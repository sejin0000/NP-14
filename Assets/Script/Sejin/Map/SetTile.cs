using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public enum TileMapType
{
    Null = 0,
    Ground = 1,
    Wall = 2,
    Door = 3
}
public class SetTile : MonoBehaviourPun
{
    
    public Tilemap groundTileMap;
    public Tilemap wallTileMap;
    public Tilemap doorTileMap;

    public RuleTile groundTile;
    public RuleTile wallTile;
    public RuleTile doorTile;

    public PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();        
    }


    [PunRPC]
    public void PunSetRectTile(TileMapType _TileMap, TileMapType _Tile, Vector2Int startPos, RectInt rect)
    {

        Tilemap tilemap = null;
        RuleTile tile = null;

        if(_TileMap == (TileMapType)1)
        {
            tilemap = groundTileMap;
        }
        else if(_TileMap == (TileMapType)2)
        {
            tilemap = wallTileMap;
        }
        else if(_TileMap == (TileMapType)3)
        {
            tilemap = doorTileMap;
        }

        if(_Tile == (TileMapType)0)
        {
            tile = null;
        }
        else if (_Tile == (TileMapType)1)
        {
            tile = groundTile;
        }
        else if (_Tile == (TileMapType)2)
        {
            tile = wallTile;
        }
        else if (_Tile == (TileMapType)3)
        {
            tile = doorTile;
        }

        SetRectTile(rect, tilemap, tile, startPos);
    }


    // SetTile----------------------------------------------------------------------------------------------

    public void SetRectTile(RectInt rect, Tilemap tilemap, RuleTile tile, Vector2 Startpos)
    {
        for (int i = 0; i < rect.width; i++)
        {
            for (int j = 0; j < rect.height; j++)
            {
                tilemap.SetTile(new Vector3Int(i + ((int)Startpos.x), j + ((int)Startpos.y)), tile);
            }
        }

        if(PhotonNetwork.IsMasterClient)
        {

        }
    }


    public void SetLineTile(Tilemap tilemap, RuleTile tile, Vector2Int startPos, int length, Vector2 direction, Vector2 Startpos)
    {
        for (int j = 0; j < length + 1; j++)
        {
            tilemap.SetTile(new Vector3Int(startPos.x - ((int)Startpos.x), startPos.y - ((int)Startpos.y)), tile);
            startPos.x -= (int)direction.x;
            startPos.y -= (int)direction.y;
        }
    }



    public void SetDoorTile(Vector2Int vector,Tilemap tilemap,RuleTile tile)
    {
        tilemap.SetTile((Vector3Int)vector,tile);
        vector.y += 1;
        tilemap.SetTile((Vector3Int)vector, tile);
    }


    // RemoveTile----------------------------------------------------------------------------------------------

    public void RemoveRectTile(RectInt rect, Tilemap tilemap, Vector2 Startpos)
    {
        for (int i = 0; i < rect.width; i++)
        {
            for (int j = 0; j < rect.height; j++)
            {
                tilemap.SetTile(new Vector3Int(i + ((int)Startpos.x), j + ((int)Startpos.y)), null);
            }
        }
    }

    public void RemoveLineTile(Tilemap tilemap, Vector2Int startPos,int length, Vector2 direction,Vector2 Startpos)
    {
        for (int j = 0; j < length + 1; j++)
        {
            tilemap.SetTile(new Vector3Int(startPos.x - ((int)Startpos.x), startPos.y - ((int)Startpos.y)), null);
            startPos.x -= (int)direction.x;
            startPos.y -= (int)direction.y;
        }
    }
}
