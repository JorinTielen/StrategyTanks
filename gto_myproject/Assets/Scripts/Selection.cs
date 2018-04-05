using System.Collections;
using UnityEngine;

public class Selection : MonoBehaviour
{
	public delegate void SelectEvent(Cell cell);
	public event SelectEvent OnSelected;
	
	public delegate void UnselectEvent();
	public event UnselectEvent OnUnselected;

	public TurnManager TurnManager;

	private bool _disabled = false;
	
	//Moveselection
	private Cell _previousSelection;
	
	//AttackSelection
	private bool _attackMode;
	private Unit _attackUnit;

	public void Disable()
	{
		_disabled = true;
	}
	
	private void Update()
	{
		if (_disabled) return;
		
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

		if (_attackMode)
		{
			UnattackMode();
		}
		
		if (_previousSelection == null)
		{
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform != null)
				{
					Cell c = hit.transform.gameObject.GetComponentInParent<Cell>();
					if (c.CurrentUnit != null && c.CurrentUnit.Player == TurnManager.CurrentPlayer)
					{
						Select(c);
					}
				}
			}
		}
		else
		{
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform != null)
				{
					var targetCell = hit.transform.gameObject.GetComponentInParent<Cell>();
					
					if (_previousSelection.CurrentUnit != null)
					{
						TurnManager.PlayerMoveUnit(_previousSelection.CurrentUnit, targetCell);
					}
					
					Unselect();
					Select(targetCell);
				}
			}
			else
			{
				Unselect();
			}
		}
	}

	private void RightMouseClick()
	{
		Unselect();
		
		RaycastHit hit;
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform != null)
			{
				if (_attackMode && _attackUnit != null)
				{
					Cell c = hit.transform.gameObject.GetComponentInParent<Cell>();
					if (c.CurrentUnit == null) return;
				
					_attackUnit.Attack(c.CurrentUnit, TurnManager.CurrentPlayer.Map);
					UnattackMode();
				}
				else
				{
					Cell c = hit.transform.gameObject.GetComponentInParent<Cell>();
					if (c.CurrentUnit == null) return;

					AttackMode(c);
				}
			}
		}
	}

	public void AttackMode(Cell c)
	{
		_attackMode = true;
		
		//TODO: WOW fix this maybe
		c.CurrentUnit.AttackMode(TurnManager.CurrentPlayer.Map);
		_attackUnit = c.CurrentUnit;
	}

	public void UnattackMode()
	{
		_attackMode = false;
		_attackUnit.UnattackMode();
	}

	private void Select(Cell cell)
	{
		cell.Select();
		_previousSelection = cell;

		if (OnSelected != null) OnSelected(cell);
	}

	private void Unselect()
	{
		if (_previousSelection == null) return;

		if (_attackMode)
		{
			UnattackMode();
		}
		
		_previousSelection.Unselect();
		_previousSelection = null;
		
		if (OnUnselected != null) OnUnselected();
	}
}
