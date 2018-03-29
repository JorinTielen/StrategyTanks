using System.Collections;
using UnityEngine;

public class Selection : MonoBehaviour
{
	public delegate void SelectEvent();
	public event SelectEvent OnSelected;

	public TurnManager TurnManager;
	
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
		RaycastHit hit;
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (_previousSelection == null)
		{
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform != null)
				{
					Select(hit.transform.gameObject.GetComponentInParent<Cell>());
				}
			}
		}
		else
		{
			if (_previousSelection.CurrentUnit != null)
			{
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.transform != null)
					{
						var targetCell = hit.transform.gameObject.GetComponentInParent<Cell>();
						TurnManager.PlayerMoveUnit(_previousSelection.CurrentUnit, targetCell);
						Unselect();
						Select(targetCell);
					}
				}
			}
			
			Unselect();
		}
	}

	private void RightMouseClick()
	{
		if (_previousSelection == null || _previousSelection.CurrentUnit == null) return;
		
		RaycastHit hit;
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform != null)
			{
				Cell c = hit.transform.gameObject.GetComponentInParent<Cell>();
				if (c.CurrentUnit == null) return;
				
				_previousSelection.CurrentUnit.Attack(c.CurrentUnit);
				Unselect();
			}
		}
	}

	private void Select(Cell cell)
	{
		cell.Select();
		_previousSelection = cell;

		if (OnSelected != null) OnSelected();
	}

	private void Unselect()
	{
		if (_previousSelection == null) return;
		
		_previousSelection.Unselect();
		_previousSelection = null;
	}
}
