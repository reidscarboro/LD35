using UnityEngine;
using System.Collections;

public class Grunt : Enemy {

    public SpriteRenderer grunt_idle;
    public SpriteRenderer grunt_dead;
    public SpriteRenderer crossbow;

    private float cooldownCounter = 0;
    public float shootCooldown = 0.2f;

    public LayerMask enemyLayer;

    protected override void Kill() {
        grunt_dead.transform.SetParent(null);
        grunt_dead.gameObject.SetActive(true);
        base.Kill();
        Destroy(gameObject);
    }

    void Start() {
        cooldownCounter = Random.Range(0, shootCooldown);
    }

    // Update is called once per frame
    void Update () {
        UpdateGruntOrientation();
        UpdateGruntOrientation();
        if (cooldownCounter > shootCooldown) {
            cooldownCounter = 0;
            Shoot();
        } else {
            cooldownCounter += Time.deltaTime;
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
            grunt_idle.flipX = true;
            grunt_dead.flipX = true;
            crossbow.flipX = true;
            crossbow.sortingOrder = -1;
            crossbow.transform.rotation = Quaternion.AngleAxis(Utils.Angle(bowRotationVector) + 90, Vector3.forward);
        } else {
            grunt_idle.flipX = false;
            grunt_dead.flipX = false;
            crossbow.flipX = false;
            crossbow.sortingOrder = 1;
            crossbow.transform.rotation = Quaternion.AngleAxis(Utils.Angle(bowRotationVector) - 90, Vector3.forward);
        }
    }

    private void Shoot() {
        
        Vector3 bowRotationVector = GetToPlayerAngle(true);
        bowRotationVector.z = 0;
        bowRotationVector.Normalize();
        bowRotationVector *= 2f;
        ObjectController.CreateEnemyArrow(transform.position + bowRotationVector, bowRotationVector, 50);
    }
}
