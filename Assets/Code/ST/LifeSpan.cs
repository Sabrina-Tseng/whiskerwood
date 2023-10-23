using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpan : MonoBehaviour
{
    public int lifetime = 5;

    void Start()
    {
        Destroy(gameObject,lifetime);
    }
}
