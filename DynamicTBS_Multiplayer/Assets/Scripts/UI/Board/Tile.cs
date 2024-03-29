using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile
{
    private TileType type;

    private int row;
    private int column;
    private Vector3 position;
    private GameObject tileGameObject;
    private Sprite tileSprite;
    private Character currentInhabitant;

    public delegate bool IsChangeable();
    public IsChangeable isChangeable;

    public Tile(TileType type, PlayerType side, int row, int column) 
    {
        this.type = type;
        this.row = row;
        this.column = column;
        this.tileSprite = TileSpriteManager.GetTileSprite(type, side, WithDepth());
        this.position = Board.FindPosition(row, column);
        this.currentInhabitant = null;
        this.isChangeable = () => type != TileType.GoalTile;
        this.tileGameObject = CreateTileGameObject();
    }

    public int GetRow()
    {
        return row;
    }

    public int GetColumn()
    {
        return column;
    }

    public TileType GetTileType()
    {
        return type;
    }

    public Character GetCurrentInhabitant()
    {
        return currentInhabitant;
    }

    public GameObject GetTileGameObject() { return tileGameObject; }

    public Vector3 GetPosition() 
    {
        return this.position;
    }

    public bool IsOccupied()
    {
        return currentInhabitant != null;
    }

    public bool IsAccessible() 
    {
        return !IsOccupied() && type != TileType.EmptyTile;
    }

    public void SetCurrentInhabitant(Character character)
    {
        currentInhabitant = character;
    }

    public Tile Transform(TileType newTileType)
    {
        this.type = newTileType;
        this.tileSprite = TileSpriteManager.GetTileSprite(newTileType, Board.FindSideOfTile(row), WithDepth());
        this.tileGameObject.GetComponent<SpriteRenderer>().sprite = this.tileSprite;

        return this;
    }

    public string GetTileName()
    {
        int row = 9 - this.GetRow();
        char columnChar = (char)(this.GetColumn() + 65);
        return columnChar.ToString() + row.ToString();
    }

    private GameObject CreateTileGameObject()
    {
        GameObject tile = new GameObject
        {
            name = GetTileName()
        };

        Quaternion startRotation = Quaternion.identity;

        SpriteRenderer spriteRenderer = tile.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = this.tileSprite;
        spriteRenderer.sortingOrder = -2;
        tile.transform.position = position;
        tile.transform.rotation = startRotation;

        tile.AddComponent<BoxCollider>();

        tile.transform.SetParent(GameObject.Find("GameplayObjects").transform);

        return tile;
    }

    private bool WithDepth()
    {
        bool withDepth = false;
        Tile tileAbove = Board.GetTileByCoordinates(row - 1, column);
        if (tileAbove != null && tileAbove.GetTileType() != TileType.EmptyTile)
        {
            withDepth = true;
        }

        return withDepth;
    }
}