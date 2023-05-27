using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ranges goes from -1 to 1
/// </summary>
[System.Serializable]
public class TDAction : CustomAction
{
    public KeyCode leftKey, rightKey;

    private float counter;
    public float speed;
    public float recoverSpeed;

    private bool keysHaveBeenPressed;

    public TDAction Initialize()
    {
        // leftKey = _leftKey;
        // rightKey = _rightKey;

        counter = 0.5f;
        // speed = _speed;
        // recoverSpeed = _recoverSpeed;
        return this;
    }

    public override void Update(float _deltaTime)
    {
        keysHaveBeenPressed = false;

        if (Input.GetKey(leftKey))
        {
            keysHaveBeenPressed = true;
            counter -= _deltaTime * speed;
        }

        if (Input.GetKey(rightKey))
        {
            keysHaveBeenPressed = true;
            counter += _deltaTime * speed;
        }

        counter = Mathf.Clamp(counter, 0, 1);

        if (keysHaveBeenPressed)
        {
            //de momento nada
        }
        else
        {
            if (counter > 0.5f)
            {
                counter -= _deltaTime * recoverSpeed;

                if (counter < 0.5f)
                    counter = 0.5f;
            }
            else
            {
                counter += _deltaTime * recoverSpeed;

                if (counter > 0.5f)
                    counter = 0.5f;
            }
        }

        onUpdate?.Invoke(counter);
    }

    public override void ConfigureView(int _actionId, ControlViewer _controlViewer)
    {
        _controlViewer.FillKeycapPerId(_actionId, this);
    }

    public KeyCode GetLeft() { return leftKey; }
    public KeyCode GetRight() { return rightKey; }
}
