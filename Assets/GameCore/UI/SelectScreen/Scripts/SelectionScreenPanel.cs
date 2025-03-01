using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScreenPanel : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private SelectButtonsID _ID;

    [SerializeField] SelectionPanelAnimations _animations;
    private bool _selected;

    public event Action<SelectionScreenPanel> OnSelected;

    public SelectButtonsID ID { get { return _ID; } }

    private void Awake()
    {
        _button.onClick.AddListener(Select);
    }
    public void Select()
    {
        if (_selected == true)
            return;

        _animations.Select();
        _selected = true;
        OnSelected?.Invoke(this);
    }
    public void Unselect()
    {
        if (_selected == false)
            return;
        _animations.Unselect();
        _selected = false;
    }
}
