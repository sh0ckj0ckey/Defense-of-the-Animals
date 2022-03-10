using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour
{
    [HideInInspector]
    public GameObject turretGo; //��ǰcube�Ϸ��õ���̨

    public GameObject buildEffect;

    private Renderer render;

    private Material originMaterial = null;

    public Material hoverMaterial;

    private void Start()
    {
        render = GetComponent<Renderer>();
        originMaterial = render.material;
    }

    public void BuildTurret(GameObject turretPrefab)
    {
        turretGo = GameObject.Instantiate(turretPrefab, transform.position, Quaternion.identity);           //�Ƕ� 4��0

        var pos = transform.position;
        pos.y += 2f;
        GameObject effect = GameObject.Instantiate(buildEffect, pos, buildEffect.transform.rotation);    //��������ԭ�������ĽǶ�
        Destroy(effect, 2);
    }

    // ����һ������֮�����������ײ��Χ��hover����Ͳ��ܱ�ɫ�ˣ�ȥProject Setting�����Physic��Queries Trigger Hit�ص�
    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (turretGo == null)
            {
                render.material = hoverMaterial;
                render.material.color = Color.green;
            }
            //else
            //{
            //    renderer.material = hoverMaterial;
            //    renderer.material.color = Color.red;
            //}
        }
    }

    private void OnMouseExit()
    {
        render.material = originMaterial;
        render.material.color = Color.white;
    }
}
