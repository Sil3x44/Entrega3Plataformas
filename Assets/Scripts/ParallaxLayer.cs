using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform visual;

    [SerializeField] private float horizontalParallax = 0.2f;
    [SerializeField] private float verticalFollow = 1f;

    private Vector3 startCameraPosition;
    private Vector3 visualStartLocalPosition;

    private void Start()
    {
        startCameraPosition = cameraTransform.position;
        visualStartLocalPosition = visual.localPosition;
    }

    private void LateUpdate()
    {
        Vector3 cameraDelta = cameraTransform.position - startCameraPosition;

        transform.position = new Vector3(
            cameraTransform.position.x,
            cameraTransform.position.y * verticalFollow,
            transform.position.z);

        visual.localPosition = new Vector3(
            visualStartLocalPosition.x - cameraDelta.x * horizontalParallax,
            visualStartLocalPosition.y,
            visualStartLocalPosition.z);
    }
}
