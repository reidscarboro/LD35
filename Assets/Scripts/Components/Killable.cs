using UnityEngine;
using System.Collections;

public class Killable : MonoBehaviour {

    public float health = 10;

    public void Damage(float damage) {
        health -= damage;
        if (health <= 0) {
            Kill();
        }
    }

    protected virtual void Kill() {

    }
}
