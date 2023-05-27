using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ActionID = CustomAction.ActionID;

public class ControlViewer : MonoBehaviour
{
    public RectTransform slotBackground;
    public UIKeyCap keycapPrefab;
    
    [Header("Slots")]
    public RectTransform slotParent;

    [Header("Components")]
    public InputController inputController;

    public Dictionary<ActionID, UIKeyCap> idKeycapPair;

    private void Awake()
    {
        idKeycapPair = new Dictionary<ActionID, UIKeyCap>();

        
    }

    public void FillKeycapPerId(TDAction _action)
    {
            TDAction _tdAction = (TDAction)_action;
            KeyCode _left = _tdAction.GetLeft(),
                _right = _tdAction.GetRight();
        ConfigureKey(_left, transform);
        ConfigureKey(_left, transform);
    }

    public void FillKeycapPerId(PatternAction _action)
    {
       
            PatternAction _patternAction = (PatternAction)_action;
            KeyCode[] _keys = _patternAction.GetPattern();

            int _length = _keys.Length;
            for (int i = 0; i < _length; i++)
            {
                ConfigureKey(_keys[i], transform);
            }
    }

    void ConfigureKey(KeyCode _key, Transform _parent)
    {
        UIKeyCap _keycap = Instantiate(keycapPrefab, _parent);
        _keycap.SetKey(_key);
    }
}
