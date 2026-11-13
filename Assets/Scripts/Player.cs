using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float movementSpeed = 10f;
    Rigidbody rigidBody;
    public float fireRate = 0.75f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    
    private float nextFire = 0f;
    public AudioClip playerShootingAudio;
    public GameObject bulletFiringEffect;
    [HideInInspector]
    public int health = 100;
    public Slider healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();

        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
           BulletController bullet = collision.gameObject.GetComponent<BulletController>();

           TakeDamage(bullet.damage);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.value = health;

        if (health <= 0)
        {
            PlayerDied();
        }
    }

    void PlayerDied()
    {
        gameObject.SetActive(false);
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

    void Move()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            return;
        
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");

        var rotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0, verticalInput));

        transform.rotation = rotation;

        var movementDir = transform.forward * Time.fixedDeltaTime * movementSpeed;

        rigidBody.MovePosition(rigidBody.position + movementDir);
        
    
    }
}
