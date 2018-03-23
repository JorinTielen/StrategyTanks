using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Cell Position;
    public Player Player;
    public int Range;
    
    public delegate void SelectUnit();
    public event SelectUnit OnSelected;
    
    private bool _isMoving;
    private bool _selected;

    public void Select()
    {
        _selected = true;
        if (OnSelected != null) OnSelected();
    }

    public void Unselect()
    {
        _selected = false;
    }

    public void Move(Cell targetCell)
    {
        if (targetCell.CanMove() && _isMoving == false)
        {
            StartCoroutine(Move(targetCell, 0.6f));			
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
