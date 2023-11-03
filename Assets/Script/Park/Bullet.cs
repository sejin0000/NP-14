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
    private SpriteRenderer _spriteRenderer;//�̹��� ��ĳ ������ �����صα�

    public bool canAngle = false;

    public PlayerStatHandler playerStatHandler;
    public GameObject player;

    public event Action BulletSetting;//�Ѿ� �����Ҷ� �̺�Ʈ�� �־ �߰�ȿ�� �ο� �ϴ¹������ ���°� ���; ������
    private void Awake()
    {
        canAngle = false;//����������÷��̾��ѿ� �����߰��ؼ��ޱ�
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        playerStatHandler = GameManager.Instance.Player.GetComponent<PlayerStatHandler>();
        player = GameManager.Instance.Player;

        //�Ʒ������׽�Ʈ�� �����
        //DirectionSetting(Vector2.one, 250, 3, 50);
        canAngle = true;
    }
    private void Update()
    {
        _currentDuration += Time.deltaTime;
        if (_currentDuration >= DuratoinTime) 
        {
            gameObject.SetActive(false);//or ��Ʈ����
        }
        _rigidbody.velocity = _direction * Time.deltaTime* MoveSpeed;
        lastVelocity = _rigidbody.velocity;
        //Debug.Log($"�ӵ� {MoveSpeed} ���ν�Ƽ {_rigidbody.velocity}");


    }

    public void DirectionSetting(Vector2 direction, float bulletSpeed, float TotalDamege, float bulletTime,TopDownCharacterController Controller) //
    {//�޴°� �ʹ� ������ ������ �÷��̾� �޴°� ��������
        _direction = direction;
        _currentDuration = 0;
        transform.right = _direction; //3d������ ������ �� �� ==�Ѿ��̳��󰡴¹���
        MoveSpeed = bulletSpeed;
        Damege= TotalDamege;
        DuratoinTime = bulletTime;
        TDCController= Controller;
    }

    void sizeControl() //ũ��== �÷��̾� ũ�� �ε� �̰� ����Ŭ������ �����Ұ� ������ �����̶�
    {
        transform.localScale = player.transform.localScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {//"1 << other.gameObject.layer"�� other.gameObject.layer�� �ش��ϴ� ��Ʈ�� Ȱ��ȭ��Ű�� ��
        // �̰� �����ݸ��� ���̾ �ٲܼ� ������ ��ų���ɶ� �� ������� �ٲ��ָ�ɵ�
        //���� ���̾ ���͸� �ְ� ���������� +�÷��̾ ���� ������
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            PlayerStatHandler stat = collision.gameObject.GetComponent<PlayerStatHandler>();
            if (stat != null)
            {
                //TDCController.CallHitEvent();
                //if()
                //���� ������
            }
        }
        else// ���̰� ���Ϳ��ϰ��� �÷��̾�,�� ������ ������ ���� �̰� ƨ��� �ִٸ� ƨ��� �ƴ϶�� ���������
        {
            if (canAngle) //�Ƹ� ��ƨ��������� ������ Ʈ�簡 �ɰ�
            {

                Vector3 income = _direction; // �Ի纤��
                Vector3 normal = collision.contacts[0].normal; // ��������
                _direction = Vector3.Reflect(income, normal).normalized; // �ݻ纤��
            }
            else
            {
                gameObject.SetActive(false);//or ��Ʈ����
            }
        }
    }
     

}
