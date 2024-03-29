using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatternAction : CustomAction
{
    public System.Action<int> onKeyAddedToPattern;
    public System.Action onComplete;
    public System.Action onFail;

    public KeyCode[] pattern;
    KeyCode[] buffer;

    public float recoverTime;
    float recoverCounter;

    public float variantPerCompletedPattern;
    public float recoverSpeed; //velocidad de recuperacion despues de X tiempo sin input
    float variationCounter; //increases after completing a combo

    int patternCounter,
        patternSize;

    public PatternAction Initialize()
    {
        recoverCounter = 0;
        variationCounter = 0;

        patternSize = pattern.Length;
        buffer = new KeyCode[patternSize];

        return this;
    }

    public override void Update(float _deltaTime)
    {
        bool _found = false;
        for(int i = 0; i<patternSize && !_found; i++)
        {
            if (Input.GetKeyDown(pattern[i]))
            {
                buffer[patternCounter++] = pattern[i];

                if (buffer[patternCounter-1] == pattern[patternCounter-1])
                {
                    _found = true;
                    recoverCounter = 0;

                    onKeyAddedToPattern?.Invoke(patternCounter - 1);

                    if (patternCounter >= patternSize)
                    {
                        patternCounter = 0;
                        CompletePattern();
                    }
                }
                else
                {
                    ResetCounter();
                }
            }
        }

            recoverCounter += _deltaTime;

            if(recoverCounter > recoverTime)
            {
                ResetCounter();

                variationCounter = Mathf.Clamp01(variationCounter - _deltaTime * recoverSpeed);
                onUpdate?.Invoke(variationCounter);
            }

        onUpdate?.Invoke(variationCounter);
    }

    void ResetCounter()
    {
        if(patternCounter>0)
            ComboFailed();

        patternCounter = 0;
    }

    void CompletePattern()
    {
        variationCounter = Mathf.Clamp01(variationCounter + variantPerCompletedPattern);
        onComplete?.Invoke();
    }

    public override void ConfigureView(int _actionId, ControlViewer _controlViewer)
    {
        _controlViewer.FillKeycapPerId(_actionId, this);
    }

    public KeyCode[] GetPattern()
    {
        return pattern;
    }

    public void ComboFailed()
    {
        onFail?.Invoke();
    }
}
