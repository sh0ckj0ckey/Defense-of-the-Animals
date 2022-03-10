using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : MonoBehaviour
{
    public enum AttackType
    {
        Cactus,     // ���˼��
        SoulStream, // ��꼤��
        ChaosMeteor // ������ʯ
    }

    public AttackType GuardianType;

    public List<GameObject> enemiesList = new List<GameObject>();

    Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemiesList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemiesList.Remove(other.gameObject);
        }
    }

    private float cactusAttackRate = 0.7f;      // ����Ƶ��(x��/��)
    private float cactusAttackTimer = 0;

    private float soulStreamDamageRate = 40f;    // �˺�ֵ(�˺�/��)

    public GameObject BulletPrefab;
    public Transform FirePosition;

    public LineRenderer SoulStreamLineRenderer;
    public GameObject SoulStreamHitEffect;

    void Start()
    {
        cactusAttackTimer = cactusAttackRate;
        animator = GetComponent<Animator>();
        //SoulStreamLineRenderer.transform.position += SoulStreamHitEffect.transform.up * (-10);
        SoulStreamLineRenderer.enabled = false;
    }

    void Update()
    {
        // ת���������
        if (enemiesList.Count > 0 && enemiesList[0] != null)
        {
            Vector3 targetPos = enemiesList[0].transform.position;
            targetPos.y = transform.position.y; // �����ȶ��룬ֻ��Ҫ�����ת��
            transform.LookAt(targetPos);
        }

        // ��������
        if (this.GuardianType == AttackType.Cactus)
        {
            if (enemiesList.Count > 0)
            {
                cactusAttackTimer += Time.deltaTime;
                if (cactusAttackTimer >= cactusAttackRate)
                {
                    cactusAttackTimer -= cactusAttackRate;
                    animator.SetBool("IsAttacking", true);
                    StartCoroutine(DelayInvoker.DelayToInvoke(() =>
                    {
                        Attack();
                    }, 0.3f));
                }
            }
            else
            {
                animator.SetBool("IsAttacking", false);
            }
        }
        else if (this.GuardianType == AttackType.SoulStream)
        {
            if (enemiesList.Count > 0)
            {
                if (enemiesList[0] == null)
                {
                    UpdateEnemies();
                }
                if (enemiesList.Count > 0)
                {
                    animator.SetBool("IsSpellcasting", true);

                    StartCoroutine(DelayInvoker.DelayToInvoke(() =>
                    {
                        if (enemiesList.Count > 0)
                        {
                            // ��������
                            if (SoulStreamLineRenderer.enabled == false)
                            {
                                SoulStreamLineRenderer.enabled = true;
                            }
                            var enemyPos = enemiesList[0].transform.position;
                            enemyPos.y = FirePosition.position.y;
                            SoulStreamLineRenderer.SetPositions(new Vector3[] { FirePosition.position, enemyPos });

                            // ����˺�
                            enemiesList[0].GetComponent<Enemy>().TakeDamage(soulStreamDamageRate * Time.deltaTime);

                            // �����Ч
                            SoulStreamHitEffect.transform.position = enemiesList[0].transform.position;

                            // ����Ч����ʩ����
                            Vector3 pos = transform.position;
                            pos.y = enemiesList[0].transform.position.y;
                            SoulStreamHitEffect.transform.LookAt(pos);

                            //����Ч��ʩ�����ƶ�һ����룬���ⱻ����ģ�͵�ס
                            SoulStreamHitEffect.transform.position += SoulStreamHitEffect.transform.forward * 1;
                            SoulStreamHitEffect.transform.position += SoulStreamHitEffect.transform.up * 1;
                        }
                        else
                        {
                            animator.SetBool("IsSpellcasting", false);

                            // û�е��˾Ͱ���Ч�ص�����ȥ
                            HideSoulStream();
                        }
                    }, 1.4f));
                }
            }
            else
            {
                animator.SetBool("IsSpellcasting", false);

                // û�е��˾Ͱ���Ч�ص�����ȥ
                HideSoulStream();
            }
        }
    }

    void Attack()
    {
        if (enemiesList.Count <= 0 || enemiesList[0] == null)
        {
            UpdateEnemies();
        }

        if (enemiesList.Count > 0)
        {
            var bullet = GameObject.Instantiate(BulletPrefab, FirePosition.position, FirePosition.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemiesList[0].transform);
        }
        else
        {
            cactusAttackTimer = cactusAttackRate;
        }

    }

    void UpdateEnemies()
    {
        for (int i = enemiesList.Count - 1; i >= 0; i--)
        {
            if (enemiesList[i] == null)
            {
                enemiesList.RemoveAt(i);
            }
        }
    }

    void HideSoulStream()
    {
        //SoulStreamLineRenderer.SetPositions(new Vector3[] { FirePosition.position, FirePosition.position });
        //SoulStreamLineRenderer.transform.position += SoulStreamHitEffect.transform.up * (-10);
        SoulStreamLineRenderer.enabled = false;

        SoulStreamHitEffect.transform.position = FirePosition.position;
        SoulStreamHitEffect.transform.position += SoulStreamHitEffect.transform.up * (-10);
    }
}
