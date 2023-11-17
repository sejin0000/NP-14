using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class SetTile : MonoBehaviour
{
    
    public Tilemap wallTileMap;
    public Tilemap groundTileMap;

    public RuleTile wallTile;
    public RuleTile groundTile;


    public void SetRectTile(RectInt rect, Tilemap tilemap, Vector2 Startpos)
    {
        for (int i = 0; i < rect.width; i++)
        {
            for (int j = 0; j < rect.height; j++)
            {
                tilemap.SetTile(new Vector3Int(i + ((int)Startpos.x), j + ((int)Startpos.y)), null);
            }
        }
    }
    public void SetRectTile(RectInt rect, Tilemap tilemap)
    {
        for (int i = 0; i < rect.width; i++)
        {
            for (int j = 0; j < rect.height; j++)
            {
                tilemap.SetTile(new Vector3Int(i - (rect.width / 2), j - (rect.height / 2)), null);
            }
        }
    }
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
    public void SetRectTile(RectInt rect, Tilemap tilemap, RuleTile tile)
    {
        for (int i = 0; i < rect.width; i++)
        {
            for (int j = 0; j < rect.height; j++)
            {
                tilemap.SetTile(new Vector3Int(i - (rect.width / 2), j - (rect.height / 2)), tile);
            }
        }
    }

    public void SetLineTile(Tilemap tilemap, Vector2Int startPos,int length, Vector2 direction,Vector2 Startpos)
    {
        for (int j = 0; j < length + 1; j++)
        {
            tilemap.SetTile(new Vector3Int(startPos.x - ((int)Startpos.x), startPos.y - ((int)Startpos.y)), null);
            startPos.x -= (int)direction.x;
            startPos.y -= (int)direction.y;
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
}
