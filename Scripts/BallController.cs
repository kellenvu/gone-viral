using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
    public float gravity = 1f;
    public float sizeFactor = 1f;
    private float speed;
    [SerializeField] Transform centerOfBouyancy;

    bool isSubmerged = false;
    bool canWaterJump = true;

    public enum state {
        normal,
        metallic,
        bouyant,
        sticky,
    }
    state myState = state.normal;

    public bool TakesSpikeDamage {
        get { return myState != state.metallic; }
    }

    public ParticleSystem deathEffect;

    private Rigidbody2D rb;
    private Animator squashAnimator;
    [SerializeField] Animator spriteAnimator;
    [SerializeField] SpriteRenderer mySprite;
    private bool isGrounded = false;
    private Vector3 startingScale;
    private GameObject ballPrefab;

    // Constants
    private float jumpForce = 1200f;
    private float sizeIncrement = 0.6f;
    private float minSize = 0.4f;
    private float maxSize = 2.5f;

    public float startingSpeed = 1000f;
    private float speedIncrement = 300f;
    private float minSpeed = 100f;
    private float maxSpeed = 2200f;

    [SerializeField] float bouyancyScale = 7f;
    [SerializeField] float bounceDamp = 0.05f;

    // Sounds
    private AudioSource myAudio;
    private AudioSource rollingAudioSource;
    private AudioSource squishAudioSource;
    private AudioClip rollingSound;
    private AudioClip squishSound;
    private AudioClip jumpSound;
    private AudioClip splashSound;
    private AudioClip powerupSound;

    private void Awake() {
        // Sounds
        myAudio = GetComponent<AudioSource>();
        rollingAudioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        squishAudioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        rollingSound = Resources.Load("Sounds/Marble Rolling") as AudioClip;
        squishSound = Resources.Load("Sounds/Continuous Squish") as AudioClip;
        jumpSound = Resources.Load("Sounds/Jump 4") as AudioClip;
        splashSound = Resources.Load("Sounds/Water Splash") as AudioClip;
        powerupSound = Resources.Load("Sounds/Powerup") as AudioClip;
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        squashAnimator = GetComponent<Animator>();

        ballPrefab = Resources.Load<GameObject>("Ball");

        // Gravity
        rb.gravityScale = Mathf.Abs(rb.gravityScale) * gravity;

        speed = startingSpeed;
        startingScale = transform.localScale;
        transform.localScale = startingScale * sizeFactor;

        InvokeRepeating("PlaySquishSound", 0f, squishSound.length);
    }

    void Update() {
        doMovement();

        updateSize();
        // testTools();
        PlaySounds();
    }

    void testTools() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            increaseSize();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            decreaseSize();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            increaseSpeed();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            decreaseSpeed();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            reverseGravity();
        }
    }

    void doMovement() {
        // Horizontal control
        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.AddForce(new Vector2(horizontal * speed * Time.deltaTime, 0));

        // Jump
        isGrounded = checkIsGrounded();

        if (rb.velocity.y < 0) canWaterJump = true;

        if ((isGrounded || (isSubmerged && canWaterJump && myState != state.metallic)) && Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }
    }

    void Jump() {
        if (gravity > 0) {
            rb.AddForce(Vector3.up * jumpForce);
        } else if (gravity < 0) {
            rb.AddForce(Vector3.down * jumpForce);
        }

        if(isSubmerged) {
            canWaterJump = false;
        }

        if(myState != state.metallic)
            squashAnimator.SetTrigger("stretch");

        myAudio.PlayOneShot(jumpSound, 0.5f);
    }

    bool checkIsGrounded() {
        /*
        float radius = gameObject.GetComponent<CircleCollider2D>().bounds.extents.x;
        //RaycastHit2D hit;
        //RaycastHit2D hitLeft;
        //RaycastHit2D hitRight;

        Vector3 leftDir;
        Vector3 rightDir;
        Vector3 centerDir;

        float raycastDist = 0.5f;

        if (gravity > 0) {
            float leftAngle = (3 * Mathf.PI / 2) - Mathf.PI / 12;
            float rightAngle = (3 * Mathf.PI / 2) + Mathf.PI / 12;
            leftDir = new Vector3(Mathf.Cos(leftAngle), Mathf.Sin(leftAngle), 0);
            rightDir = new Vector3(Mathf.Cos(rightAngle), Mathf.Sin(rightAngle), 0);
            centerDir = new Vector3(0, -1, 0);
        }
        else {
            float leftAngle = (Mathf.PI / 2) - Mathf.PI / 12;
            float rightAngle = (Mathf.PI / 2) + Mathf.PI / 12;
            leftDir = new Vector3(Mathf.Cos(leftAngle), Mathf.Sin(leftAngle), 0);
            rightDir = new Vector3(Mathf.Cos(rightAngle), Mathf.Sin(rightAngle), 0);
            centerDir = new Vector3(0, 1, 0);
        }

        hit = Physics2D.Raycast(transform.position + (radius + 0.01f) * centerDir,
                centerDir, raycastDist, LayerMask.GetMask("Ground", "Player"));
        hitLeft = Physics2D.Raycast(transform.position + ((radius + 0.01f) * leftDir),
            leftDir, raycastDist, LayerMask.GetMask("Ground", "Player"));
        hitRight = Physics2D.Raycast(transform.position + ((radius + 0.01f) * rightDir),
            rightDir, raycastDist, LayerMask.GetMask("Ground", "Player"));

        Debug.DrawRay(transform.position + radius * centerDir, centerDir * raycastDist, Color.green);
        Debug.DrawRay(transform.position + radius * leftDir, leftDir * raycastDist, Color.green);
        Debug.DrawRay(transform.position + radius * rightDir, rightDir * raycastDist, Color.green);

        bool isGrounded = (hit.collider != null && hit.collider != GetComponent<Collider2D>()) ||
                          (hitLeft.collider != null && hitLeft.collider != GetComponent<Collider2D>()) ||
                          (hitRight.collider != null && hitRight.collider != GetComponent<Collider2D>());
        */

        float radius = gameObject.GetComponent<CircleCollider2D>().bounds.extents.x;
        Vector3 centerDir;

        float raycastDist = 0.2f;
        float offsetAmount = radius * 0.5f;

        if(gravity > 0) {
            centerDir = new Vector3(0, -1, 0);
        }
        else {
            centerDir = new Vector3(0, 1, 0);
        }

        // Raycasting
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + radius * centerDir,
                centerDir, raycastDist, LayerMask.GetMask("Ground", "Player"));

        Vector3 offsetRight = transform.position + new Vector3(offsetAmount, 0, 0);
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(offsetRight + radius * centerDir,
                centerDir, raycastDist, LayerMask.GetMask("Ground", "Player"));

        Vector3 offsetLeft = transform.position + new Vector3(-offsetAmount, 0, 0);
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(offsetLeft + radius * centerDir,
                centerDir, raycastDist, LayerMask.GetMask("Ground", "Player"));

        // Debugging
        Debug.DrawRay(transform.position + radius * centerDir, centerDir * raycastDist, Color.green);
        Debug.DrawRay(offsetRight + radius * centerDir, centerDir * raycastDist, Color.green);
        Debug.DrawRay(offsetLeft + radius * centerDir, centerDir * raycastDist, Color.green);

        // Check hits
        bool isGrounded = false;
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider != GetComponent<Collider2D>()) {
                isGrounded = true;
            }
        }
        foreach(RaycastHit2D hit in hitsLeft) {
            if(hit.collider != GetComponent<Collider2D>()) {
                isGrounded = true;
            }
        }
        foreach(RaycastHit2D hit in hitsRight) {
            if(hit.collider != GetComponent<Collider2D>()) {
                isGrounded = true;
            }
        }

        return isGrounded;
    }

    float Remap(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void PlaySounds() {
        rollingAudioSource.volume = 0f;
        squishAudioSource.volume = 0f;
        if(checkIsGrounded()) {
            float continuousVolume = Remap(rb.velocity.magnitude, 3f, 15f, 0f, 0.2f);
            if(transform.GetComponentInParent<BallGroup>().GetTotalRollingVolume() >= 0.4f) {
                continuousVolume = 0f; // Cap the volume if it's too loud
            }
            if(myState == state.metallic) {
                rollingAudioSource.volume = continuousVolume;
            }
            else {
                squishAudioSource.volume = continuousVolume;
            }
        }
    }

    void PlayMarbleSound() {
        rollingAudioSource.PlayOneShot(rollingSound);
    }

    void PlaySquishSound() {
        squishAudioSource.PlayOneShot(squishSound);
    }

    public void PlayPowerupSound() {
        myAudio.PlayOneShot(powerupSound, 0.05f);
    }

    public float GetRollingVolume() {
        return rollingAudioSource.volume + squishAudioSource.volume;
    }

    /*
     * Smoothly updates size each frame to match startingScale * sizeFactor
     */
    public void updateSize() {
        Vector2 scale = transform.localScale;
        scale = Vector3.Lerp(scale, startingScale * sizeFactor, 0.4f);
        transform.localScale = scale;
    }

    public void increaseSize() {
        sizeFactor = Mathf.Clamp(sizeFactor + sizeIncrement, minSize, maxSize);
    }

    public void decreaseSize() {
        sizeFactor = Mathf.Clamp(sizeFactor - sizeIncrement, minSize, maxSize);
    }

    public void increaseSpeed() {
        speed = Mathf.Clamp(speed + speedIncrement, minSpeed, maxSpeed);
    }

    public void decreaseSpeed() {
        speed = Mathf.Clamp(speed - speedIncrement, minSpeed, maxSpeed);
    }

    public void die() {
        GameObject particles = Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
    }

    public void reverseGravity() {
        gravity *= -1;
        rb.gravityScale *= -1;
    }

    public void clone() {
        var newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        newBall.transform.parent = BallGroup.instance.transform;
        BallController newBallController = newBall.GetComponent<BallController>();
        newBallController.Initialize(sizeFactor, speed, gravity, myState, rb.velocity);
    }

    public void Initialize(float newSize, float newSpeed, float newGravity, state newState, Vector2 newVelocity) {
        sizeFactor = newSize;
        speed = newSpeed;
        gravity = newGravity;
        updateState(newState);

        GetComponent<Rigidbody2D>().velocity = newVelocity;
    }

    public void updateState(state newState) {
        myState = newState;

        switch(myState) {
            case state.metallic: {
                    spriteAnimator.SetTrigger("toMetal");
                    CancelInvoke();
                    InvokeRepeating("PlayMarbleSound", 0f, 4f);
                    break;
                }
            default: {
                    spriteAnimator.SetTrigger("toNormal");
                    CancelInvoke();
                    InvokeRepeating("PlaySquishSound", 0f, squishSound.length);
                    break;
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water")) {
            myAudio.PlayOneShot(splashSound, 0.05f);
            isSubmerged = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water")) {
            isSubmerged = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        // Float in water if not metallic
        if (myState != state.metallic && collision.gameObject.layer == LayerMask.NameToLayer("Water")) {
            Float(collision);
        }
    }

    private void Float(Collider2D collision) {
        float forceFactor = 1f - (centerOfBouyancy.position.y - collision.gameObject.transform.position.y);

        if (forceFactor > 0f) {
            Vector2 uplift = -Physics2D.gravity * Mathf.Abs(gravity) * (forceFactor - GetComponent<Rigidbody2D>().velocity.y * bounceDamp);
            rb.AddForceAtPosition(uplift * bouyancyScale, centerOfBouyancy.position);
        }
    }
}
