using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : MonoBehaviourPunCallbacks //���������� ������ �ҷ����°� AugmentManager.Instance.Invoke(code,0); ������ �ش� �����ҷ���
{
    public static AugmentManager Instance;
    public PlayerStatHandler playerstatHandler;
    public GameObject player;

    public PhotonView PV;

    int atk = 5;
    int hp = 10;
    float speed = 1;
    float atkspeed = 1f;
    float bulletSpread = -1f;
    int cooltime = -1;
    int critical = 1;
    int AmmoMax = 1;

    // Start is called before the first frame update
    private void Start()
    {
        Instance = this;
        playerstatHandler = GameManager.Instance.Player.GetComponent<PlayerStatHandler>();
        player = GameManager.Instance.Player.gameObject;
    }

        //private void A999(PlayerStatHandler PlayerStat)
    //ó���ǵ� ������ ������ �� �� + �ϴ°��̴� �� ����������� >> 0+0���� �ڵ� ������ �ѹ��� �ϴ°� �����žƴ�??
    //{
        //StatAugment pick = MakeAugmentListManager.stat1[0] as StatAugment;
        //PlayerStat.HP.added = pick.Health;
        //PlayerStat.ATK.added = pick.Atk;
        //PlayerStat.AtkSpeed.added = pick.AtkSpeed;
        //���⼭ ���ٰ� ���� ��г��� ���ص� ���߿� ���� �Լ� 
    //}
private void A901()//���� �� Ƽ�� 1
    {
        playerstatHandler.ATK.added += atk;
        Debug.Log(playerstatHandler.ATK.added);
    }
    private void A902()//���� ü Ƽ�� 1
    {
        playerstatHandler.HP.added += hp;
        Debug.Log(playerstatHandler.HP.added);
    }
    private void A903()//���� �̼� Ƽ�� 1
    {
        playerstatHandler.Speed.added += speed;
        Debug.Log(playerstatHandler.Speed.added);

    }
    private void A904()//���� ���� Ƽ�� 1
    {
        playerstatHandler.AtkSpeed.added += atkspeed;
        Debug.Log(playerstatHandler.AtkSpeed.added);
    }
    private void A905()//���� ���е� Ƽ�� 1 ź������ �̻��ؼ� ���е��� �ٲ�µ� ��������? �����ǹٲ㵵��
    {
        playerstatHandler.BulletSpread.added += bulletSpread;
        Debug.Log(playerstatHandler.BulletSpread.added);
    }
    private void A906()//���� ��ų��Ÿ�� Ƽ��1
    {
        playerstatHandler.SkillCoolTime.added += cooltime;
        Debug.Log(playerstatHandler.SkillCoolTime.added);
    }
    private void A907()//���� ġ��Ÿ Ƽ��1
    {
        playerstatHandler.Critical.added += critical;
        Debug.Log(playerstatHandler.Critical.added);
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ����2Ƽ��
    private void A911()//���� �� Ƽ�� 2 @@@@@@@@@@@@@@@@@�̾Ʒ� �������� �ؾ���
    {
        playerstatHandler.ATK.added += atk * 2;
    }
    private void A912()//���� ü Ƽ�� 2
    {
        playerstatHandler.HP.added += hp * 2;
    }
    private void A913()//���� �̼� Ƽ�� 2
    {
        playerstatHandler.Speed.added += speed * 2;

    }
    private void A914()//���� ���� Ƽ�� 2
    {
        playerstatHandler.AtkSpeed.added += atkspeed * 2;
    }
    private void A915()//���� ���е� Ƽ�� 2
    {
        playerstatHandler.BulletSpread.added += bulletSpread * 2;
    }
    private void A916()//���� ��ų��Ÿ�� Ƽ��2
    {
        playerstatHandler.SkillCoolTime.added += cooltime * 2;
    }
    private void A917()//���� ġ��Ÿ Ƽ��2
    {
        playerstatHandler.Critical.added += critical * 2;
    }
    private void A918()//���� ġ��Ÿ Ƽ��2
    {
        playerstatHandler.AmmoMax.added += AmmoMax;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@����3Ƽ��
    private void A921()//���� �� Ƽ�� 3
    {
        playerstatHandler.ATK.added += atk * 3;
    }
    private void A922()//���� ü Ƽ�� 3
    {
        playerstatHandler.HP.added += hp * 3;
    }
    private void A923()//���� �̼� Ƽ�� 3
    {
        playerstatHandler.Speed.added += speed * 3;

    }
    private void A924()//���� ���� Ƽ�� 3
    {
        playerstatHandler.AtkSpeed.added += atkspeed * 3;
    }
    private void A925()//���� ���е� Ƽ�� 3
    {
        playerstatHandler.BulletSpread.added += bulletSpread * 3;
    }
    private void A926()//���� ��ų��Ÿ�� Ƽ��3
    {
        playerstatHandler.SkillCoolTime.added += cooltime * 3;
    }
    private void A927()//���� ġ��Ÿ Ƽ��3
    {
        playerstatHandler.Critical.added += critical * 3;
    }
    private void A928()//���� ġ��Ÿ Ƽ��3
    {
        playerstatHandler.AmmoMax.added += AmmoMax * 2;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@����1Ƽ��
    private void A101()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A102()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A103()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A104()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A105()// �������� //���� �ִ� ü���� 1�� ����� �� �� �� ���� ��ŭ ����
    {
        float up = ((int)playerstatHandler.HP.total - 1);
        playerstatHandler.HP.added -= up;
        playerstatHandler.ATK.added += up * 0.5f;
    }
    private void A106()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A107()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A108()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A109()// ����ȭ //�׽�Ʈ���غ�
    {
        float x = (player.transform.localScale.x * 0.75f);//����
        float y = (player.transform.localScale.y * 0.75f);//����
        player.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient -= 0.1f;
        playerstatHandler.Speed.coefficient += 0.2f;
    }
    private void A110()//����ȭ // �׽�Ʈ���غ�
    {
        float x = (player.transform.localScale.x * 1.25f);
        float y = (player.transform.localScale.y * 1.25f);
        player.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient += 0.5f;
        playerstatHandler.Speed.coefficient += 0.2f;
    }
    private void A111()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A112()//��������
    {
        playerstatHandler.ReloadCoolTime.added -= 0.3f;
    }
    private void A113()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A114()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A115()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A116()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A117()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A118()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A119()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A120()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A121()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A122()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A123()
    {
        Debug.Log("�̿ϼ�");
    }
    //�׽�Ʈ �Ϸ� �׷��� �̺�Ʈ�� �۵��Ͽ� �߰� �׽�Ʈ�� �ʿ�
    private void A124()//���������� : �þ߰� ���� ���� �ϸ� ���� �ӵ�, ������ �ӵ��� �����մϴ�.
    {
        player.AddComponent<A0124>();//A0124���� ȭ���Ӱ� �ϴ� ������ ����� �����������ۿ� ON ���� OFF
        playerstatHandler.AtkSpeed.added += 15;
        playerstatHandler.ReloadCoolTime.added += 15;
    }
    private void A125()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A126()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A127()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A128()
    {
        Debug.Log("�̿ϼ�");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ����2Ƽ��
    private void A201()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A202()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A203()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A204()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A205()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A206()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A207()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A208()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A209()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A210()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A211()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A212()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A213()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A214()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A215()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A216()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A217()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A218()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A219()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A220()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A221()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A222()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A223()
    {
        Debug.Log("�̿ϼ�");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 3Ƽ��
    private void A301()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A302()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A303()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A304()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A305()
    {
        Debug.Log("�̿ϼ�");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@�������� 1Ƽ��
    private void A1101()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1102()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1103()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1104()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1105() 
    {
        Debug.Log("�̿ϼ�");

    }
    private void A1106()
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1107()
    {
        if (PV.IsMine)
        {
            GameObject Prefabs = Resources.Load<GameObject>("AugmentList/A1107");
            //������ �ε��ϴ°ɷ� ���ҽ� �ε� = �ҷ����Ⱑ ��û���ſ� =���۶� ó���Ұ�, Ȥ�� �ּ��Ѹ� �÷����ϰ� ��ġ�� �صѰ�
            //�������� �ѹ��� �ε��Ѵ� ��ŸƮ Ȥ�� �����ũ���� �ε��ؼ� �����Ұ�
            GameObject fire = Instantiate(Prefabs, player.transform);
            //fire.transform.SetParent(player.transform);
            A1107 a1107 = fire.GetComponent<A1107>();
            a1107.Init(playerstatHandler);
            // �� ���ݱ��� ���� ���� �޾ƿԴµ� �����Ҷ� ���� �༭ ������ �Ұ� -
        }
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@�������� 2Ƽ��
    private void A1201()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1202()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1203()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1204()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1205()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1206()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1207()
    {
        Debug.Log("�̿ϼ�");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@�������� 3Ƽ��
    private void A1301()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1302()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1303()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A1304()
    {
        Debug.Log("�̿ϼ�");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 1Ƽ��
    private void A2101()//����� = ��ų����� ���� ���� �׽�Ʈ ��
    {
        player.AddComponent<A2101>();
    }
    private void A2102() ///�ʹٴٴ٤��ٴ� �׽�Ʈ���� �ٵ� �����̶� �����������
    {
        playerstatHandler.AtkSpeed.coefficient *= 2;
        playerstatHandler.ATK.coefficient *= 0.5f;
    }
    private void A2103()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A2104()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A2105()
    {
        Debug.Log("�̿ϼ�");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 2Ƽ��
    private void A2201()// ��ƴ ����� //�⺻ ���� �� ������ ��Ÿ���� �����մϴ�.
    {
        player.AddComponent<A2201>();
    }
    private void A2202()//ƼŸ�� ������ ��ų ���� ���ð� ����
    {
        player.AddComponent<A2202>();
    }
    private void A2203()//�����ڸ��������� �׽�Ʈ ��
    {
        player.AddComponent<A2203_1>();
    }
    private void A2204()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A2205()
    {
        Debug.Log("�̿ϼ�");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 3Ƽ��
    private void A2301()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A2302()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A2303()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A2304()
    {
        Debug.Log("�̿ϼ�");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 1Ƽ��
    private void A3101()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A3102()
    {
        player.AddComponent<A3102>();
    }
    private void A3103()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A3104()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A3105()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A3106()
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A3107() // ���̾� ����̵� �׽�Ʈ����
    {
        if (PV.IsMine)
        {
            GameObject Prefabs = Resources.Load<GameObject>("AugmentList/A3107");
            GameObject fire = Instantiate(Prefabs);
            fire.transform.SetParent(player.transform);
        }

    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 2Ƽ��
    private void A3201() //������ ���� << ������ 2������ ��ź�� +3���� ������
    {
        playerstatHandler.RollCoolTime.added += 2f;
        player.AddComponent<A3201>();
    }
    private void A3202()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A3203()//������� ��2��ü��3��
    {
        float x = (player.transform.localScale.x * 2f);//����
        float y = (player.transform.localScale.y * 2f);//����
        player.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient *= 3;
    }
    private void A3204()
    {
        player.AddComponent<A3204>();
    }
    private void A3205()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A3206()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A3207()
    {
        Debug.Log("�̿ϼ�");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 3Ƽ��
    private void A3301()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A3302()
    {
        Debug.Log("�̿ϼ�");
    }
    private void A3303()
    {
        Debug.Log("�̿ϼ�");
    }
}
