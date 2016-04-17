using UnityEngine;
using System.Collections;

public class Grunt : Enemy {

    public Wakeable wakeable;

    public SpriteRenderer grunt_sleep;
    public SpriteRenderer grunt_awake;
    public SpriteRenderer crossbow;

    private float cooldownCounter = 0;
    public float shootCooldown = 0.2f;

    public LayerMask enemyLayer;

    protected override void Kill() {
        base.Kill();
        Destroy(gameObject);
    }

    void Start() {
        cooldownCounter = Random.Range(0, shootCooldown);
    }

    // Update is called once per frame
    void Update () {
        if (wakeable.awake) {

            if (!grunt_awake.gameObject.activeInHierarchy) grunt_awake.gameObject.SetActive(true);
            if (grunt_sleep.gameObject.activeInHierarchy) grunt_sleep.gameObject.SetActive(false);

            UpdateGruntOrientation();
            if (cooldownCounter > shootCooldown) {
                cooldownCounter = 0;
                Shoot();
            } else {
                cooldownCounter += Time.deltaTime;
            }
        }
    }

    private Vector2 GetToPlayerAngle(bool randomize) {
        Vector3 playerPosition = ObjectController.GetPlayer().transform.position;
        Vector2 toPlayerVector = playerPosition - transform.position;

        toPlayerVector.y += Mathf.Abs(toPlayerVector.x) * 0.25f;
        if (randomize) toPlayerVector.y += Random.Range(-Mathf.Abs(toPlayerVector.x) * 0.1f, Mathf.Abs(toPlayerVector.x) * 0.3f);

        return toPlayerVector;
    }

    private void UpdateGruntOrientation() {
        Vector2 bowRotationVector = GetToPlayerAngle(false);
        bowRotationVector.y *= -1;

        if (bowRotationVector.x < 0) {
            grunt_sleep.flipX = true;
            grunt_awake.flipX = true;
            crossbow.flipX = true;
            crossbow.sortingOrder = -1;
            crossbow.transform.rotation = Quaternion.AngleAxis(Utils.Angle(bowRotationVector) + 90, Vector3.forward);
        } else {
            grunt_sleep.flipX = false;
            grunt_awake.flipX = false;
            crossbow.flipX = false;
            crossbow.sortingOrder = 1;
            crossbow.transform.rotation = Quaternion.AngleAxis(Utils.Angle(bowRotationVector) - 90, Vector3.forward);
        }
    }

    private void Shoot() {
        
        Vector3 bowRotationVector = GetToPlayerAngle(true);
        bowRotationVector.z = 0;
        bowRotationVector.Normalize();
        bowRotationVector *= 1.5f;
        ObjectController.CreateEnemyArrow(transform.position + bowRotationVector, bowRotationVector, 50);
        SoundController.PlayArrowShoot();
    }
}
