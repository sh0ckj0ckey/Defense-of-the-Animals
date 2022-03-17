using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� https://github.com/aceyan/UnityEffects
/// UV��ͼ������
/// ʹ����һ���ݹ�ķ����㷨���������������ε�λ�á�Ȼ������lineRender��position������ģ������Ч��������uv����Ч������
/// ����ԭ����ҵ�OneNote
/// </summary>

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class UVChainLightning : MonoBehaviour
{
    public float Detail = 1;        //���Ӻ�������������٣�ÿ�������������
    public float Displacement = 8; //λ������Ҳ����������ֵ����ƫ�Ƶ����ֵ

    public Transform ChainStart;
    public Transform ChainEnd;

    //public float yOffset = 0;

    private LineRenderer lineRender;
    private List<Vector3> linePosList;

    private void Awake()
    {
        lineRender = GetComponent<LineRenderer>();
        linePosList = new List<Vector3>();
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            linePosList.Clear();
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = Vector3.zero;
            if (ChainEnd != null)
            {
                endPos = ChainEnd.position;// + Vector3.up * yOffset;
            }
            if (ChainStart != null)
            {
                startPos = ChainStart.position;// + Vector3.up * yOffset;
            }

            CollectLinPos(startPos, endPos, Displacement);
            linePosList.Add(endPos);

            //_lineRender.SetVertexCount(_linePosList.Count);
            lineRender.positionCount = linePosList.Count;

            for (int i = 0, n = linePosList.Count; i < n; i++)
            {
                lineRender.SetPosition(i, linePosList[i]);
            }
        }
    }

    //�ռ����㣬�е���η���ֵ����
    private void CollectLinPos(Vector3 startPos, Vector3 destPos, float displace)
    {
        if (displace < Detail)
        {
            linePosList.Add(startPos);
        }
        else
        {

            float midX = (startPos.x + destPos.x) / 2;
            float midY = (startPos.y + destPos.y) / 2;
            float midZ = (startPos.z + destPos.z) / 2;

            midX += (float)(UnityEngine.Random.value - 0.5) * displace;
            midY += (float)(UnityEngine.Random.value - 0.5) * displace;
            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;

            Vector3 midPos = new Vector3(midX, midY, midZ);

            CollectLinPos(startPos, midPos, displace / 2);
            CollectLinPos(midPos, destPos, displace / 2);
        }
    }

    public void ClearChainLightning()
    {
        enabled = false;
        lineRender = null;
        linePosList.Clear();
        linePosList = null;
    }
}
