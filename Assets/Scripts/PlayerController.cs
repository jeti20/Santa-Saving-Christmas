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
    [SerializeField] private float minY;
    private bool isGrounded;

    public int score;

    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        // Get the keyboard inputs.
        float x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float z = Input.GetAxisRaw("Vertical") * moveSpeed;

        // Set out velocity (move the player).
        rig.linearVelocity = new Vector3(x, rig.linearVelocity.y, z);

        // Create a temporary velocity vector and cancel out the Y.
        Vector3 vel = rig.linearVelocity;
        vel.y = 0;

        // Je�li posta� si� porusza, ustaw isRunning na true, w przeciwnym wypadku na false
        animator.SetBool("isRunning", vel.magnitude > 0);

        // If we are moving, rotate to face that direction.
        if (vel.x != 0 || vel.z != 0)
        {
            transform.forward = vel;
        }

        // If we press SPACE and we are on the ground, jump.
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            isGrounded = false;
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        // If we are below the level, game over.
        if (transform.position.y < -15)
        {
            GameOver();
        }

    }


    private void OnCollisionEnter(Collision collision)
{
    // Jeśli zderzamy się z ziemią lub platformą, ustaw isGrounded na true.
    if (collision.GetContact(0).normal == Vector3.up)
    {
        isGrounded = true;
    }

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

    // Called when an enemy hits us, or we fall off the level.
    public void GameOver ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Called when we collect a coin.
    public void AddScore (int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }
}
