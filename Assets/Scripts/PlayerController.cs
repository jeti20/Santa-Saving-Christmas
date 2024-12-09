using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private Rigidbody rig;
    private Animator animator;
    [SerializeField] private LayerMask groundLayerMask; // Warstwa dla ziemi i platform.
    [SerializeField] private float groundCheckDistance = 0.1f; // Dystans Raycastów do sprawdzania ziemi.
    [SerializeField] private float raycastOffset = 0.2f; // Przesunięcie Raycastów od środka gracza.

    public int score;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Odczyt wejść klawiatury.
        float x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float z = Input.GetAxisRaw("Vertical") * moveSpeed;

        // Ustawianie prędkości gracza.
        rig.linearVelocity = new Vector3(x, rig.linearVelocity.y, z);

        // Sprawdzanie prędkości dla animacji biegania.
        Vector3 vel = rig.linearVelocity;
        vel.y = 0;
        animator.SetBool("isRunning", vel.magnitude > 0);

        // Obrót w kierunku ruchu.
        if (vel.x != 0 || vel.z != 0)
        {
            transform.forward = vel;
        }

        // Skok, jeśli gracz jest na ziemi i naciśnięto spację.
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Restart poziomu, jeśli gracz spadnie poniżej pewnej wysokości.
        if (transform.position.y < -15)
        {
            GameOver();
        }
    }

    private bool IsGrounded()
    {
        // Definiowanie pozycji Raycastów (cztery rogi podstawy gracza).
        Vector3[] raycastOrigins = new Vector3[]
        {
            transform.position + (transform.forward * raycastOffset) + (Vector3.up * 0.01f), // Przód
            transform.position + (-transform.forward * raycastOffset) + (Vector3.up * 0.01f), // Tył
            transform.position + (transform.right * raycastOffset) + (Vector3.up * 0.01f), // Prawo
            transform.position + (-transform.right * raycastOffset) + (Vector3.up * 0.01f) // Lewo
        };

        // Sprawdzanie każdego Raycasta.
        foreach (var origin in raycastOrigins)
        {
            if (Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Jeśli gracz wchodzi na ruchomą platformę.
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(collision.transform); // Ustawienie rodzica na platformę.
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Jeśli gracz opuszcza platformę.
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null); // Usunięcie rodzica.
        }
    }

    public void GameOver()
    {
        // Restart sceny.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    private void OnDrawGizmos()
    {
        // Wizualizacja Raycastów w trybie edycji.
        Gizmos.color = Color.red;

        Vector3[] raycastOrigins = new Vector3[]
        {
            transform.position + (transform.forward * raycastOffset) + (Vector3.up * 0.01f),
            transform.position + (-transform.forward * raycastOffset) + (Vector3.up * 0.01f),
            transform.position + (transform.right * raycastOffset) + (Vector3.up * 0.01f),
            transform.position + (-transform.right * raycastOffset) + (Vector3.up * 0.01f)
        };

        foreach (var origin in raycastOrigins)
        {
            Gizmos.DrawRay(origin, Vector3.down * groundCheckDistance);
        }
    }
}
