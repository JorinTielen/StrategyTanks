using UnityEngine;

public class CellSelector : MonoBehaviour
{
	public Color Empty, Hover, Selected, Attack;
	
	private MeshRenderer _renderer;
	private bool _selected;
	private bool _mouseOver;

	private void Awake()
	{
		_renderer = GetComponent<MeshRenderer>();
		_renderer.material.color = Empty;
	}

	public bool GetMouseOver()
	{
		return _mouseOver;
	}

	public void Select()
	{
		if (_selected) return;
		_selected = true;
		_renderer.material.color = Selected;
	}

	public void Unselect()
	{
		_selected = false;
		_renderer.material.color = Empty;
	}

	private void OnMouseEnter()
	{
		_mouseOver = true;
		if (_selected)
		{
			_renderer.material.color = Color.yellow;
		}
		else
		{
			_renderer.material.color = Hover;
		}
	}

	private void OnMouseExit()
	{
		_mouseOver = false;
		if (_selected)
		{
			_renderer.material.color = Selected;
		}
		else
		{
			_renderer.material.color = Empty;
		}
	}
}
