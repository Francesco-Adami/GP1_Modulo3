using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public int playerHealth = 3;
    public float speed = 5f;

    [Header("Bomb Stats")]
    public bool canPlaceBomb = true;
    public float bombCooldown = 1f;
    public GameObject bombPrefab;

    CharacterController characterController;
    InputController inputController;

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
            StartCoroutine(PlaceBomb());
        }
    }

    float SnapToHalf(float value)
    {
        return Mathf.Floor(value) + 0.5f;
    }

    private IEnumerator PlaceBomb()
    {
        canPlaceBomb = false;
        inputController.isPlacingBomb = false;

        // Instantiate the bomb at the player's position
        Vector3 bombPosition = new Vector3(
            SnapToHalf(transform.position.x),
            0,
            SnapToHalf(transform.position.z));
        Instantiate(bombPrefab, bombPosition, Quaternion.identity);

        yield return new WaitForSeconds(bombCooldown);
        canPlaceBomb = true;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        characterController.Move(speed * Time.deltaTime * inputController.moveDirection);
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            await Die();
        }
    }

    public async Task Die()
    {
        Door.Instance.PauseGame();
        GetComponentInChildren<MeshRenderer>().enabled = false;

        await Task.Delay(750);
        UIManager.Instance.ShowUI(UIManager.GameUI.Lose);
        Destroy(gameObject);
    }
}
