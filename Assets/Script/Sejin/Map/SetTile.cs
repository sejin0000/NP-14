using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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

    private void Start()
    {
        GameManager.Instance.OnStageEndEvent += TileClear;
    }

    public void TileClear()
    {
        Debug.Log("Áö¿ì±â");
        groundTileMap.ClearAllTiles();  
        wallTileMap.ClearAllTiles();
        doorTileMap.ClearAllTiles();
    }


    public void OrderSetRectTile(RectInt rect, Tilemap tilemap, RuleTile tile, Vector2 Startpos)
    {
        TileMapType _tileMap;
        TileMapType _tile;

        Vector2 rectPos;
        Vector2 widthHeight;

        TileToEnum(out _tileMap, out _tile, tilemap, tile);
        RectIntToVector2(rect, out rectPos, out widthHeight);

        PV.RPC("PunSetRectTile",RpcTarget.AllBuffered, _tileMap, _tile, Startpos, rectPos, widthHeight);
    }

    [PunRPC]
    public void PunSetRectTile(TileMapType _TileMap, TileMapType _Tile, Vector2 startPos, Vector2 rectPos, Vector2 widthHeight)
    {
        Tilemap tilemap;
        RuleTile tile;

        RectInt rect;

        EnumToTile(_TileMap,_Tile,out tilemap, out tile);
        Vector2ToRectInt(out rect, rectPos, widthHeight);

        SetRectTile(rect, tilemap, tile, startPos);
    }


    public void OrderSetLineTile(Tilemap tilemap, RuleTile tile, Vector2Int startPos, int length, Vector2 direction)
    {
        TileMapType _tileMap;
        TileMapType _tile;

        TileToEnum(out _tileMap, out _tile, tilemap, tile);

        PV.RPC("PunSetLineTile",RpcTarget.AllBuffered, _tileMap, _tile, (Vector2)startPos, length, direction);
    }

    [PunRPC]
    public void PunSetLineTile(TileMapType _TileMap, TileMapType _Tile, Vector2 startPos,int length,Vector2 direction)
    {
        Tilemap tilemap;
        RuleTile tile;


        EnumToTile(_TileMap, _Tile, out tilemap, out tile);

        SetLineTile(tilemap, tile, new Vector2Int((int)startPos.x, (int)startPos.y), length, direction);
    }

    public void OrderSetDoorTile(Vector2Int vector, Tilemap tilemap, RuleTile tile)
    {
        TileMapType _tileMap;
        TileMapType _tile;

        TileToEnum(out _tileMap, out _tile, tilemap, tile);


        PV.RPC("PunSetDoorTile", RpcTarget.AllBuffered, _tileMap, _tile, (Vector2)vector);
    }

    [PunRPC]
    public void PunSetDoorTile(TileMapType _TileMap, TileMapType _Tile, Vector2 startPos)
    {
        Tilemap tilemap;
        RuleTile tile;


        EnumToTile(_TileMap, _Tile, out tilemap, out tile);

        SetDoorTile(new Vector2Int( (int)startPos.x, (int)startPos.y),tilemap,tile);
    }






    public void TileToEnum(out TileMapType _TileMapEnum,out  TileMapType _TileEnum, Tilemap tilemap, RuleTile tile)
    {
        _TileMapEnum = 0;
        _TileEnum = 0;

        if (tilemap == groundTileMap)
        {
            _TileMapEnum = (TileMapType)1;
        }
        else if (tilemap == wallTileMap)
        {
            _TileMapEnum = (TileMapType)2;
        }
        else if (tilemap == doorTileMap)
        {
            _TileMapEnum = (TileMapType)3;
        }

        if (tile == null)
        {
            _TileEnum = (TileMapType)0;
        }
        else if (tile == groundTile)
        {
            _TileEnum = (TileMapType)1;
        }
        else if (tile == wallTile)
        {
            _TileEnum = (TileMapType)2;
        }
        else if (tile == doorTile)
        {
            _TileEnum = (TileMapType)3;
        }
    }

    public void EnumToTile(TileMapType _TileMapEnum, TileMapType _TileEnum , out Tilemap tilemap , out RuleTile tile)
    {
        tilemap = null;
        tile = null;

        if (_TileMapEnum == (TileMapType)1)
        {
            tilemap = groundTileMap;
        }
        else if (_TileMapEnum == (TileMapType)2)
        {
            tilemap = wallTileMap;
        }
        else if (_TileMapEnum == (TileMapType)3)
        {
            tilemap = doorTileMap;
        }

        if (_TileEnum == (TileMapType)0)
        {
            tile = null;
        }
        else if (_TileEnum == (TileMapType)1)
        {
            tile = groundTile;
        }
        else if (_TileEnum == (TileMapType)2)
        {
            tile = wallTile;
        }
        else if (_TileEnum == (TileMapType)3)
        {
            tile = doorTile;
        }
    }


    public void RectIntToVector2(RectInt rect, out Vector2 rectPos, out Vector2 widthHeight)
    {
        rectPos.x = rect.x;
        rectPos.y = rect.y;
        widthHeight.x = rect.width;
        widthHeight.y = rect.height;
    }

    public void Vector2ToRectInt(out RectInt rect, Vector2 rectPos, Vector2 widthHeight)
    {
        rect = new RectInt();

        rect.x = (int)rectPos.x;
        rect.y = (int)rectPos.y;
        rect.width  = (int)widthHeight.x;
        rect.height = (int)widthHeight.y;
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
    }


    public void SetLineTile(Tilemap tilemap, RuleTile tile, Vector2Int startPos, int length, Vector2 direction)
    {
        for (int j = 0; j < length + 1; j++)
        {
            tilemap.SetTile(new Vector3Int(startPos.x, startPos.y), tile);
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
}
