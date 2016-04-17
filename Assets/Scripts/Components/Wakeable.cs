using UnityEngine;
using System.Collections;

public class Wakeable : MonoBehaviour {

    public bool awake = false;
    private float aggroDistance = 10;

    void Update() {
        if (Vector2.Distance(transform.position, ObjectController.GetPlayer().transform.position) < aggroDistance) {
            awake = true;
        }
    }
}
