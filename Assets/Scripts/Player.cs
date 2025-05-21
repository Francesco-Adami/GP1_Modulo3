using System;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{

    CharacterController characterController;
    InputController inputController;

    public float speed = 5f;
    public bool canPlaceBomb = true;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputController = GetComponent<InputController>();
    }

    private void Update()
    {
        if (inputController.isPlacingBomb && canPlaceBomb)
        {
            // Handle bomb placement logic here
            Debug.Log("Placing bomb...");
            inputController.isPlacingBomb = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        characterController.Move(speed * Time.deltaTime * inputController.moveDirection);
    }
}
