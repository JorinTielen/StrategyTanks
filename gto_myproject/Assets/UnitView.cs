using UnityEngine;
using UnityEngine.UI;

public class UnitView : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    private Camera _cam;
    
    private Image _hpImage;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        transform.position = _cam.WorldToScreenPoint(Target.position + Offset);
    }
}
