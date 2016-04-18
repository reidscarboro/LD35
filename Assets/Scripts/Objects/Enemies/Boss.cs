using UnityEngine;
using System.Collections;

public class Boss : Enemy {

    public LayerMask defaultLayerMask;
    public Rigidbody2D body;
    public float moveSpeed = 50;
    public SpriteRenderer sr;

    public float fireRate = 1;
    public float shootCooldown = 0;

    private int horizontalMovementDirection = 0;
    private int verticalMovementDirection = 0;
    private float initialHealth;

    void Start() {
        initialHealth = health;
    }

    protected override void Kill() {
        Destroy(gameObject);
        base.Kill();
    }

        // Update is called once per frame
    void FixedUpdate () {
        Vector2 toPlayerAngle = GetToPlayerAngle();
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, GetToPlayerAngle(), toPlayerAngle.magnitude, defaultLayerMask);
        if (!raycast) {
            if (shootCooldown <= 0) {
                if (Random.Range(0, 1.0f) < 0.05f) {
                    Shoot(toPlayerAngle);
                    shootCooldown = fireRate;
                }
            } else {
                shootCooldown -= Time.fixedDeltaTime;
            }
            horizontalMovementDirection = 0;
            verticalMovementDirection = 0;
            body.AddForce(GetToPlayerAngle().normalized * moveSpeed);
        } else {
            if (Mathf.Abs(raycast.normal.y) > 0.1f) {
                //if we need to pick a horizontal movement direction
                if (horizontalMovementDirection == 0) {
                    if (toPlayerAngle.x < 0) {
                        horizontalMovementDirection = -1;
                    } else {
                        horizontalMovementDirection = 1;
                    }
                }
                verticalMovementDirection = 0;
                body.AddForce(Vector2.right * moveSpeed * horizontalMovementDirection);
            } else {
                if (verticalMovementDirection == 0) {
                    if (toPlayerAngle.y < 0) {
                        verticalMovementDirection = -1;
                    } else {
                        verticalMovementDirection = 1;
                    }
                }
                horizontalMovementDirection = 0;
                body.AddForce(Vector2.up * moveSpeed * verticalMovementDirection);
            }
        }

        if (toPlayerAngle.x < 0) {
            sr.flipX = true;
        } else {
            sr.flipX = false;
        }
    }

    private Vector2 GetToPlayerAngle() {
        Vector3 playerPosition = ObjectController.GetPlayer().transform.position;
        Vector2 toPlayerVector = playerPosition - transform.position;
        return toPlayerVector;
    }

    private void Shoot(Vector2 angle) {
        if (health / initialHealth < 0.25f) {
            if (fireRate > 0.6) fireRate = 0.6f;
            if (moveSpeed < 7) moveSpeed = 7;
            ObjectController.CreateFireball(transform.position, Utils.Rotate(angle, -60 + Random.Range(-10,10)), 50);
            ObjectController.CreateFireball(transform.position, Utils.Rotate(angle, -30 + Random.Range(-10, 10)), 50);
            ObjectController.CreateFireball(transform.position, Utils.Rotate(angle, 0 + Random.Range(-10, 10)), 50);
            ObjectController.CreateFireball(transform.position, Utils.Rotate(angle, 30 + Random.Range(-10, 10)), 50);
            ObjectController.CreateFireball(transform.position, Utils.Rotate(angle, 60 + Random.Range(-10, 10)), 50);

        } else if (health / initialHealth < 0.5f) {
            if (fireRate > 0.8) fireRate = 0.8f;
            if (moveSpeed < 6) moveSpeed = 6;
            ObjectController.CreateFireball(transform.position, Utils.Rotate(angle, -30 + Random.Range(-5, 5)), 50);
            ObjectController.CreateFireball(transform.position, Utils.Rotate(angle, 0 + Random.Range(-5, 5)), 50);
            ObjectController.CreateFireball(transform.position, Utils.Rotate(angle, 30 + Random.Range(-5, 5)), 50);
        } else {
            ObjectController.CreateFireball(transform.position, angle, 50);
        }

        SoundController.PlayBossShoot();
    }
}
