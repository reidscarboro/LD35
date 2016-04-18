using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    private Transform target;
    private float z;

    public float lerpSpeed;
    public int minY = 0;
    public int maxY;
    public int minX;
    public int maxX;

    private bool shake = false;

    void Start() {
        z = transform.position.z;
    }

    void FixedUpdate() {
        if (target != null) {
            Vector3 newPosition;
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, z);
            if (Vector3.Distance(transform.position, targetPosition)> 0.005f) {
                newPosition = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);

                if (shake) {
                    Vector3 shakeAmount = Utils.Rotate(Vector2.up, Random.Range(0, 360));
                    shakeAmount *= 0.2f;
                    newPosition += shakeAmount;
                    shake = false;
                }


                if (newPosition.y - Camera.main.orthographicSize < minY) newPosition.y = minY + Camera.main.orthographicSize;
                if (newPosition.y + Camera.main.orthographicSize > maxY) newPosition.y = maxY - Camera.main.orthographicSize;

                float aspect = Camera.main.aspect;

                if (newPosition.x - Camera.main.orthographicSize * aspect < minX) newPosition.x = minX + Camera.main.orthographicSize * aspect;
                if (newPosition.x + Camera.main.orthographicSize * aspect > maxX) newPosition.x = maxX - Camera.main.orthographicSize * aspect;

                transform.position = newPosition;
            }

        }
    }

    public void Shake() {
        shake = true;
    }

    public void SetTarget(Transform _target) {
        target = _target;
    }
}
