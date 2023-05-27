using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameObject trigger1;
    public GameObject trigger2;

    public static System.Action Finish;

    private int count;

    // Start is called before the first frame update
    void Start()
    {
        trigger2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(count == 0)
        {
        trigger1.SetActive(false);
        trigger2.SetActive(true);
        }
        else if(count == 1)
        {
            trigger2.SetActive(false);
            Finish?.Invoke();
        }
        count++;
    }
}
