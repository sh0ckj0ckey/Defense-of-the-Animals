using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : MonoBehaviour
{
    public GuardianType GuardianType;

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

    private float cactusAttackRate = 1.2f;          // ����Ƶ��(x��/��)
    private float cactusAttackTimer = 0;
    private float cactusAttackDamage = 20;            // �˺�ֵ

    private float soulStreamDamageRate = 40f;       // �˺�ֵ(�˺�/��)

    private float chainLightningAttackRate = 1.4f;  // ����Ƶ��(x��/��)
    private float chainLightningAttackTimer = 0;
    private float chainLightningAttackDamage = 40;    // �˺�ֵ
    private float chainLightningLeapDamage = 30;      // ��Ծ�˺�ֵ
    private float chainLightningSlowdownTime = 1;     // ����ʱ��(��)

    private int guardianLevel = 0;                  // �����ȼ�(����+1)
    private const int guardianMaxLevel = 1;               // Ŀǰ�������1��

    public Transform FirePosition;

    public GameObject CactusPrefab;

    public LineRenderer SoulStreamLineRenderer;
    public GameObject SoulStreamHitEffect;

    public GameObject LightningPrefab;

    void Start()
    {
        cactusAttackTimer = cactusAttackRate;
        animator = GetComponent<Animator>();

        HideSoulStream();
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
        if (this.GuardianType == GuardianType.Cactus)
        {
            if (enemiesList.Count > 0)
            {
                cactusAttackTimer += Time.deltaTime;
                if (cactusAttackTimer >= cactusAttackRate)
                {
                    cactusAttackTimer -= cactusAttackRate;
                    animator.SetTrigger("IsCactusAttacking");
                    StartCoroutine(DelayInvoker.DelayToInvoke(() =>
                    {
                        CactusAttack(cactusAttackDamage);
                    }, 0.3f));
                }
            }
        }
        else if (this.GuardianType == GuardianType.SoulStream)
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
                            enemiesList[0].GetComponent<Enemy>().TakeCactusDamage(soulStreamDamageRate * Time.deltaTime);

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
        else if (this.GuardianType == GuardianType.ChainLightning)
        {
            if (enemiesList.Count > 0)
            {
                chainLightningAttackTimer += Time.deltaTime;
                if (chainLightningAttackTimer >= chainLightningAttackRate)
                {
                    chainLightningAttackTimer -= chainLightningAttackRate;
                    animator.SetTrigger("IsLightningAttacking");
                    StartCoroutine(DelayInvoker.DelayToInvoke(() =>
                    {
                        ChainLightningAttack(chainLightningAttackDamage, chainLightningLeapDamage, chainLightningSlowdownTime);
                    }, 0.3f));
                }
            }
        }
    }

    /// <summary>
    /// ���������������˺�ֵ������
    /// </summary>
    public void UpgradeGuardian()
    {
        if (guardianLevel < guardianMaxLevel)
        {
            guardianLevel += 1;
        }

        this.cactusAttackDamage *= (1f + guardianLevel * 1.5f);

        this.soulStreamDamageRate *= (1f + guardianLevel * 1.8f);

        this.chainLightningAttackDamage *= (1f + guardianLevel * 1.5f);
        this.chainLightningLeapDamage *= (1f + guardianLevel);
        this.chainLightningSlowdownTime *= (1f + guardianLevel * 0.5f);
    }

    void CactusAttack(float damage)
    {
        if (enemiesList.Count <= 0 || enemiesList[0] == null)
        {
            UpdateEnemies();
        }

        if (enemiesList.Count > 0)
        {
            var bullet = GameObject.Instantiate(CactusPrefab, FirePosition.position, FirePosition.rotation);
            bullet.GetComponent<CactusBullet>().InitBullet(enemiesList[0].transform, damage);
        }
        else
        {
            cactusAttackTimer = cactusAttackRate;
        }

    }

    void ChainLightningAttack(float damage, float leapDamage, float slowdownTime)
    {
        if (enemiesList.Count <= 0 || enemiesList[0] == null)
        {
            UpdateEnemies();
        }

        if (enemiesList.Count > 0)
        {
            var bullet = GameObject.Instantiate(LightningPrefab, FirePosition.position, FirePosition.rotation);
            bullet.GetComponent<LightningBullet>().InitBullet(enemiesList[0].transform, damage, leapDamage, slowdownTime);
        }
        else
        {
            chainLightningAttackTimer = chainLightningAttackRate;
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
        if (this.GuardianType == GuardianType.SoulStream)
        {
            //SoulStreamLineRenderer.SetPositions(new Vector3[] { FirePosition.position, FirePosition.position });
            //SoulStreamLineRenderer.transform.position += SoulStreamHitEffect.transform.up * (-10);
            SoulStreamLineRenderer.enabled = false;

            SoulStreamHitEffect.transform.position = FirePosition.position;
            //SoulStreamHitEffect.transform.position += SoulStreamHitEffect.transform.up * (-20);
            var pos = SoulStreamHitEffect.transform.position;
            pos.y = -20;
            SoulStreamHitEffect.transform.position = pos;
        }
    }
}
