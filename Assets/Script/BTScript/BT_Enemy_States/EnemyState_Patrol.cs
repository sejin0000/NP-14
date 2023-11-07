using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

public class EnemyState_Patrol : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;
    private Vector2 destination;   // 목적지
    private int patrolSpeed;


    private float ActionTime;      // 걷기 시간
    private float currentTime;     // 시간 계산용


    float destinationX = 0f;
    float destinationY = 0f;
    //임시
    public GameObject tempTarget;

    public EnemyState_Patrol(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;

        ActionTime = enemySO.actionTime;
        patrolSpeed = enemySO.patrolSpeed;


        enemyAI.nav.updateRotation = false;
        enemyAI.nav.updateUpAxis = false;

        //임시 타겟지정
        enemyAI.target = tempTarget;
    }

    
    //노드 Start()
    public override void Initialize()
    {

    }


    //노드 종료 순간 호출
    public override void Terminate()
    {
        
    }

    //노드 Update()
    public override Status Update()
    {
        Patrol();
        ElapseTime();
        return Status.BT_Running;
    }


    //목적지 리셋, 애니메이션, 액션타임, 스피드, 플립 등등 모두 초기화 관리
    private void Reset()
    {
        //anim.SetBool("isRun", true);
        currentTime = ActionTime;
        enemyAI.nav.speed = patrolSpeed;

        //목적지 리셋
        enemyAI.nav.ResetPath();

        //애니메이션 초기화
        //anim.SetBool("isRun", false); //anim.SetBool("Running", isRunning);

        float beforDestionX = destinationX;
        float beforDestionY = destinationY;

        destinationX = Random.Range(-6f, 6f);
        destinationY = Random.Range(-5f, 5f);
      

        destination.Set(destinationX, destinationY); // 랜덤 목적지 지정


        if (destinationX < beforDestionX)
        {
            enemyAI.spriteRenderer.flipX = true;
        }
        else
        {
            enemyAI.spriteRenderer.flipX = false;
        }

        //오버플로우 : 코드가 잘못됨  경로 계산 참거짓(시작/끝/네비매쉬 영역/결과 경로)
        /*
        while (!NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path))
        {
            destination.Set(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0); // 랜덤 목적지 지정
        }
        */

        Debug.Log("걷기");
    }


    //실제 이동
    private void Patrol()
    {
        enemyAI.nav.SetDestination(destination);
        //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        //리지드바디 이동(현재 위치에서 전방으로, 1초당 walkSpeed 수치만큼 이동     
    }

    //패턴 시간 계산용
    private void ElapseTime()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            Reset();
        }           
    }

    /*

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;
        if (collision.gameObject.tag == "Bullet")
        {
            Damage((int)obj.GetComponent<Bullet>()?.CurrentAtk);
        }
    }

    */


    /*
    //데미지 받는경우 = Enemy 타입에 따라 데미지를 받는 경우에 할 추가적인 행동(현상) 지정 가능
    //데드 셀렉터로 추가하자. 최우선 판정 Damage, Attack, RotateArm
    public virtual void Damage(int _dmg)
    {
        if (!isDead)
        {
            HP -= _dmg;

            if (HP <= 0)
            {
                Dead();
                //switch case / enemy type에 따라 골드 +
                Destroy(gameObject);
                return;
            }

            // 피격 사운드 재생
            //anim.SetTrigger("Hurt"); // 피격모션 실행
        }
    }

    public void Attack() // 피해량, 플레이어 위치 받아옴
    {
        if (!isDead)
        {

            Debug.Log("공격");
            isAttacking = true; // 공격상태 ON
            nav.ResetPath(); // 제자리에서 공격하도록 추격정지 (목적지 리셋/네비게이션 내장함수)    


            //플레이어를 바라보도록 설정

            //anim.SetTrigger("Attack"); // 공격 애니매이션

            //공격() 각도 바꿔준 후 -> 생성
            Vector3 direction = (target.transform.position - bulletSpawnPoint.transform.position).normalized;

            RotateArm(direction);

            Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);

            isAttacking = false;
        }
    }
    */

    /*
    private void RotateArm(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;

        enemyAI.EnemyAim.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    */
}
