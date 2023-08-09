using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyTypeString : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(typeof(AudioClip).AssemblyQualifiedName);
    }
}
