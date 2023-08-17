using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    [SerializeField]
    protected int attackDamage; // ���� ������
    [SerializeField]
    protected float attackDelay; // ���� ������
    [SerializeField]
    protected float attackDistance; // ���� ������ ���� �Ÿ�. �� �ȿ� �÷��̾ ������ ������ �ִٰ� �Ǵ�.
    [SerializeField]
    protected LayerMask targetMask; // �÷��̾� ���̾ �Ҵ� �� ��.

    [SerializeField]
    protected float chaseTime;  // �� �߰� �ð�
    protected float currentChaseTime;
    [SerializeField]
    protected float chaseDelayTime; // �߰� ������

    public void Chase(Vector3 _targetPos)
    {
        destination = _targetPos;

        isChasing = true;

        isWalking = false;
        isRunning = true;
        nav.speed = runSpeed;

        anim.SetBool("Running", isRunning);

        if (!isDead)
            nav.SetDestination(destination);
    }

    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)
            Chase(_targetPos);
    }

    protected IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;
        Chase(theFieldOfViewAngle.GetTargetPos());

        while (currentChaseTime < chaseTime)
        {
            Chase(theFieldOfViewAngle.GetTargetPos());
            // �÷��̾�� ����� ������ �ְ� 
            if (Vector3.Distance(transform.position, theFieldOfViewAngle.GetTargetPos()) <= attackDistance)
            {
                if (theFieldOfViewAngle.View())  // �� �տ� ���� ���
                {
                    Debug.Log("�÷��̾� ���� �õ�");
                    StartCoroutine(AttackCoroutine());
                }
            }
            yield return new WaitForSeconds(chaseDelayTime);
            currentChaseTime += chaseDelayTime;
        }

        isChasing = false;
        isRunning = false;
        anim.SetBool("Running", isRunning);
        nav.ResetPath();
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        // ������ ���ڸ����� �̷������ ��.
        nav.ResetPath();
        currentChaseTime = chaseTime;

        // ������ ����� �ٶ󺸰� �ϵ���. (0.5�� ����� ��)
        yield return new WaitForSeconds(0.5f);
        transform.LookAt(theFieldOfViewAngle.GetTargetPos());

        // ���� �ִϸ��̼��� ������� ����� �� �������� �������� ���
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);

        RaycastHit _hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, attackDistance, targetMask))
        {
            Debug.Log("�÷��̾ ����!!");
            HPBar.curHp = HPBar.curHp - attackDamage;
        }
        else
        {
            Debug.Log("�÷��̾� ������!!");
        }
        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
        StartCoroutine(ChaseTargetCoroutine());
    }
}