using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] Vector2Int boardSize;
    [SerializeField] Vector2 offset;
    [SerializeField] Vector2 spacing;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject ladderPrefab;
    [SerializeField] GameObject snakePrefab;

    List<Tile> tiles;

    void Awake()
    {
        GenerateBoard();

        int ladderCount = PlayerPrefs.GetInt("ladder", 2);
        int snakeCount = PlayerPrefs.GetInt("snake", 2);
        RandomizeLadderAndSnake(ladderCount, snakeCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateBoard()
    {
        tiles?.Clear();
        tiles = new List<Tile>();

        for (int y = 0; y < boardSize.y; y++)
        {
            if(y % 2 == 0)
            {
                for (int x = boardSize.x - 1; x >= 0; x--)
                {
                    InstantiateTile(new Vector3(x + (x * spacing.x), y + (y * spacing.y), 0));
                }
            } else
            {
                for (int x = 0; x < boardSize.x; x++)
                {
                    InstantiateTile(new Vector3(x + (x * spacing.x), y + (y * spacing.y), 0));
                }
            }
        }
    }

    void InstantiateTile(Vector3 position)
    {
        GameObject newTile = Instantiate(tilePrefab, this.transform);
        newTile.transform.localPosition = position + (Vector3)offset;
        newTile.SetActive(true);

        Tile tile = newTile.GetComponent<Tile>();

        tiles.Add(tile);

        tile.SetTileNumber(tiles.Count);
    }

    public List<Tile> GetTiles(Tile tileIndex, int tileCount)
    {
        int startIndex = tiles.IndexOf(tileIndex);

        //normal move
        if(startIndex + 1 + tileCount < tiles.Count)
        {
            return tiles.GetRange(startIndex + 1, tileCount);
        } else //token will reach the last tile, but has extra move(s)
        {
            List<Tile> tilePath = new List<Tile>();

            int move = tileCount - 1;

            //get all remaining tiles
            for (int i = startIndex + 1; i < tiles.Count; i++)
            {
                move--;
                tilePath.Add(tiles[i]);
            }

            int tileIndexOffset = 0;

            //add backward path tiles
            while (move >= 0)
            {
                tileIndexOffset++;
                move--;
                tilePath.Add(tiles[tiles.Count - 1 - tileIndexOffset]);
            }

            return tilePath;
        }
    }

    public Tile GetTile(int index)
    {
        return tiles[index];
    }

    public void RandomizeLadderAndSnake(int ladderCount, int snakeCount)
    {
        List<int> selectedIndexLadder = new List<int>();

        for (int i = 0; i < ladderCount * 2; i++) //double the count to find the ladder pair index
        {
            bool uniqueIndex = false;
            int randomIndex = 0;

            while(!uniqueIndex)
            {
                randomIndex = Random.Range(1, tiles.Count - 1); //exclude first and last tile

                uniqueIndex = selectedIndexLadder.IndexOf(randomIndex) == -1; //check if randomIndex is already on the list, we want unique index
            }

            selectedIndexLadder.Add(randomIndex);
        }

        List<int> selectedIndexSnake = new List<int>();
        for (int i = 0; i < snakeCount * 2; i++) //double the count to find the snake pair index
        {
            bool uniqueIndex = false;
            int randomIndex = 0;

            while (!uniqueIndex)
            {
                randomIndex = Random.Range(1, tiles.Count - 1); //exclude first and last tile

                uniqueIndex = (selectedIndexLadder.IndexOf(randomIndex) == -1 && selectedIndexSnake.IndexOf(randomIndex) == -1); //check if randomIndex is already on the list, we want unique index
            }

            selectedIndexSnake.Add(randomIndex);
        }


        //pair the ladders and snakes. I will use odd number as start point, and even number as end point
        for (int i = 0; i < selectedIndexLadder.Count; i++)
        {
            int index = selectedIndexLadder[i];

            //setup ladder
            tiles[index].Type = Tile.TileType.LADDER;

            if(i % 2 == 0)
            {
                tiles[index].PairedTile = tiles[selectedIndexLadder[i + 1]];
                GameObject ladderObject = Instantiate(ladderPrefab);

                ladderObject.SetActive(true);

                LineRenderer lineRenderer = ladderObject.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, tiles[index].transform.position);
                lineRenderer.SetPosition(1, tiles[index].PairedTile.transform.position);
            } else
            {
                tiles[index].PairedTile = tiles[selectedIndexLadder[i - 1]];
            }
        }

        for (int i = 0; i < selectedIndexSnake.Count; i++)
        {
            int index = selectedIndexSnake[i];

            //setup snake
            tiles[index].Type = Tile.TileType.SNAKE;

            if (i % 2 == 0)
            {
                tiles[index].PairedTile = tiles[selectedIndexSnake[i + 1]];
                GameObject snakeObject = Instantiate(snakePrefab);

                snakeObject.SetActive(true);

                LineRenderer lineRenderer = snakeObject.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, tiles[index].transform.position);
                lineRenderer.SetPosition(1, tiles[index].PairedTile.transform.position);
            }
            else
            {
                tiles[index].PairedTile = tiles[selectedIndexSnake[i - 1]];
            }
        }
    }

    public int GetTileIndex(Tile tile)
    {
        return tiles.IndexOf(tile);
    }

    public bool IsLastTile(Tile tile)
    {
        return tiles.IndexOf(tile) == tiles.Count - 1;
    }
}
