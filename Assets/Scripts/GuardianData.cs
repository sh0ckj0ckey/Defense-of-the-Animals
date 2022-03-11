using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuardianData
{
    public GameObject GuardianPrefab;
    public int Cost;
    public int UpgradeCost;
    public GuardianType type;
}

public enum GuardianType
{
    Cactus,         // ���˴���
    SoulStream,     // ��꼤��
    ChainLightning, // ��������
    ChaosMeteor     // ������ʯ
}
