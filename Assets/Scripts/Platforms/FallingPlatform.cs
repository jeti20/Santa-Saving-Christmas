using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1.0f; // Czas op�nienia przed spadni�ciem.
    private Rigidbody rb;
    private BoxCollider boxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();


        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Sprawd�, czy gracz (tag "Player") wszed� na platform�.
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    private IEnumerator FallAfterDelay()
    {
        yield return new WaitForSeconds(fallDelay); // Odczekaj okre�lony czas.
        rb.isKinematic = false; // Ustaw isKinematic na false, by platforma zacz�a spada�.
        boxCollider.enabled = false;

    }
}
