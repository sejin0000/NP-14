using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSlot : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Info;
    public IAugment stat;
    public bool Ispick = false;
    public int listIndex;
    PlayerStatHandler playerstatHandler;
    private void Start()
    {
        playerstatHandler = GameManager.Instance.Player.GetComponent<PlayerStatHandler>();
    }


    int rare;

    int atk=5;
    int hp=10;
    float speed=1;
    float atkspeed=1f;
    float bulletSpread=-1f;
    int cooltime=-1;
    int critical=1;
    int AmmoMax = 1;

    //[Range(1, 3)] int StatType;

    private void OnEnable()// �̸�,����,�� ������Ʈ
    {
        Name.text = stat.Name;
        Ispick = false;
        Info.text = stat.func;
        rare = stat.Rare;
        Image image = gameObject.GetComponent<Image>();
        //Debug.Log($"{rare}");
        switch (rare)
        {
            case 1:
                image.color = new Color(205 / 255f, 127 / 255f, 50 / 255f);//��
                break;

            case 2:
                image.color = new Color(192 / 255f, 192 / 255f, 192 / 255f);//��
                break;

            case 3:
                image.color = new Color(255 / 255f, 215 / 255f, 0 / 255f);//��
                break;
        }
    }
    public void pick()
    {
        string str = "A"+stat.Code.ToString();
        Debug.Log($"{str}");
        Invoke(str,0);
        Ispick = true;
        ResultManager.Instance.close();
    }
    private void A999(PlayerStatHandler PlayerStat)
    //ó���ǵ� ������ ������ �� �� + �ϴ°��̴� �� ����������� >> 0+0���� �ڵ� ������ �ѹ��� �ϴ°� �����žƴ�??
    {
        //StatAugment pick = MakeAugmentListManager.stat1[0] as StatAugment;
        //PlayerStat.HP.added = pick.Health;
        //PlayerStat.ATK.added = pick.Atk;
        //PlayerStat.AtkSpeed.added = pick.AtkSpeed;
        //���⼭ ���ٰ� ���� ��г��� ���ص� ���߿� ���� �Լ� 
    }
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

    private void A911()//���� �� Ƽ�� 2 @@@@@@@@@@@@@@@@@�̾Ʒ� �������� �ؾ���
    {
        playerstatHandler.ATK.added += atk*2;
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
}
