using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 180.0f;

    // Update is called once per frame
    void Update()
    {
        // Rotate around over time.
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter (Collider other)
    {
        // Did the player enter our trigger?
        if(other.CompareTag("Player"))
        {
            // Add player score then destroy the coin.
            other.GetComponent<PlayerController>().AddScore(1);
            Destroy(gameObject);
        }
    }
}