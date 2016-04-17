using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    public Rigidbody2D body;

	public void Spawn(Vector2 direction, float velocity) {
        body.AddForce(direction.normalized * velocity);
    }
	
	void Update () {
        if (body.velocity.magnitude > 0.01f) {
            SetRotation(body.velocity);
        }
	}

    void OnCollisionEnter2D(Collision2D coll) {
        gameObject.SetActive(false);
        ObjectController.CreateArrowHit(transform.position, transform.rotation, coll.gameObject.transform);
        Killable killable = coll.gameObject.GetComponent<Killable>();
        SoundController.PlayArrowHit();
        if (killable != null) killable.Damage(5);
        Destroy(gameObject);
    }

    private void SetRotation(Vector2 angle) {
        angle.y *= -1;
        body.rotation = Utils.Angle(angle) - 90;
    }
}
