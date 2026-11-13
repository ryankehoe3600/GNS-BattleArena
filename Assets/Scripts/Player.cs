using UnityEngine;

public class Player : MonoBehaviour
{

    public float movementSpeed = 10f;
    Rigidbody rigidBody;
    public float fireRate = 0.75f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    
    private float nextFire = 0f;
    public AudioClip playerShootingAudio;
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
