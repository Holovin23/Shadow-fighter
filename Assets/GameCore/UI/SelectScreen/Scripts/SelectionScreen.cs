using UnityEngine;
using System;

public class SelectionScreen : MonoBehaviour
{
    [SerializeField] private SelectionScreenPanel[] _panels;
    [SerializeField] private SelectionScreenPanel _onStartSelected;

    private SelectionScreenPanel _currentPanel;

    public event Action<SelectButtonsID> OnSelectedButtonChanged;

    private void Awake()
    {
        Sub();
        SelectOnStart();
    }
    private void OnDestroy()
    {
        Unsub();
    }
    private void Sub()
    {
        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].OnSelected += EnterPanel;
        }
    }
    private void Unsub()
    {
        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].OnSelected -= EnterPanel;
        }
    }
    private void SelectOnStart()
    {
        _onStartSelected.Select();
    }
    private void EnterPanel(SelectionScreenPanel panel)
    {
        _currentPanel?.Unselect();
        _currentPanel = panel;
        OnSelectedButtonChanged?.Invoke(panel.ID);
    }
}
