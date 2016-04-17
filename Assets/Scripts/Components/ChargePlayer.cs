using UnityEngine;
using System.Collections;

public class ChargePlayer : MonoBehaviour {

    public Rigidbody2D body;
    public BoxCollider2D collider;
    public float jumpForce = 1500;
    public float runForce = 60;
    private bool grounded = false;
    private float groundCheckOffset = 0.05f;
    public float horizontalDamping = 0.8f;

    private float previousPositionX = 100;

	void FixedUpdate() {

        bool playerToLeft = ObjectController.GetPlayer().transform.position.x < transform.position.x;
        if (playerToLeft) {
            body.AddForce(new Vector2(-runForce, 0));
        } else {
            body.AddForce(new Vector2(runForce, 0));
        }

        if (Mathf.Abs(transform.position.x - previousPositionX) < 0.001f && grounded) {
            body.AddForce(new Vector2(0, jumpForce));
            grounded = false;
        }
        body.velocity = new Vector2(body.velocity.x * horizontalDamping, body.velocity.y);
        previousPositionX = transform.position.x;
    }

    void OnCollisionEnter2D(Collision2D coll) {
        GroundCheck();
    }

    void OnCollisionExit2D(Collision2D coll) {
        GroundCheck();
    }

    private void GroundCheck() {
        Vector2 left = new Vector2(transform.position.x - collider.size.x / 2 + 0.01f, transform.position.y - collider.size.y - groundCheckOffset);
        Vector2 right = new Vector2(transform.position.x + collider.size.x / 2 - 0.01f, transform.position.y - collider.size.y - groundCheckOffset);
        RaycastHit2D raycast = Physics2D.Linecast(left, right);
        grounded = raycast;
    }
}
