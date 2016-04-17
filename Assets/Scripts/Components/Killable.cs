using UnityEngine;
using System.Collections;

public class Killable : MonoBehaviour {

    public float health = 10;

    public void Damage(float damage) {
        health -= damage;
        if (health <= 0) {
            Kill();
        }
        Wakeable wakeable = GetComponent<Wakeable>();
        if (wakeable != null) {
            wakeable.awake = true;
        }
    }

    protected virtual void Kill() {

    }
}
