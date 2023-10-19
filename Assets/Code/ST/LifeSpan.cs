using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpan : MonoBehaviour
{
    public int lifetime = 2;

    void Start()
    {
        Destroy(gameObject,lifetime);
    }
}
