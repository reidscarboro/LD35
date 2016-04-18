using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    public Rigidbody2D body;

    public void Spawn(Vector2 direction, float velocity) {
        body.AddForce(direction.normalized * velocity);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        gameObject.SetActive(false);
        Killable killable = coll.gameObject.GetComponent<Killable>();
        SoundController.PlayArrowHit();
        if (killable != null) killable.Damage(5);
        Destroy(gameObject);
    }
}
