using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed at which the enemy moves between cells")] public float moveSpeed = 3f;
    [Tooltip("Number of cells to move in one step")] public int maxCellMoving = 1;

    [Header("Obstacle Detection")]
    [Tooltip("LayerMask for walls/obstacles")]
    public LayerMask obstacleMask;

    private CharacterController controller;
    private Vector3 targetPosition;
    private Vector3[] directions = new[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        Vector3 pos = transform.position;
        transform.position = new Vector3(SnapToHalf(pos.x), 0f, SnapToHalf(pos.z));

        targetPosition = transform.position;
        StartCoroutine(MoveRandomly());
    }

    private float SnapToHalf(float value)
    {
        return Mathf.Floor(value) + 0.5f;
    }

    private IEnumerator MoveRandomly()
    {
        while (true)
        {
            if (Door.Instance.IsGameActive() == false)
            {
                yield return null;
                continue;
            }

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                Vector3 current = transform.position;
                transform.position = new Vector3(SnapToHalf(current.x), 0f, SnapToHalf(current.z));

                targetPosition = CalculateNewTarget();
            }

            Vector3 horizontalDir = (targetPosition - transform.position);
            horizontalDir.y = 0f;
            horizontalDir.Normalize();

            Vector3 step = horizontalDir * moveSpeed * Time.deltaTime;
            Vector3 delta = targetPosition - transform.position;
            delta.y = 0f;

            if (step.sqrMagnitude >= delta.sqrMagnitude)
            {
                controller.Move(new Vector3(delta.x, 0f, delta.z));
            }
            else
            {
                controller.Move(new Vector3(step.x, 0f, step.z));
            }

            Vector3 after = transform.position;
            transform.position = new Vector3(after.x, 0f, after.z);

            yield return null;
        }
    }

    private Vector3 CalculateNewTarget()
    {
        List<Vector3> validDirs = new List<Vector3>();
        foreach (var dir in directions)
        {
            Vector3 checkPos = transform.position + dir * maxCellMoving;
            checkPos = new Vector3(SnapToHalf(checkPos.x), 0f, SnapToHalf(checkPos.z));

            if (!Physics.Raycast(transform.position, dir, maxCellMoving, obstacleMask))
            {
                validDirs.Add(dir);
            }
        }

        if (validDirs.Count == 0)
            return transform.position;

        Vector3 chosenDir = validDirs[UnityEngine.Random.Range(0, validDirs.Count)];
        Vector3 newPos = transform.position + chosenDir * maxCellMoving;
        return new Vector3(SnapToHalf(newPos.x), 0f, SnapToHalf(newPos.z));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetPosition, 0.1f);
    }
}
