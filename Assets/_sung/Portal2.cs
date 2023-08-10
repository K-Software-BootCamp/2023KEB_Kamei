using UnityEngine;

public class Portal2 : MonoBehaviour
{
    public Transform destinationPortal; // ���� ��Ż�� Transform

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            // ���� ��Ż�� �����̵��� ��ġ ���
            Vector3 teleportPosition = destinationPortal.position + (other.transform.position - transform.position);

            // �����̵�
            other.transform.position = teleportPosition;
        }
    }
}
