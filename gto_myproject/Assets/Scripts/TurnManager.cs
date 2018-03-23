﻿using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Player> Players;
	public Player CurrentPlayer;

	private int _currentIndex;
	
	private void Start()
	{
		_currentIndex = 0;
		CurrentPlayer = Players[_currentIndex];
		
		//Start up every player
		foreach (var p in Players)
		{
			p.StartGame();
		}
		
		CurrentPlayer.StartTurn();
	}

	public void NextTurn()
	{
		CurrentPlayer.EndTurn();
		_currentIndex++;
		if (_currentIndex >= Players.Count)
		{
			_currentIndex = 0;
		}
		
		CurrentPlayer = Players[_currentIndex];
		CurrentPlayer.StartTurn();
	}

	public void PlayerMoveUnit(Unit u, Cell targetCell)
	{
		if (u.Player == CurrentPlayer)
		{
			u.Move(targetCell);
		}
	}
}
