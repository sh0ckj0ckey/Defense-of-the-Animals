using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҫԭ���ǰ������position��rotation�仯��Sin��Cos�˶���������ʾ��ʵ�����Ч����
/// 
/// �������߿��Ա�ʾΪy=Asin(��x+��)+k������Ϊ����y=Asin(��x+��)+k��ֱ������ϵ�ϵ�ͼ��
/// ����sinΪ���ҷ��ţ�x��ֱ������ϵx���ϵ���ֵ��y����ͬһֱ������ϵ�Ϻ�����Ӧ��yֵ��k���غͦ��ǳ�����k���ء��ա�R�Ҧء�0����
/// 
/// ����������⣬A���˶����ߵ���������ǽ��ٶȣ���СΪ2��*f��f=1/T���������������ڣ���Ϊx=0ʱ����λ��kΪƫ�࣬��������y�������ƶ���ֵ�������Ϳ����������ʽ����ʾ�����˶��ˡ�
/// </summary>
public class DriftingController : MonoBehaviour
{
    //X��Y��Z�����������ߵĦ�ֵ
    public float wX = 1;
    public float wY = 1;
    public float wZ = 1;
    //X��Y��Z�����������ߵ�Aֵ
    public float aX = 0.2f;
    public float aY = 0.2f;
    public float aZ = 0.2f;
    public float speed = 1f;
    public float range = 1;
    private float t = 0;
    private Vector3 originPos;
    private Vector3 originRotation;

    void Awake()
    {
        originPos = transform.position;
        originRotation = transform.eulerAngles;
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        transform.position = originPos + new Vector3(aX * Mathf.Sin(wX * t), aY * Mathf.Sin(wY * t), aZ * Mathf.Sin(wZ * t)) * range;
        transform.rotation = Quaternion.Euler(originRotation + new Vector3(aX * 20 * Mathf.Sin(wX * t), aY * 20 * Mathf.Sin(wY * t), aZ * 20 * Mathf.Sin(wZ * t)));
    }
}
