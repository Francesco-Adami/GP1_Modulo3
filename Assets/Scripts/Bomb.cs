using System;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionTimer = 3f;
    [SerializeField] private float maximumExplosionRadius = 2f;
    [SerializeField] private LayerMask explosionMask;
    [SerializeField] private GameObject explosionPrefab;
    private RaycastHit[] hitsBuffer = new RaycastHit[4];

    private void Start()
    {
        StartCoroutine(ExplosionTimer());
    }

    private IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(explosionTimer);
        ExplodeOnGrid();
    }


    //private void Explode()
    //{
    //    Vector3[] dirs = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    //    foreach (var dir in dirs)
    //    {
    //        int hitCount = Physics.RaycastNonAlloc(
    //            transform.position,
    //            dir,
    //            hitsBuffer,
    //            maximumExplosionRadius,
    //            explosionMask
    //        );

    //        for (int i = 0; i < hitCount; i++)
    //        {
    //            HandleHit(hitsBuffer[i]);
    //            // se colpisci un muro solido -> break; se è distruttibile, continua a propagare
    //        }

    //        if (hitCount == 0)
    //        {
    //            Debug.Log($"Explosion reached end tile at {transform.position + dir * maximumExplosionRadius}");
    //        }
    //    }

    //    Destroy(gameObject);
    //}

    private void ExplodeOnGrid()
    {
        int power = Mathf.RoundToInt(maximumExplosionRadius);
        Vector3[] dirs = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
        foreach (var dir in dirs)
        {
            for (int step = 1; step <= power; step++)
            {
                Vector3 checkPos = transform.position + dir * step;
                Collider[] hits = Physics.OverlapSphere(checkPos, 0.4f, explosionMask);
                Instantiate(explosionPrefab, checkPos, Quaternion.identity);

                bool stop = false;
                foreach (var c in hits)
                {
                    if (c.CompareTag("Player"))
                    {
                        if (c.TryGetComponent<Player>(out var player))
                        {
                            player.Die();
                        }
                        stop = true;
                        break;
                    }
                    else if (c.CompareTag("Enemy"))
                    {
                        Destroy(c.gameObject);
                        Door.Instance.KillEnemy();
                        FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).SetEnemyText();
                        stop = true;
                        break;
                    }
                    else if (c.CompareTag("Wall"))
                    {
                        var bw = c.GetComponent<BreakableWall>();
                        if (bw != null) Destroy(c.gameObject);
                        stop = true;
                        break;
                    }
                }
                if (stop) break;
            }
        }
        Destroy(gameObject);
    }

    private IEnumerator DestroyRoutine(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }


    //private void HandleHit(RaycastHit hit)
    //{
    //    var t = hit.transform;
    //    if (t.CompareTag("Wall"))
    //    {
    //        Debug.Log("Wall hit: " + t.name);
    //        var bw = t.GetComponent<BreakableWall>();
    //        if (bw != null) Destroy(t.gameObject);
    //        // blocca propagazione se è muro indistruttibile
    //    }
    //    else if (t.CompareTag("Enemy"))
    //    {
    //        Destroy(t.gameObject);
    //    }
    //    else
    //    {
    //        Debug.Log("Explosion at: " + hit.point);
    //    }
    //}

}
