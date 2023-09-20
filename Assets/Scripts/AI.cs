using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AI : MonoBehaviour
{

    public static int health = 1337;
    public static int maxHealth = 1337;
    public GameObject healthTextContainer;
    public TMP_Text healthText;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float shootForce = 20f, reloadTime = 2f;
    private float currentAmmo = Mathf.Infinity;
    private bool isShooting = false;
    public AudioSource soundEffects;
    public AudioClip laser;
    public AudioClip hurt;
    public AudioClip death;
    public GameObject player;

    void Update()
    {
        CalculateShotDirection();
        UpdateHealthText();
        float randomTime = UnityEngine.Random.Range(.5f, 3f);
        if (currentAmmo > 0 && !isShooting)
        {
            isShooting = true;
            Invoke("Shoot", randomTime);
        }
    }
    private void UpdateHealthText()
    {
        Vector3 playerDirection = player.transform.position - transform.position;
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        healthTextContainer.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.up);
        TextMeshPro healthText = healthTextContainer.GetComponentInChildren<TextMeshPro>();
        healthText.text = health + "/" + maxHealth;
    }
    public void Shoot()
    {
        soundEffects.PlayOneShot(laser);
        currentAmmo--;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<MeshRenderer>().enabled = true;
        bullet.GetComponent<Collider>().enabled = true;
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.useGravity = false;
        bulletRB.AddForce(bulletSpawnPoint.forward * shootForce, ForceMode.Impulse);
        isShooting = false;
        StartCoroutine(DestroyBullet(bullet, 5f));
    }
    private IEnumerator DestroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
    private void CalculateShotDirection()
    {
        Vector3 playerDirection = player.transform.position - transform.position;
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        /*Debug.Log(" ai's y rotation: " + this.transform.rotation.eulerAngles);*/
    }
    private void OnCollisionEnter(Collision collision)
    {    
            if (collision.gameObject.tag == "Bullet")
            {
                health -= (int)(UnityEngine.Random.Range(0.02f, 0.05f) * health);
                if (health > 0)
                    soundEffects.PlayOneShot(hurt);
                
                if (health <= 0)
                {
                    soundEffects.PlayOneShot(death);
                    Destroy(gameObject);
                    PlayerController.score += 1000;
                }
            }
    }
}
