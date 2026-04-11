using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer.Unity;

public class GridInteractionPresenter : IInitializable, IDisposable
{
    private readonly GridManager _gridManager;
    private readonly HeroManager _heroManager;
    private readonly GridClickDetector _clickDetector;
    private readonly GameUIView _uiView;
    private readonly CoinModel _coinModel;
    
    private Vector3Int _selectedCellPos;
    private Vector3 _selectedWorldPos;
    private HeroPresenter _selectedHero;
    private CompositeDisposable _disposables = new CompositeDisposable();
    

    public GridInteractionPresenter(GridManager gridManager, GridClickDetector clickDetector, GameUIView uiView, HeroManager heroManager, CoinModel coinModel)
    {
        _gridManager = gridManager;
        _clickDetector = clickDetector;
        _uiView = uiView;
        _heroManager = heroManager;
        _coinModel = coinModel;
    }
    
    public void Initialize()
    {
        _clickDetector.OnGridClicked += HandleGridClicked;
        
        foreach (var brokenPos in _clickDetector.GetBrokenCells())
        {
            _gridManager.RegisterBrokenCell(brokenPos);
        }
        
        _uiView.OnSummonClicked
            .Subscribe(_ =>
            {
                _heroManager.TrySpawnHero(HeroGrade.Normal, _selectedCellPos, _selectedWorldPos);
                _uiView.HideGridPopup();
            })
            .AddTo(_disposables);
        
        _uiView.OnUpgradeClicked
            .Subscribe(_ =>
            {
                if (_selectedHero != null)
                {
                    _heroManager.TryUpgradeHero(_selectedHero);
                }
                _uiView.HideGridPopup();
            })
            .AddTo(_disposables);
        
        _uiView.OnRepairClicked
            .Subscribe(_ =>
            {
                int repairCost = 50;
                
                if (_coinModel.TrySpendCoin(repairCost))
                {
                    _gridManager.RepairCell(_selectedCellPos);
                    _clickDetector.ChangeToNormalTile(_selectedCellPos);
                }
                
                _uiView.HideGridPopup();
            })
            .AddTo(_disposables);
        
        _coinModel.CurrentCoin
            .Subscribe(amount => _uiView.UpdateCoin(amount))
            .AddTo(_disposables);
    }

    private void HandleGridClicked(Vector3Int cellPos, Vector3 worldPos)
    {
        _selectedCellPos = cellPos;
        _selectedWorldPos = worldPos;

        if (_gridManager.IsBroken(cellPos))
        {
            int cost = 50;
            _uiView.ShowRepairGridPopup(worldPos, cost);
        }
        else if (_gridManager.IsEmpty(cellPos))
        {
            _selectedHero = null;
            int cost = HeroCostHelper.GetCost(HeroGrade.Normal);
            _uiView.ShowGridPopup(worldPos, isSummon: true, cost);
        }
        else
        {
            _selectedHero = _gridManager.GetHero(cellPos);
            if (_selectedHero.Model.Config.Grade == HeroGrade.Legendary) return;
            
            int cost = HeroCostHelper.GetCost(_selectedHero.Model.Config.Grade);
            _uiView.ShowGridPopup(worldPos, isSummon: false, cost);
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
