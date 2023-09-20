using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * A simple class that allows for a player to move in any direction.
 */
public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    private Vector3 moveDirection = Vector3.zero;
    public static int health = 100;
    public Text healthText;
    public AudioSource soundEffects;
    public AudioSource rollerCoasterSounds;
    public AudioSource music;
    public AudioClip hurt;
    public AudioClip death;
    public Text scoreText;
    public AudioClip chainLift;
    public AudioClip woosh;
    public AudioClip musicClip;
    public static int score = 0;
    void Start()
    {
        StartCoroutine(rollerCoaster());
    }
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        controller.Move(moveDirection * Time.deltaTime);
        healthText.text = "Health: " + health;
        scoreText.text = "Score: " + score;
        soundEffects.volume = SettingsScene.soundEffectsVolume;
        rollerCoasterSounds.volume = SettingsScene.soundEffectsVolume;
        music.volume = SettingsScene.musicVolume;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "AIBullet")
        {   
            health -= 10;
            if (health > 0)
                soundEffects.PlayOneShot(hurt);
            
            if (health <= 0)
            {
                soundEffects.PlayOneShot(death);
                Destroy(gameObject);
            }
        }
    }
    IEnumerator rollerCoaster()
    {
        music.PlayOneShot(musicClip);
        music.loop = true;
        rollerCoasterSounds.PlayOneShot(chainLift);
        yield return new WaitForSeconds(26);
        rollerCoasterSounds.loop = true;
        rollerCoasterSounds.clip = woosh;
        rollerCoasterSounds.Play();
    }
}