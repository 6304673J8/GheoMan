using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInstance : MonoBehaviour
{
    public void Start()
    {
        Destroy(gameObject, 1);
    }
}
