using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    [SerializeField] private Button[] stageButtons;
    
    private Subject<int> _onStageSelected = new Subject<int>();
    public IObservable<int> OnStageSelected => _onStageSelected;

    private void Start()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i;
            stageButtons[i].onClick.AddListener((() =>
            {
                _onStageSelected.OnNext(index);
                Debug.Log($"{index}버튼 누름!");
            }));
        }
    }
}
