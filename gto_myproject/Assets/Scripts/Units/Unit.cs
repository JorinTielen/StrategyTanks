using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Compatibility;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Unit : MonoBehaviour
{
    [HideInInspector]
    public Cell Position;
    [HideInInspector]
    public Player Player;

    [Header("Dependencies")]
    public Bullet BulletPrefab;
    public GameObject ExplosionEffect;

    [Header("Max Stats")]
    public string UnitName;
    public UnitType UnitType;
    public int MaxRange;
    public int MaxHealth;
    public int Damage;

    [Header("Anchors")]
    public Transform TankRotationAnchor;
    public Transform GunRotationAnchor;
    public Transform FirePoint;
    
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
        
            //Please don't look at this
            if (UnitType == UnitType.SOLDIER)
            {
                _selectedCells = range.GetCellsInRange(Position, MaxRange, true);
            }
            else if (UnitType == UnitType.SNIPER)
            {
                _selectedCells = range.GetCellsInLineRange(Position);
            }
            
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
            List<Cell> cellsInRange = null;
            if (UnitType == UnitType.SOLDIER)
            {
                cellsInRange = range.GetCellsInRange(Position, MaxRange, true);
            }
            else if (UnitType == UnitType.SNIPER)
            {
                cellsInRange = range.GetCellsInLineRange(Position);
            }
            if (cellsInRange.Contains(u.Position))
            {
                StartCoroutine(Attack(u, 0.6f));
                _canAttack = false;
                _currentRange = 0;
            }
        }
    }

    private IEnumerator Attack(Unit u, float speed)
    {
        var elapsedTime = 0.0f;
        
        Quaternion neededRotation = Quaternion.LookRotation(u.transform.position - transform.position);
        while (GunRotationAnchor.rotation != neededRotation && elapsedTime / 3 < speed)
        {
            GunRotationAnchor.rotation = Quaternion.RotateTowards(GunRotationAnchor.rotation, neededRotation, 250f * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Shoot(u.transform, Damage);
    }

    private void Shoot(Transform target, int damage)
    {
        Bullet b = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        b.Seek(target, damage);
    }

    public void DoDamage(int amount)
    {
        if (amount >= _currentHealth)
        {
            _currentHealth = 0;
            Debug.Log("ded");
            GameObject effect = Instantiate(ExplosionEffect, transform.position, transform.rotation);
            
            Player.RemoveUnit(this);
            Position.Leave();
            Destroy(effect, 5f);
            Destroy(gameObject);

            Player.CheckGameOver();
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
