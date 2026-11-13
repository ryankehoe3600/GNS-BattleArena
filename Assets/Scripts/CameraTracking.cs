using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform playerCharacter;

    private Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - playerCharacter.position;
    }

    void LateUpdate()
    {
        transform.position = playerCharacter.position + cameraOffset;
    }
}
