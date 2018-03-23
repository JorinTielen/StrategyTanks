using UnityEngine;

public class UnitFactory : MonoBehaviour
{
	public Unit UnitPrefab;

	public Unit BuildUnit(Cell cell, Player player)
	{
		var u = Instantiate(UnitPrefab, cell.transform);
		cell.Place(u);
		u.Player = player;
		return u;
	}
}
