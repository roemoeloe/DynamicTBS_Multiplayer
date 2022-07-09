using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : Tile
{
    public FloorTile(Vector3 position) : base(position)
    {
        this.type = TileType.FloorTile;
        this.tileSprite = SpriteManager.FLOOR_TILE_SPRITE;
        
        Init();
    }
}