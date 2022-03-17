using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusBullet : MonoBehaviour
{
    public float Speed = 20;

    public GameObject ExplosionEffectPrefab;

    private float distanceArriveTarget = 1.2f;

    private Transform target;
    private float damage = 20;

    public void InitBullet(Transform targetEnemy, float damage)
    {
        this.target = targetEnemy;
        this.damage = damage;
    }

    void Update()
    {
        if (target == null)
        {
            DestroyBullet();
            return;
        }

        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        Vector3 dir = target.position - transform.position;
        if (dir.magnitude < distanceArriveTarget)
        {
            target.GetComponent<Enemy>().TakeCactusDamage(damage);
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        var effect = GameObject.Instantiate(ExplosionEffectPrefab, transform.position, transform.rotation);
        Destroy(effect, 1);
        Destroy(this.gameObject);
    }

    // ��ײ�Ļ����ܻ����ӵ�������һ�����˹��������ײ���ڶ������ʹ�ڶ���������
    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.tag == "Enemy")
    //    {
    //        col.GetComponent<Enemy>().TakeDamage(Damage);
    //        GameObject.Instantiate(ExplosionEffectPrefab, transform.position, transform.rotation);
    //        Destroy(this.gameObject);
    //    }
    //}
}
