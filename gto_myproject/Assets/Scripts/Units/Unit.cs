using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Unit : MonoBehaviour
{
    public Cell Position;
    public Player Player;
    public int MaxRange;

    private int _currentRange;
    
    public delegate void SelectUnit();
    public event SelectUnit OnSelected;

    private List<Cell> _selectedCells;
    private bool _isMoving;
    private bool _selected;

    public void StartTurn()
    {
        _currentRange = MaxRange;
    }

    public void Select(ICellRange range)
    {
        if (!_selected)
        {
            _selected = true;
            _selectedCells = range.GetCellsInRange(Position, _currentRange);
            foreach (var cell in _selectedCells)
            {
                cell.Highlight();
            }
        
            if (OnSelected != null) OnSelected();
        }
    }

    public void Unselect()
    {
        if (_selected)
        {
            foreach (var cell in _selectedCells)
            {
                cell.Unhighlight();
            }

            _selectedCells = null;
            _selected = false;
        }
    }

    public void Move(ICellRange range, Cell targetCell)
    {
        var cellsInRange = range.GetCellsInRange(Position, _currentRange);
        foreach (var cell in cellsInRange)
        {
            if (cell.Position.Equals(targetCell.Position))
            {
                if (targetCell.CanMove() && _isMoving == false)
                {
                    var path = GetComponent<Pathfinder>().FindPath(Position, targetCell);
                    _currentRange -= path.Count;
                    
                    StartCoroutine(Move(targetCell, 0.6f));
                    return;
                }
            }
        } 
    }
    
    private IEnumerator Move(Cell targetCell, float time)
    {
        _isMoving = true;
        var elapsedTime = 0.0f;
        var startPos = transform.position;
        var endPos = targetCell.transform.position;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
		
        SetPosition(targetCell);
        _isMoving = false;
    }
    
    private void SetPosition(Cell cell)
    {
        Position.Leave();
        Position = cell;
        Position.Enter(this);
    }
}
