using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName; // ������ �̸�
    [SerializeField] protected int hp;  // ������ ü��

    [SerializeField] protected float walkSpeed;  // �ȱ� �ӷ�
    [SerializeField] protected float runSpeed;  // �޸��� �ӷ�
    [SerializeField] protected float turningSpeed;  // ȸ�� �ӷ�
    protected float applySpeed;

    protected Vector3 direction;  // ����

    // ���� ����
    protected bool isAction;  // �ൿ ������ �ƴ��� �Ǻ�
    protected bool isWalking; // �ȴ���, �� �ȴ��� �Ǻ�
    protected bool isRunning; // �޸����� �Ǻ�
    protected bool isDead;   // �׾����� �Ǻ�

    [SerializeField] protected float walkTime;  // �ȱ� �ð�
    [SerializeField] protected float waitTime;  // ��� �ð�
    [SerializeField] protected float runTime;  // �ٱ� �ð�
    protected float currentTime;

    // �ʿ��� ������Ʈ
    [SerializeField] protected Animator anim;
    protected AudioSource theAudio;

    [SerializeField] protected AudioClip[] sound_Normal;
    [SerializeField] protected AudioClip sound_Hurt;
    [SerializeField] protected AudioClip sound_Dead;

    protected Vector3 destination;  // ������

    // �ʿ��� ������Ʈ
    protected NavMeshAgent nav;

    protected void Start()
    {
        currentTime = waitTime;   // ��� ����
        isAction = true;   // ��⵵ �ൿ
        theAudio = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();
    }

    protected void Update()
    {
        if (!isDead)
        {
            Move();
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            Vector3 randomPosition = GetRandomPositionOnNavMesh();
            nav.SetDestination(randomPosition);
        }

    }
    Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 20f;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 20f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return transform.position;
        }
    }

    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)  // �����ϰ� ���� �ൿ�� ����
                ReSet();
        }
    }

    protected virtual void ReSet()  // ���� �ൿ �غ�
    {
        isAction = true;

        nav.ResetPath();

        isWalking = false;
        anim.SetBool("Walking", isWalking);
        isRunning = false;
        anim.SetBool("Running", isRunning);
        nav.speed = walkSpeed;

    }
    
    protected void TryWalk()  // �ȱ�
    {
        currentTime = walkTime;
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        nav.speed = walkSpeed;
        Debug.Log("�ȱ�");
    }

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_Hurt);
            anim.SetTrigger("Hurt");
            // Run(_targetPos);
        }
    }

    protected void Dead()
    {
        PlaySE(sound_Dead);

        isWalking = false;
        isRunning = false;
        isDead = true;

        anim.SetTrigger("Dead");

    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, sound_Normal.Length);  // ������ �ϻ� ����� 3 ��
        PlaySE(sound_Normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}


