using UnityEngine;

public class InputController : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 moveDirection;

    [Header("Bomb")]
    public bool isPlacingBomb;

    private void Update()
    {
        // MOVEMENT
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            moveDirection.Set(h, 0, v);
            moveDirection.Normalize();
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        // BOMB PLACEMENT
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPlacingBomb = true;
        }
    }
}

