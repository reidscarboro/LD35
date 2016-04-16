using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    private Transform target;
    private float z;

    public float lerpSpeed;

    void Start() {
        z = transform.position.z;
    }

    void FixedUpdate() {
        if (target != null) {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, z);
            if (Vector3.Distance(transform.position, targetPosition)> 0.005f) {
                transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
            }
        }
    }

    public void SetTarget(Transform _target) {
        target = _target;
    }
}
