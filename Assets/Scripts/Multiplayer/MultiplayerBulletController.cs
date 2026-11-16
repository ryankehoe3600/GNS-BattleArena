using UnityEngine;

public class MultiplayerBulletController : MonoBehaviour
{
    Rigidbody rigidBody;

    public float bulletSpeed = 15f;
    public AudioClip bulletHitSound;
    public GameObject bulletImpactEffect;
    public int damage = 10;

    [HideInInspector]
    public Photon.Realtime.Player owner;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void InitializeBullet(
        Vector3 originalDirection,
        Photon.Realtime.Player givenPlayer)
    {
        transform.forward = originalDirection;
        rigidBody.linearVelocity = transform.forward * bulletSpeed;

        owner = givenPlayer;
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
