using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    public GuardianData CactusGuardian;
    public GuardianData SoulStreamGuardian;
    public GuardianData ChainLightningGuardian;
    public GuardianData ChaosMeteor;

    // ��ǰѡ�е�����
    private GuardianData selectedGuardian = null;

    public Text MoneyText;

    public Animator MoneyAnimator;

    public int Money = 50;

    public void AddMoney(int change = 0)
    {
        Money += change;
        MoneyText.text = Money.ToString();
    }

    private void Awake()
    {
        Instance = this;    //�ѵ�ǰ�Ķ��󸳸�Instance�����ⲿ����
    }

    private void Start()
    {
        selectedGuardian = CactusGuardian;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                var sss = LayerMask.GetMask("MapCube");
                bool isCollider = Physics.Raycast(ray, out hit, 1000, sss);
                if (isCollider)
                {
                    MapCube mapCube = hit.collider.GetComponent<MapCube>();   // �õ������MapCube
                    if (mapCube.turretGo == null)
                    {
                        if (Money >= selectedGuardian.Cost)
                        {
                            AddMoney(-selectedGuardian.Cost);
                            mapCube.BuildTurret(selectedGuardian.GuardianPrefab);
                        }
                        else
                        {
                            MoneyAnimator.SetTrigger("Flick");
                        }
                    }
                    else
                    {
                        //����
                    }
                }
            }
        }
    }

    public void OnNormalSelected(bool isOn)
    {
        if (isOn)
        {
            selectedGuardian = CactusGuardian;
        }
    }
    public void OnAdvanceSelected(bool isOn)
    {
        if (isOn)
        {
            selectedGuardian = SoulStreamGuardian;
        }
    }
    public void OnSuperSelected(bool isOn)
    {
        if (isOn)
        {
            selectedGuardian = ChainLightningGuardian;
        }
    }
}
