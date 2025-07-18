using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    [SerializeField]
    private Field field;

    [SerializeField]
    private float cameraHeight = 10f;

    [SerializeField]
    private float cameraAngle = 45f;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("No camera found!");
                return;
            }
        }
    }

    private void Update()
    {
        SetupIsometricCamera();
    }

    private void Start()
    {
        SetupIsometricCamera();
    }

    private void SetupIsometricCamera()
    {
        if (field == null)
        {
            Debug.LogWarning("Field reference is not set in IsometricCameraController!");
            return;
        }

        transform.rotation = Quaternion.Euler(cameraAngle, -45f, 0f);

        Vector3 centerPosition = new Vector3(0, 0, 0);
        transform.position = new Vector3(centerPosition.x, cameraHeight, centerPosition.z);

        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 5f;
    }
}
