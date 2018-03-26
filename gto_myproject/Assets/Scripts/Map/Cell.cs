using UnityEngine;

public class Cell : MonoBehaviour
{  
    public Point Position { get; private set; }
    public CellType CellType { get; private set; }
    
    public bool Empty { get; private set; }
    public Unit CurrentUnit { get; private set; }

    private Map _map;
    private CellSelector _cellSelector;

    private void OnEnable()
    {
        Empty = true;
        _cellSelector = GetComponentInChildren<CellSelector>();
    }

    public void Construct(Map map, Point pos, CellType type)
    {
        Position = pos;
        CellType = type;

        _map = map;
    }
    
    public bool CanMove()
    {
        return Empty && CellType == CellType.GROUND;
    }

    public void Highlight()
    {
        if (_cellSelector != null) _cellSelector.Select();
    }

    public void Unhighlight()
    {
        if (_cellSelector != null) _cellSelector.Unselect();
    }

    public void Select()
    {
        if (_cellSelector != null) _cellSelector.Select();
        if (CurrentUnit != null) CurrentUnit.Select(_map);
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
