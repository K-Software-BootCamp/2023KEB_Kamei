using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : StrongAnimal
{
    protected override void Update()
    {
        base.Update();
        if (theFieldOfViewAngle.View() && !isDead)
        {
            StopAllCoroutines();
            StartCoroutine(ChaseTargetCoroutine());
        }
    }

    IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;
        Chase(theFieldOfViewAngle.GetTargetPos());

        while (currentChaseTime < chaseTime)
        {
            Chase(theFieldOfViewAngle.GetTargetPos());
            yield return new WaitForSeconds(chaseDelayTime);
            currentChaseTime += chaseDelayTime;
        }

        isChasing = false;
        isRunning = false;
        anim.SetBool("Running", isRunning);
        nav.ResetPath();
    }

    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }

    private void RandomAction()
    {
        RandomSound();

        int _random = Random.Range(0, 2); // ���, �ȱ�

        if (_random == 0)
            Wait();
        else if (_random == 1)
            TryWalk();
    }

    private void Wait()  // ���
    {
        currentTime = waitTime;
        Debug.Log("���");
    }
}