using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

using ActionID = CustomAction.ActionID;

public class InputController : MonoBehaviour
{
    // public TextMeshProUGUI leftRight;
    // public TextMeshProUGUI accelerate;

    CustomAction[] actions;

    public TDAction leftWheelAction;
    public TDAction rightWheelAction;
    public PatternAction motorAction;
    public TDAction brakeAction;

    private void Awake()
    {
        actions = new CustomAction[]
        {
            leftWheelAction.Initialize(),
            rightWheelAction.Initialize(),
            motorAction.Initialize(),
            brakeAction.Initialize()
        };

        // actions[(int)ActionID.TurnWheel_Left].onUpdate += (val) => leftRight.text = $"left_right: {val.ToString("0.00")}";
        // actions[(int)ActionID.Accelerate].onUpdate += (val) => accelerate.text = $"accelerate: {val.ToString("0.00")}";
    }

    public void Update()
    {
        float _deltaTime = Time.deltaTime;
        for(int i  = 0; i<actions.Length; i++)
        {
            actions[i].Update(_deltaTime);
        }
    }

    public void SetListener(ActionID _actionID, Action<float> onUpdate)
    {
        actions[(int)_actionID].onUpdate += onUpdate;
    }

    public CustomAction[] GetActions() { return actions; }
}
