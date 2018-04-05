using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    private int _damage;

    public float speed = 55f;
    public GameObject ImpactEffect;

    public void Seek(Transform target, int damage)
    {
        _target = target;
        _damage = damage;
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        GameObject effect = Instantiate(ImpactEffect, transform.position, transform.rotation);
        Destroy(effect, 2f);
        
        //Do damage to the target
        Unit targetUnit = _target.GetComponent<Unit>();
        targetUnit.DoDamage(_damage);
        
        Destroy(gameObject);
    }
}
