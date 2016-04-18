using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : Killable {

    public enum Form {
        BIRD,
        PERSON
    };

    public GameObject cameraFollowObject;
    public GameObject invinvibilityIndicator;

    //variables
    public float horizontalDamping = 0.5f;
    public float maxYVelocity = 1;

    public float moveForce_bird = 10;
    public float moveForce_person = 50;
    public float jumpForce_bird = 5;
    public float jumpForce_person = 10;

    public float gravityScale_bird = 1;
    public float gravityScale_person = 4;

    private bool shotQueued = false;
    private float fireRate = 0.5f;
    private float fireCooldown = 0;


    private Form form;
    private bool moving = false;
    private bool grounded = false;
    private bool jumpAvailable = false;
    private float moveForce;
    private float jumpForce;

    private float groundCheckOffset = 0.05f;

    public SpriteRenderer bow;

    public SpriteRenderer person_idle;
    public SpriteRenderer person_jump;
    public SpriteRenderer person_run;
    public SpriteRenderer bird_flap;
    public SpriteRenderer bird_idle;

    //components
    private Rigidbody2D body;
    private CircleCollider2D collider;

    private float footstepTimer = 0;
    private float footstepFrequency = 0.4f;

    private bool dead = false;
    private bool invincible = false;

    protected override void Kill() {
        if (!invincible) {
            GameController.GameOver();
            dead = true;
        }
    }

    void Start () {
        ObjectController.SetPlayer(this);
        Camera.main.GetComponent<FollowCamera>().SetTarget(cameraFollowObject.transform);
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();

        SetForm(Form.PERSON);
	}
	
    void FixedUpdate() {

        if (dead) return;

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && form == Form.PERSON && grounded) {
            if (footstepTimer == 0) {
                SoundController.PlayFootstep();
                ObjectController.CreateSmallSmoke(transform.position - new Vector3(0, collider.radius));
                footstepTimer += Time.fixedDeltaTime;
            } else {
                footstepTimer += Time.fixedDeltaTime;
                if (footstepTimer > footstepFrequency) {
                    footstepTimer = 0;
                }
            }
        } else {
            footstepTimer = 0;
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            if (form == Form.PERSON || !grounded) {
                body.AddForce(new Vector2(-moveForce, 0));
                if (!moving) {
                    moving = true;
                    UpdatePlayerDisplay();
                }
            }
        } else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) {
            if (form == Form.PERSON || !grounded) {
                body.AddForce(new Vector2(moveForce, 0));
                if (!moving) {
                    moving = true;
                    UpdatePlayerDisplay();
                }
            }
        } else {
            if (moving) {
                moving = false;
                UpdatePlayerDisplay();
            }
        }

        //apply horizontal damping
        if (form == Form.PERSON) {
            body.velocity = new Vector2(body.velocity.x * horizontalDamping, body.velocity.y);
        }
        if (body.velocity.y < -15) {
            body.velocity = new Vector2(body.velocity.x, -15);
        }
    }

    void Update() {
        if (dead) return;

        if (Input.GetKeyDown(KeyCode.P)) {
            invincible = !invincible;
            invinvibilityIndicator.SetActive(invincible);
        }

        if (Input.GetKeyDown(KeyCode.Space) && (jumpAvailable || form == Form.BIRD)) {
            Jump();
        }

        if (Input.GetMouseButtonDown(1)) {
            ToggleForm();
        }

        if (Input.GetMouseButtonDown(0)) {
            if (fireCooldown < 0.2f) shotQueued = true;
        }

        if (fireCooldown <= 0) {
            if (shotQueued) {
                Shoot();
                fireCooldown = fireRate;
                shotQueued = false;
            }
        } else {
            fireCooldown -= Time.deltaTime;
        }

        UpdatePlayerOrientation();
        UpdateCameraFollowObject();

        if (transform.position.y < -1) Kill();
    }

    void OnCollisionEnter2D(Collision2D coll) {
        GroundCheck();
    }

    void OnCollisionExit2D(Collision2D coll) {
         GroundCheck();
    }

    private void UpdateCameraFollowObject() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cameraFollowOffset = mouseWorldPosition - transform.position;
        cameraFollowOffset.z = 0;
        cameraFollowOffset *= 0.25f;
        cameraFollowObject.transform.localPosition = cameraFollowOffset;
    }

    private void UpdatePlayerOrientation() {

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 bowRotationVector = mouseWorldPosition - transform.position;
        bowRotationVector.y *= -1;

        if (mouseWorldPosition.x < transform.position.x) {
            person_idle.flipX = true;
            person_jump.flipX = true;
            person_run.flipX = true;
            bird_flap.flipX = true;
            bird_idle.flipX = true;
            bow.flipX = true;
            bow.sortingOrder = -1;
            bow.transform.rotation = Quaternion.AngleAxis(Utils.Angle(bowRotationVector) + 90, Vector3.forward);
        } else {
            person_idle.flipX = false;
            person_jump.flipX = false;
            person_run.flipX = false;
            bird_flap.flipX = false;
            bird_idle.flipX = false;
            bow.flipX = false;
            bow.sortingOrder = 1;
            bow.transform.rotation = Quaternion.AngleAxis(Utils.Angle(bowRotationVector) - 90, Vector3.forward);
        }

       
        
    }

    private void UpdatePlayerDisplay() {
        if (form == Form.PERSON) {
            person_idle.gameObject.SetActive(false);
            person_jump.gameObject.SetActive(false);
            person_run.gameObject.SetActive(false);
            bird_flap.gameObject.SetActive(false);
            bird_idle.gameObject.SetActive(false);
            if (!bow.gameObject.activeInHierarchy) bow.gameObject.SetActive(true);
            if (!grounded) {
                person_jump.gameObject.SetActive(true);
            } else {
                if (moving) {
                    person_run.gameObject.SetActive(true);
                } else {
                    person_idle.gameObject.SetActive(true);
                }
            }
        } else if (form == Form.BIRD) {
            person_idle.gameObject.SetActive(false);
            person_jump.gameObject.SetActive(false);
            person_run.gameObject.SetActive(false);
            bird_idle.gameObject.SetActive(false);
            if (bow.gameObject.activeInHierarchy) bow.gameObject.SetActive(false);

            if (!grounded) {
                if (!bird_flap.gameObject.activeInHierarchy) bird_flap.gameObject.SetActive(true);
            } else {
                if (bird_flap.gameObject.activeInHierarchy) bird_flap.gameObject.SetActive(false);
                bird_idle.gameObject.SetActive(true);
            }
        }
    }

    private void Shoot() {
        if (form == Form.PERSON) {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 bowRotationVector = mouseWorldPosition - transform.position;
            bowRotationVector.z = 0;
            bowRotationVector.Normalize();
            ObjectController.CreateArrow(transform.position + bowRotationVector, bowRotationVector, 50);
            SoundController.PlayArrowShoot();
        }
    }

    private void Jump() {
        body.AddForce(new Vector2(0, jumpForce));
        if (form == Form.BIRD) {
            if (body.velocity.y > maxYVelocity) {
                body.velocity = new Vector2(body.velocity.x, maxYVelocity);
            }
            bird_flap.gameObject.SetActive(false);
            bird_flap.gameObject.SetActive(true);
            SoundController.PlayJumpBird();
        } else {
            SoundController.PlayJumpPerson();
            ObjectController.CreateSmallSmoke(transform.position - new Vector3(0, collider.radius));
        }
        grounded = false;
    }

    private void GroundCheck() {
        Vector2 left = new Vector2(transform.position.x - collider.radius / 2, transform.position.y - collider.radius - groundCheckOffset);
        Vector2 right = new Vector2(transform.position.x + collider.radius / 2, transform.position.y - collider.radius - groundCheckOffset);
        RaycastHit2D raycast = Physics2D.Linecast(left, right);
        grounded = raycast;
        jumpAvailable = grounded;
        if (grounded && form == Form.BIRD) {
            body.velocity = new Vector2();
        }
        UpdatePlayerDisplay();
    }

    private void ToggleForm() {
        if (form == Form.PERSON) {
            SetForm(Form.BIRD);
        } else if (form == Form.BIRD) {
            SetForm(Form.PERSON);
        }
        if (grounded && form == Form.BIRD) {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                Jump();
            } else {
                body.velocity = new Vector2();
            }
        }
        UpdatePlayerDisplay();
        SoundController.PlayShapeshift();
        ObjectController.CreateTransformationSmoke(transform.position);
    }

    private void SetForm(Form _form) {
        if (form != _form) {
            form = _form;
            if (form == Form.BIRD) {
                jumpAvailable = true;
                body.drag = 1;
                body.gravityScale = gravityScale_bird;
                moveForce = moveForce_bird;
                jumpForce = jumpForce_bird;
            } else if (form == Form.PERSON) {
                GroundCheck();
                body.drag = 0;
                body.gravityScale = gravityScale_person;
                moveForce = moveForce_person;
                jumpForce = jumpForce_person;
            }
        }
    }

    public Form GetForm() {
        return form;
    }
}
