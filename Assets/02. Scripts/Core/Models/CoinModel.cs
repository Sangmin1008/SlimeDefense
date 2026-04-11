using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CoinModel
{
    public ReactiveProperty<int> CurrentCoin { get; }

    public CoinModel(int initialCoin)
    {
        CurrentCoin = new ReactiveProperty<int>(initialCoin);
    }

    public void AddCoin(int amount)
    {
        CurrentCoin.Value += amount;
    }

    public bool TrySpendCoin(int amount)
    {
        if (CurrentCoin.Value >= amount)
        {
            CurrentCoin.Value -= amount;
            return true;
        }
        return false;
    }
}
