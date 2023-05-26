using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

using Action = CustomAction.Action;

public class InputController : MonoBehaviour
{
    public TextMeshProUGUI leftRight;
    public TextMeshProUGUI accelerate;

    CustomAction[] actions;

    private void Awake()
    {
        actions = new CustomAction[]
        {
            new TDAction(KeyCode.A, KeyCode.D, 1, 1),
            new PatternAction(new KeyCode[]{ KeyCode.Q, KeyCode.W}, 0.2f, 1, 0.5f)
        };

        actions[(int)Action.TurnWheel_Left].onUpdate += (val) => leftRight.text = $"left_right: {val.ToString("0.00")}";
        actions[(int)Action.Accelerate].onUpdate += (val) => accelerate.text = $"accelerate: {val.ToString("0.00")}";
    }

    public void Update()
    {
        float _deltaTime = Time.deltaTime;
        for(int i  = 0; i<actions.Length; i++)
        {
            actions[i].Update(_deltaTime);
        }
    }
}