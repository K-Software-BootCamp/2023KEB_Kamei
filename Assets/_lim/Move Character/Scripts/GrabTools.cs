using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTools : MonoBehaviour
{
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Use()
    {
        StopCoroutine(Swing());
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
        
    }
}
