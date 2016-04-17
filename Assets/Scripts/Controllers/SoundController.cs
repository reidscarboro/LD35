using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

    public static SoundController instance;

    public AudioClip arrow_hit;
    public AudioClip arrow_shoot;
    public AudioClip enemy_death;
    public AudioClip jump_bird;
    public AudioClip jump_person;
    public AudioClip level_complete;
    public AudioClip shapeshift;
    public AudioClip footstep;

    public AudioSource audioSource;

    void Start() {
        instance = this;
    }

    public static void PlayArrowHit() {
        instance.audioSource.clip = instance.arrow_hit;
        instance.audioSource.volume = 0.5f;
        instance.audioSource.Play();
    }

    public static void PlayArrowShoot() {
        instance.audioSource.clip = instance.arrow_shoot;
        instance.audioSource.volume = 1.0f;
        instance.audioSource.Play();
    }

    public static void PlayEnemyDeath() {
        instance.audioSource.clip = instance.enemy_death;
        instance.audioSource.volume = 1.0f;
        instance.audioSource.Play();
    }

    public static void PlayJumpBird() {
        instance.audioSource.clip = instance.jump_bird;
        instance.audioSource.volume = 0.5f;
        instance.audioSource.Play();
    }

    public static void PlayJumpPerson() {
        instance.audioSource.clip = instance.jump_person;
        instance.audioSource.volume = 1.0f;
        instance.audioSource.Play();
    }

    public static void PlayLevelComplete() {
        instance.audioSource.clip = instance.level_complete;
        instance.audioSource.volume = 1.0f;
        instance.audioSource.Play();
    }

    public static void PlayShapeshift() {
        instance.audioSource.clip = instance.shapeshift;
        instance.audioSource.volume = 1.0f;
        instance.audioSource.Play();
    }

    public static void PlayFootstep() {
        instance.audioSource.clip = instance.footstep;
        instance.audioSource.volume = 1.0f;
        instance.audioSource.Play();
    }
}
