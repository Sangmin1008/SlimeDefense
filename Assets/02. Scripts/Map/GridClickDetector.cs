using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridClickDetector : MonoBehaviour, IPointerClickHandler
{
    private Tilemap _tilemap;

    public event Action<Vector3Int, Vector3> OnGridClicked;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        GetComponent<TilemapRenderer>().enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 worldPos = eventData.pointerCurrentRaycast.worldPosition;
        Vector3Int cellPos = _tilemap.WorldToCell(worldPos);

        if (_tilemap.HasTile(cellPos))
        {
            Vector3 centerWorldPos = _tilemap.GetCellCenterWorld(cellPos);
            OnGridClicked?.Invoke(cellPos, centerWorldPos);
        }
    }
}
