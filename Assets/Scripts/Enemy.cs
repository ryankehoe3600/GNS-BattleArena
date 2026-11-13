using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float fireRate = 0.75f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    private float nextFire;

    public AudioClip playerShootingAudio;
    public GameObject bulletFiringEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            transform.LookAt(other.transform);
            Fire();
        }
    }


    void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            GameObject bullet = Instantiate(
                bulletPrefab, 
                bulletPosition.position, 
                bulletPosition.rotation
            );

            bullet.GetComponent<BulletController>()?
            .InitializeBullet(transform.rotation * Vector3.forward);

            AudioManager.Instance.Play3D(
                playerShootingAudio, 
                transform.position
            );

            VFXManager.Instance.PlayVFX(
                bulletFiringEffect, 
                bulletPosition.position
            );



        }
    
    }
}

