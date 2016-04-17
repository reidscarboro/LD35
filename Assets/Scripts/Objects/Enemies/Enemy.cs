using UnityEngine;
using System.Collections;

public class Enemy : Killable {

    public SpriteRenderer dead;
    public LayerMask defaultLayer;

    void Awake() {
        health *= GameController.GetDifficulty() + 1;
    }

    protected override void Kill() {
        SoundController.PlayEnemyDeath();
        if (dead != null) {
            dead.transform.SetParent(null);
            RaycastHit2D raycast = Physics2D.Raycast(dead.transform.position, Vector2.down, 100, defaultLayer);
            dead.gameObject.transform.position = raycast.point;
            dead.gameObject.SetActive(true);
        }

        
        GameController.DecrementEnemies();
    }
}
