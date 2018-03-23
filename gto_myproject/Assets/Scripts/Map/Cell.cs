using UnityEngine;

public class Cell : MonoBehaviour
{
    public Selection Selection;
    
    public Point Position { get; private set; }
    public CellType CellType { get; private set; }
    
    public bool Empty { get; private set; }
    public Unit CurrentUnit { get; private set; }

    private CellSelector _cellSelector;

    private void OnEnable()
    {
        Empty = true;
        _cellSelector = GetComponentInChildren<CellSelector>();
    }

    public void Construct(Point pos, CellType type)
    {
        Position = pos;
        CellType = type;
    }
    
    public bool CanMove()
    {
        return Empty && CellType == CellType.GROUND;
    }

    public void Select()
    {
        if (_cellSelector != null) _cellSelector.Select();
        if (CurrentUnit != null) CurrentUnit.Select();
    }

    public void Unselect()
    {
        if (_cellSelector != null) _cellSelector.Unselect();
        if (CurrentUnit != null) CurrentUnit.Unselect();
    }

    public void Place(Unit unit)
    {
        CurrentUnit = unit;
        unit.Position = this;
        Empty = false;
    }

    public void Enter(Unit unit)
    {
        CurrentUnit = unit;
        Empty = false;
    }

    public void Leave()
    {
        CurrentUnit = null;
        Empty = true;
    }
}
