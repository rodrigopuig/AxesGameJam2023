using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAction
{
    public enum Action { TurnWheel_Left, Accelerate }//TurnWheel_Right, }

    public System.Action<float> onUpdate;

    public virtual void Update(float _deltaTime) {}
}
