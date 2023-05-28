using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ActionID = CustomAction.ActionID;

using DG.Tweening;

public class ControlViewer : MonoBehaviour
{
    public KeyGroup[] keyGroups;

    InputController inputController;
    Dictionary<ActionID, UIKeyCap[]> idKeycapPair;
    
    private void Awake()
    {
      
    }

    private void Start()
    {
        inputController = FindObjectOfType<InputController>();

        idKeycapPair = new Dictionary<ActionID, UIKeyCap[]>()
        {
            [ActionID.TurnWheel_Left] = new UIKeyCap[2] { keyGroups[0].key1, keyGroups[0].key2 },
            [ActionID.TurnWheel_Right] = new UIKeyCap[2] { keyGroups[1].key1, keyGroups[1].key2 },
            [ActionID.Accelerate] = new UIKeyCap[2] { keyGroups[2].key1, keyGroups[2].key2 },
            [ActionID.Brake] = new UIKeyCap[2] { keyGroups[3].key1, keyGroups[3].key2 }
        };

        CustomAction[] _actions = inputController.GetActions();
        int _length = _actions.Length;
        for (int i = 0; i < _length; i++)
        {
            _actions[i].ConfigureView(i, this);
        }
    }

    public void FillKeycapPerId(int _actionId, TDAction _action)
    {
        ActionID _id = (ActionID)_actionId;


        TDAction _tdAction = (TDAction)_action;
        _tdAction.onLeftPressed = (isOn) => { if (isOn == 1) idKeycapPair[_id][0].SetPressedColor(); else idKeycapPair[_id][0].SetNormalColor(); };
        _tdAction.onRightPressed = (isOn) => { if (isOn == 1) idKeycapPair[_id][1].SetPressedColor(); else idKeycapPair[_id][1].SetNormalColor(); };
        KeyCode _left = _tdAction.GetLeft(),
            _right = _tdAction.GetRight();

        LeftRightSlider _slider = keyGroups[_actionId].lrSlider;
        _action.onUpdate += _slider.SetSlidervalue;

        ConfigureKey(_left, _id, 0);
        ConfigureKey(_right, _id, 1);
    }

    public void FillKeycapPerId(int _actionId, PatternAction _action)
    {
        ActionID _id = (ActionID)_actionId;

        PatternAction _patternAction = (PatternAction)_action;
        _patternAction.onFail += () => { for (int i = 0; i < idKeycapPair[_id].Length; i++) idKeycapPair[_id][i].SetNormalColor(); };
        _patternAction.onKeyAddedToPattern += (value) => idKeycapPair[_id][value].SetPressedColor();
        _patternAction.onComplete += () => {

            for (int i = 0; i < idKeycapPair[_id].Length; i++)
                idKeycapPair[_id][i].SetPressedColor();
            float _a = 0;
            DOTween.To(() => _a, x => _a = x, 1, 0.1f).OnComplete(() =>
            {
                for (int i = 0; i < idKeycapPair[_id].Length; i++)
                    idKeycapPair[_id][i].SetNormalColor();
            });
            //for (int i = 0; i < idKeycapPair[_id].Length; i++)
            //    idKeycapPair[_id][i].SetNormalColor();
        };

        KeyCode[] _keys = _patternAction.GetPattern();

        NormalSlider _slider = keyGroups[_actionId].normalSlider;
        _action.onUpdate += _slider.SetSlidervalue;

        int _length = _keys.Length;


        for (int i = 0; i < _length; i++)
        {
            ConfigureKey(_keys[i], _id, i);
        }
    }

    void ConfigureKey(KeyCode _key, ActionID _actionId, int _index)
    {
        idKeycapPair[_actionId][_index].SetKey(_key);
    }


}
