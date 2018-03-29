using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Unit : MonoBehaviour
{
    [HideInInspector]
    public Cell Position;
    [HideInInspector]
    public Player Player;

    [Header("Max Stats")]
    public string UnitName;
    public int MaxRange;
    public int MaxHealth;
    public int Damage;

    [Header("Rotation Anchors")]
    public Transform TankRotationAnchor;
    public Transform GunRotationAnchor;
    
    public delegate void HealthEvent();
    public event HealthEvent OnHealthChanged;

    //Current variables
    private int _currentRange;
    private int _currentHealth;
    private bool _canAttack;
    private List<Cell> _selectedCells;
    private bool _isMoving;
    private bool _selected;

    private void Start()
    {
        _currentHealth = MaxHealth;
    }

    public void StartTurn()
    {
        _currentRange = MaxRange;
        _canAttack = true;
    }

    public void Select(ICellRange range)
    {
        if (!_selected)
        {
            _selected = true;
            _selectedCells = range.GetCellsInRange(Position, _currentRange, false);
            foreach (var cell in _selectedCells)
            {
                cell.Highlight();
            }
        }
    }

    public void Unselect()
    {
        if (_selected)
        {
            foreach (var cell in _selectedCells)
            {
                cell.Unhighlight();
            }

            _selectedCells = null;
            _selected = false;
        }
    }

    public void AttackMode(ICellRange range)
    {
        if (_canAttack)
        {
            if (_selectedCells != null)
            {
                foreach (var cell in _selectedCells)
                {
                    cell.Unhighlight();
                }
            }
        
            _selectedCells = range.GetCellsInRange(Position, MaxRange, true);
            foreach (var cell in _selectedCells)
            {
                cell.Highlight();
            }
        }
    }
    
    public void UnattackMode()
    {
        if (_selectedCells != null)
        {
            foreach (var cell in _selectedCells)
            {
                cell.Unhighlight();
            }
        }
    }
    
    public void Attack(Unit u, ICellRange range)
    {
        if (_canAttack && u.Player != Player)
        {
            if (range.GetCellsInRange(Position, MaxRange, true).Contains(u.Position))
            {
                u.DoDamage(Damage);
                _canAttack = false;
                _currentRange = 0;
            }
        }
    }

    public void DoDamage(int amount)
    {
        if (amount >= _currentHealth)
        {
            _currentHealth = 0;
            Debug.Log("ded");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("ooff");
            _currentHealth -= amount;
            if (OnHealthChanged != null) OnHealthChanged();
        }
    }

    public int GetHealth()
    {
        return _currentHealth;
    }

    public void Move(ICellRange range, Cell targetCell)
    {
        var cellsInRange = range.GetCellsInRange(Position, _currentRange, false);
        foreach (var cell in cellsInRange)
        {
            if (cell.Position.Equals(targetCell.Position))
            {
                if (targetCell.CanMove() && _isMoving == false)
                {
                    var path = GetComponent<Pathfinder>().FindPath(Position, targetCell);
                    if (path == null) return;

                    _currentRange -= path.Count;
                    StartCoroutine(Move(path, 0.6f));
                    return;
                }
            }
        } 
    }
    
    private IEnumerator Move(List<Node> path, float time)
    {
        _isMoving = true;

        //Loop through all the tiles in the path
        for (int i = 0; i < path.Count; i++)
        {
            var elapsedTime = 0.0f;
            var targetCell = path[i].WorldCell;
            
            //First rotate towards target
            Quaternion neededRotation = Quaternion.LookRotation(targetCell.transform.position - transform.position);
            while (TankRotationAnchor.rotation != neededRotation && elapsedTime / 3 < time)
            {
                TankRotationAnchor.rotation = Quaternion.RotateTowards(TankRotationAnchor.rotation, neededRotation, 250f * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        
            //Then start moving
            elapsedTime = 0.0f;
            var startPos = transform.position;
            var endPos = targetCell.transform.position;

            while (elapsedTime < time)
            {
                transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            SetPosition(targetCell);
        }

        _isMoving = false;
    }
    
    private void SetPosition(Cell cell)
    {
        Position.Leave();
        transform.parent = cell.gameObject.transform;
        Position = cell;
        Position.Enter(this);
    }
}
