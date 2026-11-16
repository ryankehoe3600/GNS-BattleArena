using UnityEngine;

public class MultiplayerBulletController : MonoBehaviour
{
    Rigidbody rigidBody;

    public float bulletSpeed = 15f;
    public AudioClip bulletHitSound;
    public GameObject bulletImpactEffect;
    public int damage = 10;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void InitializeBullet(Vector3 originalDirection)
    {
        transform.forward = originalDirection;
        rigidBody.linearVelocity = transform.forward * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)

    {
        AudioManager.Instance.Play3D(
            bulletHitSound, 
            transform.position
        );

        VFXManager.Instance.PlayVFX(
            bulletImpactEffect, 
            transform.position
        );

        Destroy(gameObject);
    }
    
}
