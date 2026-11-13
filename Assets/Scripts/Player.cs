using UnityEngine;

public class Player : MonoBehaviour
{

    public float movementSpeed = 10f;
    Rigidbody rigidBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
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
