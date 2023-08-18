using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTimeSecond; // ���� ���迡���� 100�� = ���� ������ 1��

    private bool isNight = false;

    [SerializeField] private float nightFogDensity; // �� ������ Fog �е�
    private float dayFogDensity; // �� ������ Fog �е�
    [SerializeField] private float fogDensityCalc; // ������ ����
    private float currentFogDensity;

    [SerializeField] private TextMeshProUGUI dayTimeText;
    private string h, m = "00";
    
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
        dayTimeText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // ��� �¾��� X �� �߽����� ȸ��. ���ǽð� 1�ʿ�  0.1f * secondPerRealTimeSecond ������ŭ ȸ��
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170) // x �� ȸ���� 170 �̻��̸� ��
            isNight = true;
        else if (transform.eulerAngles.x <= 10)  // x �� ȸ���� 10 ���ϸ� ��
            isNight = false;

        dayTimeText.text = TimeCheck();

        if (isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }

    private string TimeCheck()
    {
        float hh = transform.eulerAngles.x / 3600;
        float mm = (transform.eulerAngles.x % 3600) / 60;

        h = startZero(hh);
        m = startZero(mm);
        return "Time " + h + " : " + m;
    }
    public string startZero(float num)
    {
        return (num < 10) ? "0" + num : "" + num;
    }
}
