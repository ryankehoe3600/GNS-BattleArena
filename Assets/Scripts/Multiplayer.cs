using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Multiplayer : MonoBehaviour, IPunObservable
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
    PhotonView photonView;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        Move();

        if (Input.GetKey(KeyCode.Space))
            photonView.RPC("Fire", RpcTarget.AllViaServer);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
           MultiplayerBulletController bullet = collision.gameObject.GetComponent<MultiplayerBulletController>();

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
        health = 100;
        healthBar.value = health;
    }

    [PunRPC]
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

            bullet.GetComponent<MultiplayerBulletController>()
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int)stream.ReceiveNext();
            healthBar.value = health;
        }
    }
}
