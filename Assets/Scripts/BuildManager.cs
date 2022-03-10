using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    public GuardianData normalTurret;
    public GuardianData advanceTurret;
    public GuardianData superTurret;

    // ��ǰѡ�е���̨
    public GuardianData selectedTurretData = null;

    public Text moneyText;

    public Animator moneyAnimator;

    public int money = 50;

    public void AddMoney(int change = 0)
    {
        money += change;
        moneyText.text = money.ToString();
    }

    private void Awake()
    {
        Instance = this;    //�ѵ�ǰ�Ķ��󸳸�Instance�����ⲿ����
    }

    private void Start()
    {
        selectedTurretData = normalTurret;
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
                        if (money >= selectedTurretData.cost)
                        {
                            AddMoney(-selectedTurretData.cost);
                            mapCube.BuildTurret(selectedTurretData.turretPrefab);
                        }
                        else
                        {
                            moneyAnimator.SetTrigger("Flick");
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
            selectedTurretData = normalTurret;
        }
    }
    public void OnAdvanceSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = advanceTurret;
        }
    }
    public void OnSuperSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = superTurret;
        }
    }
}
