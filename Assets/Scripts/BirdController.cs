using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BirdController : MonoBehaviour
{
    public float flapForce = 5f;  // The force applied to the bird when it flaps its wings
    public float rotationSpeed = 5f;  // The speed at which the bird rotates
    public float maxRotation = 35f;  // The maximum angle of rotation

    private Rigidbody2D rb;  // Reference to the Rigidbody2D component
    private float targetRotation = 0f;  // The target rotation of the bird
    public float lerpFactor = 5f;  // The interpolation factor for smoothing the rotation transition

    public bool hasFlapped = false;

    public TMP_Text scoreText;
    int score;

    AudioSource source;
    public float minPitch = 0.8f; // minimum pitch value
    public float maxPitch = 1.2f; // maximum pitch value
    public AudioClip jumpSound;
    public AudioClip scoreSound;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        score = 0;
        scoreText.text = score.ToString();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Set isKinematic to true so that gravity does not affect the bird
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            source.pitch = Random.Range(minPitch, maxPitch);
            source.PlayOneShot(jumpSound, 0.5f);

            hasFlapped = true;
            rb.isKinematic = false; // Set isKinematic to false to allow gravity to affect the bird
            rb.velocity = new Vector2(0, flapForce);
        }

        // Calculate the target rotation based on the vertical velocity of the bird
        targetRotation = Mathf.Lerp(-maxRotation, maxRotation, rb.velocity.y / 10f);

        // Smoothly interpolate the rotation of the bird
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetRotation), lerpFactor * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Pipe"))
        {
            source.pitch = Random.Range(minPitch, maxPitch);
            source.PlayOneShot(scoreSound, 1);

            score++;
            scoreText.text = score.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pipe") || collision.gameObject.CompareTag("Ground"))
        {
            source.pitch = Random.Range(minPitch, maxPitch);
            source.PlayOneShot(deathSound, 1);

            // Delay the scene restart for 1.5 seconds to ensure the player hears the death sound
            Invoke("RestartScene", 0.15f);
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
