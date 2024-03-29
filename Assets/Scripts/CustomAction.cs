using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAction
{
    public enum ActionID { TurnWheel_Left, TurnWheel_Right, Accelerate, Brake }

    public System.Action<float> onUpdate;

    public virtual void Update(float _deltaTime) {}

    //utils
    public virtual void ConfigureView(int _actionId, ControlViewer _controlViewer)
    {
        
    }

    public T Cast<T>() where T : CustomAction { return (T)this; }
}
