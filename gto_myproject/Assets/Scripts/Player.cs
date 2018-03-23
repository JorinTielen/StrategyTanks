using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Color Color;
	public UnitFactory SniperFactory;
	public Map Map;
	
	private List<Unit> _units = new List<Unit>();

	public void StartGame()
	{
		_units.Add(SniperFactory.BuildUnit(Map.GetRandomFreeCell(), this));
		gameObject.SetActive(false);
	}
	
	public void StartTurn()
	{
		gameObject.SetActive(true);
	}

	public void EndTurn()
	{
		gameObject.SetActive(false);
	}
}