using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Portal1 portal1; // ��Ż1 ������Ʈ
    public Portal2 portal2; // ��Ż2 ������Ʈ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            Portal1 enteredPortal1 = other.GetComponent<Portal1>();
            if (enteredPortal1 != null && portal2 != null)
            {
                // ���� ��Ż�� �����̵��� ��ġ ���
                Vector3 teleportPosition = portal2.destinationPortal.position + (transform.position - enteredPortal1.transform.position);

                // �����̵�
                transform.position = teleportPosition;
            }
        }
    }
}
