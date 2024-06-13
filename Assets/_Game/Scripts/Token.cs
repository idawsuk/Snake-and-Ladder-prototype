using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Token : MonoBehaviour
{
    public delegate void MoveFinished();
    public MoveFinished OnMoveFinished;

    public Tile CurrentTile
    {
        get; set;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(List<Tile> targetTiles)
    {
        CurrentTile = targetTiles[targetTiles.Count - 1];

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < targetTiles.Count; i++)
        {
            int index = i;
            sequence.Append(transform.DOJump(targetTiles[i].transform.position, 1, 1, .5f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                if(index == targetTiles.Count - 1)
                    OnMoveFinished?.Invoke();
            }));
        }
    }

    public void Move(Tile targetTile)
    {
        CurrentTile = targetTile;
        
        transform.DOMove(targetTile.transform.position, 1f).SetEase(Ease.InOutQuart).OnComplete(() =>
        {
            OnMoveFinished?.Invoke();
        });
    }
}
