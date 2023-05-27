using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheelAnimation : MonoBehaviour
{
    public Sprite[] spriteOrder;
    public SpriteRenderer[] spWheels;

    float angles;
    int length, wheelIndex, spriteIndex;

    private void Awake()
    {
        length = spriteOrder.Length;
        angles = 360f / (float)length;
    }

    public void UpdateWheel(int _positionIndex, int _isLeft, Quaternion _rotation)
    {
        if (_positionIndex == 1)
            return;

        wheelIndex = _positionIndex * 2 + _isLeft;
        spriteIndex = Mathf.RoundToInt(_rotation.eulerAngles.x / (float)angles) % length;
        spWheels[wheelIndex].sprite = spriteOrder[spriteIndex];
    }
}
