using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Istrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("ExitEnt");
    }
}
