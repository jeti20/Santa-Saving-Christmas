using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed = 180.0f;

    // Update is called once per frame
    void Update()
    {
        // Rotate around over time.
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}