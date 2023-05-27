using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBrakeAnimation : MonoBehaviour
{
    public Quaternion left_opened, left_closed;
    public Quaternion right_opened, right_closed;
    public Transform[] mobileParts;//0 left 1 right

    public void UpdateParts(bool _left, bool _right)
    {
        if (_left)
            mobileParts[0].transform.localRotation = left_closed;
        else if(!_left)
            mobileParts[0].transform.localRotation = left_opened;

        if (_right)
            mobileParts[1].transform.localRotation = right_closed;
        else if (!_right)
            mobileParts[1].transform.localRotation = right_opened;
    }
}
