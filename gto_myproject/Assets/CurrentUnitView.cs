using UnityEngine;
using UnityEngine.UI;

public class CurrentUnitView : MonoBehaviour
{
    [Header("Dependencies")]
    public Selection Selection;
    public TurnManager TurnManager;
    
    [Header("Properties")]
    public Image ImgUnitPicture;
    public Text TxtUnitName;
    
    private bool active;
    private Unit _currentUnit;

    private void Start()
    {
        gameObject.SetActive(false);
        Selection.OnSelected += Selected;
    }

    public void Selected(Cell selectedCell)
    {
        if (selectedCell.CurrentUnit == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if (selectedCell.CurrentUnit.Player == TurnManager.CurrentPlayer)
        {
            Selection.SelectionMode = SelectionMode.ATTACK;
            gameObject.SetActive(true);
        
            active = true;
            _currentUnit = selectedCell.CurrentUnit;

            TxtUnitName.text = "Soldier Tank";
            ImgUnitPicture.color = _currentUnit.Player.Color;
        }
    }

    public void OnAttackClick()
    {
        //Attack mode
    }
}
