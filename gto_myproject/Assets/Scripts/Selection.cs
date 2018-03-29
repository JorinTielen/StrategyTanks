using System.Collections;
using UnityEngine;

public class Selection : MonoBehaviour
{
	public delegate void SelectEvent(Cell cell);
	public event SelectEvent OnSelected;

	public TurnManager TurnManager;

	public SelectionMode SelectionMode = SelectionMode.SELECT;
	
	private Cell _previousSelection;
	
	private void Update() 
	{
		if (Input.GetMouseButtonDown(0))
		{
			LeftMouseClick();
		}
		else if (Input.GetMouseButtonDown(1))
		{
			RightMouseClick();
		}
	}

	private void LeftMouseClick()
	{
		if (_previousSelection != null)
		{
			_previousSelection.Unselect();
		}

		if (SelectionMode == SelectionMode.SELECT)
		{
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform != null)
				{
					Select(hit.transform.gameObject.GetComponentInParent<Cell>());
				}
			}
		}
	}

	private void RightMouseClick()
	{
		if (_previousSelection == null) return;

		if (_previousSelection.CurrentUnit == null) return;

		if (SelectionMode == SelectionMode.SELECT)
		{
			_previousSelection.Unselect();
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform != null)
				{
					var targetCell = hit.transform.gameObject.GetComponentInParent<Cell>();
					TurnManager.PlayerMoveUnit(_previousSelection.CurrentUnit, targetCell);
					Select(targetCell);
				}
			}
		} 
		else if (SelectionMode == SelectionMode.ATTACK)
		{
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform != null)
				{
					Cell c = hit.transform.gameObject.GetComponentInParent<Cell>();

					if (c.CurrentUnit != null && c.CurrentUnit.Player != TurnManager.CurrentPlayer)
					{
						if (_previousSelection == null || _previousSelection.CurrentUnit == null) return;
						
						_previousSelection.CurrentUnit.Attack(c.CurrentUnit);
					}
				}
				
				SelectionMode = SelectionMode.SELECT;
			}
		}
	}

	private void Select(Cell cell)
	{
		cell.Select();
		_previousSelection = cell;

		if (OnSelected != null) OnSelected(cell);
	}
}
