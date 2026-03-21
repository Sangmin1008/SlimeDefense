using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    private readonly Dictionary<Vector3Int, HeroPresenter> _gridData = new Dictionary<Vector3Int, HeroPresenter>();
    public bool IsEmpty(Vector3Int cellPos) => !_gridData.ContainsKey(cellPos);
    public HeroPresenter GetHero(Vector3Int cellPos) => _gridData[cellPos];
    public void PlaceHero(Vector3Int cellPos, HeroPresenter hero) => _gridData[cellPos] = hero;
    public void ClearCell(Vector3Int cellPos) => _gridData.Remove(cellPos);
}
