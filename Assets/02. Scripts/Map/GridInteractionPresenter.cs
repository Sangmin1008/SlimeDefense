using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer.Unity;

public class GridInteractionPresenter : IInitializable, IDisposable
{
    private readonly GridManager _gridManager;
    private readonly GridClickDetector _clickDetector;
    
    private CompositeDisposable _disposables = new CompositeDisposable();

    public GridInteractionPresenter(GridManager gridManager, GridClickDetector clickDetector)
    {
        _gridManager = gridManager;
        _clickDetector = clickDetector;
    }
    
    public void Initialize()
    {
        _clickDetector.OnGridClicked += HandleGridClicked;
    }

    private void HandleGridClicked(Vector3Int cellPos, Vector3 worldPos)
    {
        if (_gridManager.IsEmpty(cellPos))
        {
            Debug.Log($"[{cellPos}] 빈칸 클릭 -> [소환 팝업 UI] 띄우기");
        }
        else
        {
            HeroPresenter hero = _gridManager.GetHero(cellPos);
            Debug.Log($"[{cellPos}] {hero.Model.Config.HeroName} 클릭 -> [승급/판매 UI] 띄우기");
        }
    }

    public void Dispose()
    {
        if (_clickDetector != null)
        {
            _clickDetector.OnGridClicked -= HandleGridClicked;
        }
        _disposables.Dispose();
    }
}
