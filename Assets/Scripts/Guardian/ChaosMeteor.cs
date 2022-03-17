using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosMeteor : MonoBehaviour
{
    // ����������Լ���һ������Layer����ChaosMeteor�����һƬ���û�й���
    // ��Ҫ��MainScene��DirectionalLight�����������CullingMask�������LayerҲ��ӽ�ȥ

    //private Vector3 spawnOffset = new Vector3(5, 5, 5);

    private Vector3 targetPosition = new Vector3(0, 0, 0);

    // �����ڵ����������Զ����
    public float TravelDistance = 200;
    // Ϊ�˷�ֹ�����������ͼ��Ե�ȵط�����סλ�ã����Լ�һ��������ʱ�䣬���������ʱ��ҲҪ���ٻ���
    public float TravelMaxDuration = 8;
    private float travelTimer = 0;

    public float flySpeed, groundSpeed, meteorSize, spinSpeed, pulseTime, pulseRadius;
    public LayerMask GroundLayer;

    // �����Y���꣬����������ֵ�����ٵ�
    public float GroundYAxis = 20;

    private GameObject impactEffect;
    private float impactEffectDestroyDelay = 3;

    private Vector3 moveDirection, landLocation;
    private float pulseTimer;

    private bool isMeteorFlying = true;

    public void SetTarget(Vector3 tar)
    {
        targetPosition = tar;
        moveDirection = transform.position - targetPosition;

        Vector3 relativePos = moveDirection - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        impactEffect = transform.Find("ImpactEffect").gameObject;

        //moveDirection = transform.position - (transform.position + spawnOffset);
        //transform.position = transform.position + targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (isMeteorFlying)
        {
            if (this.transform.position.y < GroundYAxis)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = transform.position + moveDirection * flySpeed * Time.deltaTime;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, meteorSize, GroundLayer))
            {
                isMeteorFlying = false;
                moveDirection.y = 0;
                landLocation = hit.point;

                impactEffect.SetActive(true);
                impactEffect.transform.SetParent(null);
                Destroy(impactEffect.gameObject, impactEffectDestroyDelay);
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, meteorSize, GroundLayer))
            {
                Vector3 nextPos = new Vector3(transform.position.x, hit.point.y + meteorSize, transform.position.z);
                transform.position = nextPos + moveDirection * groundSpeed * Time.deltaTime;
            }

            travelTimer += Time.deltaTime;

            float dist = Vector3.Distance(landLocation, transform.position);
            if (dist > TravelDistance || travelTimer > TravelMaxDuration)
            {
                Destroy(gameObject);
                return;
            }
            float spin = spinSpeed * Time.deltaTime;
            transform.Rotate(spin, 0, 0);
        }
    }

    void DamagePulses()
    {
        if (pulseTimer > 0)
        {
            pulseTimer -= Time.deltaTime;
        }
        else
        {
            Collider[] gameObjsInRange = Physics.OverlapSphere(transform.position, pulseRadius);
            foreach (var item in gameObjsInRange)
            {
                if (item.tag != "Enemy")
                {
                    continue;
                }

                Rigidbody enemy = item.GetComponent<Rigidbody>();
                if (enemy != null)
                {
                    //enemy.Damage();
                }
            }

            pulseTimer = pulseTime;
        }
    }
}
