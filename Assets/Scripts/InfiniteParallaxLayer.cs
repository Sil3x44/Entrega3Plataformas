using UnityEngine;

public class InfiniteParallaxLayer : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [Header("Sprites")]
    [SerializeField] private Transform[] pieces;
    [SerializeField] private float spriteWidth = 20f;

    [Header("Parallax")]
    [SerializeField] private float horizontalMultiplier = 0.5f;
    [SerializeField] private float verticalMultiplier = 1f;

    private Vector3 startPosition;
    private Vector3 startCameraPosition;

    private void Start()
    {
        startPosition = transform.position;
        startCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        MoveLayer();
        LoopPieces();
    }

    private void MoveLayer()
    {
        Vector3 cameraDelta = cameraTransform.position - startCameraPosition;

        transform.position = new Vector3(
            startPosition.x + cameraDelta.x * horizontalMultiplier,
            startPosition.y + cameraDelta.y * verticalMultiplier,
            startPosition.z);
    }

    private void LoopPieces()
    {
        foreach (Transform piece in pieces)
        {
            float pieceWorldX = piece.position.x;
            float cameraX = cameraTransform.position.x;

            if (pieceWorldX < cameraX - spriteWidth)
            {
                piece.position += Vector3.right * spriteWidth * pieces.Length;
            }
            else if (pieceWorldX > cameraX + spriteWidth)
            {
                piece.position += Vector3.left * spriteWidth * pieces.Length;
            }
        }
    }
}
