using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Bullet1 : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;
    private float MoveSpeed;
    public float Damege;
    public float DuratoinTime;
    private float _currentDuration;
    private Vector2 _direction;
    TopDownCharacterController TDCController;

    private Rigidbody2D _rigidbody;
    private Vector3 lastVelocity;
    private SpriteRenderer _spriteRenderer;//이미지 어캐 넣을지 생각해두기

    public bool canAngle = false;

    public PlayerStatHandler playerStatHandler;
    public GameObject player;

    public event Action BulletSetting;//총알 세팅할때 이벤트를 넣어서 추가효과 부여 하는방식으로 가는건 어떨까싶어서 만들어만둠
    private void Awake()
    {
        canAngle = false;//문제생기면플레이어총에 스탯추가해서받기
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        playerStatHandler = GameManager.Instance.Player.GetComponent<PlayerStatHandler>();
        player = GameManager.Instance.Player;

        //아래부터테스트용 지울것
        //DirectionSetting(Vector2.one, 250, 3, 50);
        canAngle = true;
    }
    private void Update()
    {
        _currentDuration += Time.deltaTime;
        if (_currentDuration >= DuratoinTime) 
        {
            gameObject.SetActive(false);//or 디스트로이
        }
        _rigidbody.velocity = _direction * Time.deltaTime* MoveSpeed;
        lastVelocity = _rigidbody.velocity;
        //Debug.Log($"속도 {MoveSpeed} 벨로시티 {_rigidbody.velocity}");


    }

    public void DirectionSetting(Vector2 direction, float bulletSpeed, float TotalDamege, float bulletTime,TopDownCharacterController Controller) //
    {//받는게 너무 많은거 같은데 플레이어 받는게 좋을지도
        _direction = direction;
        _currentDuration = 0;
        transform.right = _direction; //3d에서의 포워드 즉 앞 ==총알이날라가는방향
        MoveSpeed = bulletSpeed;
        Damege= TotalDamege;
        DuratoinTime = bulletTime;
        TDCController= Controller;
    }

    void sizeControl() //크기== 플레이어 크기 인데 이거 따로클래스를 내야할거 같은데 증강이라
    {
        transform.localScale = player.transform.localScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {//"1 << other.gameObject.layer"는 other.gameObject.layer에 해당하는 비트를 활성화시키는 것
        // 이거 레벨콜리전 레이어를 바꿀수 있으니 팀킬가능때 저 밸류값만 바꿔주면될듯
        //위에 레이어를 몬스터만 넣고 증강먹을때 +플레이어를 넣을 생각임
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            PlayerStatHandler stat = collision.gameObject.GetComponent<PlayerStatHandler>();
            if (stat != null)
            {
                //TDCController.CallHitEvent();
                //if()
                //대충 데미지
            }
        }
        else// 즉이건 몬스터외일거임 플레이어,벽 정도만 생각됨 이제 이건 튕길수 있다면 튕기고 아니라면 사라지겠지
        {
            if (canAngle) //아마 벽튕기는증갈을 먹으면 트루가 될것
            {

                Vector3 income = _direction; // 입사벡터
                Vector3 normal = collision.contacts[0].normal; // 법선벡터
                _direction = Vector3.Reflect(income, normal).normalized; // 반사벡터
            }
            else
            {
                gameObject.SetActive(false);//or 디스트로이
            }
        }
    }
     

}
