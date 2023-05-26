using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ranges goes from -1 to 1
/// </summary>
public class TDAction : CustomAction
{
    KeyCode leftKey, rightKey;

    float counter,
         speed,
         recoverSpeed;

    bool keysHaveBeenPressed;

    public TDAction(KeyCode _leftKey, KeyCode _rightKey, float _speed, float _recoverSpeed)
    {
        leftKey = _leftKey;
        rightKey = _rightKey;

        counter = 0.5f;
        speed = _speed;
        recoverSpeed = _recoverSpeed;
    }

    public override void Update(float _deltaTime)
    {
        keysHaveBeenPressed = false;

        if(Input.GetKey(leftKey))
        {
            keysHaveBeenPressed = true;
            counter -= _deltaTime * speed;
        }

        if(Input.GetKey(rightKey))
        {
            keysHaveBeenPressed = true;
            counter += _deltaTime * speed;
        }

        counter = Mathf.Clamp(counter, 0, 1);

        if(keysHaveBeenPressed)
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
}
