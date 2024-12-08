using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1.0f; // Czas opóŸnienia przed spadniêciem.
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
        // SprawdŸ, czy gracz (tag "Player") wszed³ na platformê.
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    private IEnumerator FallAfterDelay()
    {
        yield return new WaitForSeconds(fallDelay); // Odczekaj okreœlony czas.
        rb.isKinematic = false; // Ustaw isKinematic na false, by platforma zaczê³a spadaæ.
        boxCollider.enabled = false;

    }
}
