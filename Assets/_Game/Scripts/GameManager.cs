using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Dice dice;
    [SerializeField] Board board;
    [SerializeField] Token token;

    [SerializeField] GameObject finishedGameUI;

    GameState state = GameState.THROWING_DICE;

    // Start is called before the first frame update
    void Start()
    {
        dice.OnDiceThrow += OnDiceThrew;
        token.OnMoveFinished += OnTokenFinishedMove;

        token.CurrentTile = board.GetTile(0);
        token.Move(token.CurrentTile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDiceThrew(int diceValue)
    {
        switch (state)
        {
            case GameState.THROWING_DICE:
                MoveToken(diceValue);
                break;
            case GameState.MOVING_TOKEN:
                break;
            case GameState.SNAKE_LADDER_EVENT:
                break;
            case GameState.GAME_FINISHED:
                break;
        }
    }

    void MoveToken(int diceValue)
    {
        state = GameState.MOVING_TOKEN;
        token.Move(board.GetTiles(token.CurrentTile, diceValue));
    }

    void OnTokenFinishedMove()
    {
        //check tile
        Tile currentTile = token.CurrentTile;
        int currentTileIndex = board.GetTileIndex(currentTile);
        int pairedTileIndex = board.GetTileIndex(currentTile.PairedTile);

        if(currentTile.Type == Tile.TileType.LADDER)
        {
            if(currentTileIndex < pairedTileIndex)
            {
                state = GameState.SNAKE_LADDER_EVENT;
                token.Move(currentTile.PairedTile);
                return;
            }
        } else if(currentTile.Type == Tile.TileType.SNAKE)
        {
            if (currentTileIndex > pairedTileIndex)
            {
                state = GameState.SNAKE_LADDER_EVENT;
                token.Move(currentTile.PairedTile);
                return;
            }
        } else
        {
            if(board.IsLastTile(currentTile))
            {
                state = GameState.GAME_FINISHED;
                finishedGameUI.SetActive(true);
                return;
            }
        }

        dice.EnableButton(true);
        state = GameState.THROWING_DICE;
    }

    public enum GameState
    {
        THROWING_DICE, MOVING_TOKEN, SNAKE_LADDER_EVENT, GAME_FINISHED
    }
}
