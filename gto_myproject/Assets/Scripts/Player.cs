using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Color Color;
	public UnitFactory SniperFactory;
	public UnitFactory SoldierFactory;
	public Map Map;
	
	public delegate void GameOverEvent();
	public event GameOverEvent OnGameOver;
	
	private List<Unit> _units = new List<Unit>();
	private int _playerId;

	public void StartGame(int playerId)
	{
		_playerId = playerId;
		_units.Add(SniperFactory.BuildUnit(Map.GetRandomStartingPosition(_playerId), this));
		_units.Add(SoldierFactory.BuildUnit(Map.GetRandomStartingPosition(_playerId), this));
		gameObject.SetActive(false);
	}

	public void MoveUnit(Unit unit, Cell targetCell)
	{
		unit.Move(Map, targetCell);
	}
	
	public void StartTurn()
	{
		gameObject.SetActive(true);
		foreach (var unit in _units)
		{
			unit.StartTurn();
		}
	}

	public void EndTurn()
	{
		gameObject.SetActive(false);
	}

	public void RemoveUnit(Unit u)
	{
		_units.Remove(u);
	}

	public void CheckGameOver()
	{
		if (_units.Count == 0)
		{
			if (OnGameOver != null) OnGameOver();
		}
	}
}