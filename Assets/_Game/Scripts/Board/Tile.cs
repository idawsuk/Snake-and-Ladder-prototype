using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType Type = TileType.NORMAL_TILE;
    public Tile PairedTile; //for ladder or snake
    [SerializeField] TMPro.TextMeshPro tileNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public enum TileType
    {
        NORMAL_TILE, SNAKE, LADDER
    }

    public void SetTileNumber(int index)
    {
        tileNumber.text = index.ToString();
    }
}
