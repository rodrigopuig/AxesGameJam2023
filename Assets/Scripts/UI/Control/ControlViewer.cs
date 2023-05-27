using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ActionID = CustomAction.ActionID;

public class ControlViewer : MonoBehaviour
{
    public RectTransform slotBackground;
    public UIKeyCap keycapPrefab;

    [Header("Slots")]
    public RectTransform slotParent;

    [Header("Components")]
    public InputController inputController;
    public VerticalLayoutGroup verticalLayout;

    public Dictionary<ActionID, UIKeyCap[]> idKeycapPair;

    private IEnumerator Start()
    {
        idKeycapPair = new Dictionary<ActionID, UIKeyCap[]>();

        CustomAction[] _actions = inputController.GetActions();
        int _length = _actions.Length;
        for (int i = 0; i < _length; i++)
        {
            _actions[i].ConfigureView(i, this);
        }

        yield return null;

        verticalLayout.enabled = false;
        verticalLayout.enabled = true;
    }

    public void FillKeycapPerId(int _actionId, TDAction _action)
    {
        ActionID _id = (ActionID)_actionId;

        if (!idKeycapPair.ContainsKey(_id))
            idKeycapPair[_id] = new UIKeyCap[2];

        TDAction _tdAction = (TDAction)_action;
        KeyCode _left = _tdAction.GetLeft(),
            _right = _tdAction.GetRight();


        RectTransform _keyBackground = Instantiate(slotBackground, slotParent);
        ConfigureKey(_left, _id, 0, _keyBackground);
        ConfigureKey(_left, _id, 1, _keyBackground);
    }

    public void FillKeycapPerId(int _actionId, PatternAction _action)
    {
        ActionID _id = (ActionID)_actionId;

        PatternAction _patternAction = (PatternAction)_action;
        KeyCode[] _keys = _patternAction.GetPattern();

        RectTransform _keyBackground = Instantiate(slotBackground, slotParent);
        int _length = _keys.Length;

        if (!idKeycapPair.ContainsKey(_id))
            idKeycapPair[_id] = new UIKeyCap[_length];

        for (int i = 0; i < _length; i++)
        {
            ConfigureKey(_keys[i], _id, i, _keyBackground);
        }
    }

    void ConfigureKey(KeyCode _key, ActionID _actionId, int _index, Transform _parent)
    {
        UIKeyCap _keycap = Instantiate(keycapPrefab, _parent);
        _keycap.SetKey(_key);

        idKeycapPair[_actionId][_index] = _keycap;
    }
}
