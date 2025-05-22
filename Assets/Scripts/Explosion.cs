using System;
using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionTimer = 0.5f;

    private void Start()
    {
        StartCoroutine(ExplosionTimer());
    }

    private IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(explosionTimer);
        Destroy(gameObject);
    }
}
