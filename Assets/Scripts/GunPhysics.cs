using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunPhysics : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float shootForce = 20f, reloadTime = 2.5f;
    private float currentAmmo = 69;
    private float maxAmmo = 69;
    private bool hasShot = false;
    public Text ammoText;
    public AudioSource soundEffects;
    public AudioClip laser;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && currentAmmo > 0 && currentAmmo <= maxAmmo)
        {
            Shoot();
            hasShot = true;
        }
        if (Input.GetKey(KeyCode.R) || currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
        ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }
    public void Shoot()
    {
        soundEffects.PlayOneShot(laser);
        currentAmmo--;
        RaycastHit hit;
        Ray ray = new Ray(bulletSpawnPoint.position, bulletSpawnPoint.forward);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            Debug.DrawRay(bulletSpawnPoint.position, bulletSpawnPoint.forward * 100, Color.red, 2f);
            Vector3 hitPoint = hit.point;
            Quaternion rotation = Quaternion.LookRotation(hitPoint - bulletSpawnPoint.position);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, rotation);
            bullet.GetComponent<MeshRenderer>().enabled = true;
            bullet.GetComponent<Collider>().enabled = true;
            Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
            bulletRB.useGravity = false;
            bulletRB.AddForce(bullet.transform.forward * shootForce, ForceMode.Impulse);
            StartCoroutine(DestroyBullet(bullet, 10f));
        }
    }
    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
    }
    private IEnumerator DestroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
    private void LateUpdate()
    {
        if (Input.GetButtonUp("Fire1") && hasShot)
        {
            hasShot = false;
        }
    }
}
