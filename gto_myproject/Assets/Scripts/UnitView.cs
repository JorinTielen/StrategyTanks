using UnityEngine;
using UnityEngine.UI;

public class UnitView : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    
    private Camera _cam;
    private Unit _unit;
    
    public Image HpImage;
    public Image TeamColorImage;

    private void Start()
    {
        _cam = Camera.main;
        _unit = Target.gameObject.GetComponent<Unit>();
        _unit.OnHealthChanged += OnHealthChanged;

        TeamColorImage.color = _unit.Player.Color;
    }

    private void Update()
    {
        transform.position = _cam.WorldToScreenPoint(Target.position + Offset);
    }

    private void OnHealthChanged()
    {
        HpImage.fillAmount = (float)_unit.GetHealth() / _unit.MaxHealth;
    }
}
