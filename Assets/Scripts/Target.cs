using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject target;
    public AudioSource soundEffects;
    public AudioClip bell;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            soundEffects.PlayOneShot(bell);
            Destroy(target);
            PlayerController.score += 1000;
        }
    }
}
