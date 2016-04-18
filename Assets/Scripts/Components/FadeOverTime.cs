using UnityEngine;
using System.Collections;

public class FadeOverTime : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    private float life;
    private float lifeSpan = 3f;

    void Start() {
        life = lifeSpan;
    }

    void Update() {
        life -= Time.deltaTime;
        if (life < 0) Destroy(this);
        if (spriteRenderer != null) spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, life / lifeSpan);
    }
}
