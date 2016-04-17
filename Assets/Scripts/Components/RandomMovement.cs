using UnityEngine;
using System.Collections;

public class RandomMovement : MonoBehaviour {

    public Wakeable wakeable;

    public Rigidbody2D body;
    public float runForce = 50;
    public float horizontalDamping = 0.8f;

    private int direction = 0;

	
	// Update is called once per frame
	void FixedUpdate () {
        if (wakeable.awake) {
            body.AddForce(new Vector2(runForce * direction, 0));

            if (direction != 0) {
                if (Random.Range(0, 1.0f) > 0.9f) {
                    direction = 0;
                }
            } else {
                if (Random.Range(0, 1.0f) > 0.95f) {
                    direction = Random.Range(-1, 2);
                }
            }
        }
        body.velocity = new Vector2(body.velocity.x * horizontalDamping, body.velocity.y);
    }
}
