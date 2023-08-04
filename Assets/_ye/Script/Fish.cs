using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : WeakAnimal
{
    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }

    private void RandomAction()
    {

        int _random = Random.Range(0, 3); // ���, Ǯ���, �θ���, �ȱ�

        if (_random == 0)
            Wait();
        else if (_random > 0)
            TryWalk();
    }

    private void Wait()  // ���
    {
        currentTime = waitTime;
    }


}
