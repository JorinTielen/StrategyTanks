using UnityEngine;
using UnityEngine.UI;

public class CurrentUnitView : MonoBehaviour
{
    [Header("Dependencies")]
    public Selection Selection;
    public Player Player;

    [Header("Own components")]
    public Image HealthBar;
    public Text UnitName;
    public Image UnitImage;
    public Button Attack;

    private Unit _selectedUnit;
    
    private void Start()
    {
        gameObject.SetActive(false);
        Attack.gameObject.SetActive(false);
        
        Selection.OnSelected += OnSelectedUnit;
        Selection.OnUnselected += OnUnselectUnit;
    }

    private void SetInfo(Unit unit)
    {
        if (unit.Player == Player) Attack.gameObject.SetActive(true);
        
        HealthBar.fillAmount = (float)unit.GetHealth() / unit.MaxHealth;
        UnitName.text = unit.UnitName;
        UnitImage.color = unit.Player.Color;
    }

    private void OnSelectedUnit(Cell selectedCell)
    {
        if (selectedCell.CurrentUnit == null) return;
        gameObject.SetActive(true);
        
        var unit = selectedCell.CurrentUnit;
        SetInfo(unit);
        _selectedUnit = unit;
    }

    private void OnUnselectUnit()
    {
        _selectedUnit = null;
        gameObject.SetActive(false);
        Attack.gameObject.SetActive(false);
    }

    public void AttackBtn()
    {
        if (_selectedUnit == null) return;
        
        _selectedUnit.AttackMode(_selectedUnit.Position.GetMap());
    }
}
