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
    public PlayerStatHandler playerstatHandler;
    private GameObject player;
    int rare;

    int atk = 5;
    int hp = 10;
    float speed = 1;
    float atkspeed = 1f;
    float bulletSpread = -1f;
    int cooltime = -1;
    int critical = 1;
    int AmmoMax = 1;

    private void Awake()
    {
        playerstatHandler = GameManager.Instance.Player.GetComponent<PlayerStatHandler>();
        player = playerstatHandler.gameObject;
    }
    private void Start()
    {
        //playerstatHandler = GameManager.Instance.Player.GetComponent<PlayerStatHandler>();
    }




    //[Range(1, 3)] int StatType;

    private void OnEnable()// 이름,정보,색 업데이트
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
                image.color = new Color(205 / 255f, 127 / 255f, 50 / 255f);//브
                break;

            case 2:
                image.color = new Color(192 / 255f, 192 / 255f, 192 / 255f);//실
                break;

            case 3:
                image.color = new Color(255 / 255f, 215 / 255f, 0 / 255f);//골
                break;
        }
    }
    public void pick()
    {
        string str = "A" + stat.Code.ToString();
        Debug.Log($"{str}");
        Invoke(str, 0);
        Ispick = true;
        ResultManager.Instance.close();
    }
    public void pick2()//내가 테스트 할려고 뒤숫자 바꾸면서 하는거 
    {
        string str = "A" + "0124";//뒷숫자컨
        Invoke(str, 0);
    }
    private void A999(PlayerStatHandler PlayerStat)
    //처음의도 어차피 스탯은 걍 다 + 하는것이니 다 섞어버려야지 >> 0+0보다 코드 나눠서 한번에 하는게 좋은거아님??
    {
        //StatAugment pick = MakeAugmentListManager.stat1[0] as StatAugment;
        //PlayerStat.HP.added = pick.Health;
        //PlayerStat.ATK.added = pick.Atk;
        //PlayerStat.AtkSpeed.added = pick.AtkSpeed;
        //여기서 적다가 위에 결론나서 안해둠 나중에 지울 함수 
    }
    private void A901()//스탯 공 티어 1
    {
        playerstatHandler.ATK.added += atk;
        Debug.Log(playerstatHandler.ATK.added);
    }
    private void A902()//스탯 체 티어 1
    {
        playerstatHandler.HP.added += hp;
        Debug.Log(playerstatHandler.HP.added);
    }
    private void A903()//스탯 이속 티어 1
    {
        playerstatHandler.Speed.added += speed;
        Debug.Log(playerstatHandler.Speed.added);

    }
    private void A904()//스탯 공속 티어 1
    {
        playerstatHandler.AtkSpeed.added += atkspeed;
        Debug.Log(playerstatHandler.AtkSpeed.added);
    }
    private void A905()//스탯 정밀도 티어 1 탄퍼짐이 이상해서 정밀도로 바꿨는데 괜찮겠지? 어차피바꿔도됨
    {
        playerstatHandler.BulletSpread.added += bulletSpread;
        Debug.Log(playerstatHandler.BulletSpread.added);
    }
    private void A906()//스탯 스킬쿨타임 티어1
    {
        playerstatHandler.SkillCoolTime.added += cooltime;
        Debug.Log(playerstatHandler.SkillCoolTime.added);
    }
    private void A907()//스탯 치명타 티어1
    {
        playerstatHandler.Critical.added += critical;
        Debug.Log(playerstatHandler.Critical.added);
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 스탯2티어
    private void A911()//스탯 공 티어 2 @@@@@@@@@@@@@@@@@이아래 수정안함 해야함
    {
        playerstatHandler.ATK.added += atk * 2;
    }
    private void A912()//스탯 체 티어 2
    {
        playerstatHandler.HP.added += hp * 2;
    }
    private void A913()//스탯 이속 티어 2
    {
        playerstatHandler.Speed.added += speed * 2;

    }
    private void A914()//스탯 공속 티어 2
    {
        playerstatHandler.AtkSpeed.added += atkspeed * 2;
    }
    private void A915()//스탯 정밀도 티어 2
    {
        playerstatHandler.BulletSpread.added += bulletSpread * 2;
    }
    private void A916()//스탯 스킬쿨타임 티어2
    {
        playerstatHandler.SkillCoolTime.added += cooltime * 2;
    }
    private void A917()//스탯 치명타 티어2
    {
        playerstatHandler.Critical.added += critical * 2;
    }
    private void A918()//스탯 치명타 티어2
    {
        playerstatHandler.AmmoMax.added += AmmoMax;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@스탯3티어
    private void A921()//스탯 공 티어 3
    {
        playerstatHandler.ATK.added += atk * 3;
    }
    private void A922()//스탯 체 티어 3
    {
        playerstatHandler.HP.added += hp * 3;
    }
    private void A923()//스탯 이속 티어 3
    {
        playerstatHandler.Speed.added += speed * 3;

    }
    private void A924()//스탯 공속 티어 3
    {
        playerstatHandler.AtkSpeed.added += atkspeed * 3;
    }
    private void A925()//스탯 정밀도 티어 3
    {
        playerstatHandler.BulletSpread.added += bulletSpread * 3;
    }
    private void A926()//스탯 스킬쿨타임 티어3
    {
        playerstatHandler.SkillCoolTime.added += cooltime * 3;
    }
    private void A927()//스탯 치명타 티어3
    {
        playerstatHandler.Critical.added += critical * 3;
    }
    private void A928()//스탯 치명타 티어3
    {
        playerstatHandler.AmmoMax.added += AmmoMax * 2;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@공용1티어
    private void A0101()
    {
        Debug.Log("미완성");
    }
    private void A0102()
    {
        Debug.Log("미완성");
    }
    private void A0103()
    {
        Debug.Log("미완성");
    }
    private void A0104()
    {
        Debug.Log("미완성");
    }
    private void A0105()
    {
        Debug.Log("미완성");
    }
    private void A0106()
    {
        Debug.Log("미완성");
    }
    private void A0107()
    {
        Debug.Log("미완성");
    }
    private void A0108()
    {
        Debug.Log("미완성");
    }
    private void A0109()// 소형화 //테스트안해봄
    {
        float x = (player.transform.localScale.x * 0.5f);//절반
        float y = (player.transform.localScale.y * 0.5f);//절반
        player.transform.localScale = new Vector2(x,y);
        //playerstatHandler.HP.coefficient -= 0.1f;
        //playerstatHandler.Speed.coefficient += 0.2f;
    }
    private void A0110()//대형화 // 테스트아직안해봄
    {
        float x = (player.transform.localScale.x * 2f);//절반
        float y = (player.transform.localScale.y * 2f);//절반
        player.transform.localScale = new Vector2(x, y);
        //playerstatHandler.HP.coefficient += 0.5f;
        //playerstatHandler.Speed.coefficient += 0.2f;
    }
    private void A0111()
    {
        Debug.Log("미완성");
    }
    private void A0112()//빠른장전
    {
        //playerstatHandler.ReloadCoolTime -= 0.1f;
    }
    private void A0113()
    {
        Debug.Log("미완성");
    }
    private void A0114()
    {
        Debug.Log("미완성");
    }
    private void A0115()
    {
        Debug.Log("미완성");
    }
    private void A0116()
    {
        Debug.Log("미완성");
    }
    private void A0117()
    {
        Debug.Log("미완성");
    }
    private void A0118()
    {
        Debug.Log("미완성");
    }
    private void A0119()
    {
        Debug.Log("미완성");
    }
    private void A0120()
    {
        Debug.Log("미완성");
    }
    private void A0121()
    {
        Debug.Log("미완성");
    }
    private void A0122()
    {
        Debug.Log("미완성");
    }
    private void A0123()
    {
        Debug.Log("미완성");
    }
    //테스트 완료 그러나 이벤트로 작동하여 추가 테스트가 필요
    private void A0124()//눈먼총잡이 : 시야가 대폭 감소 하며 공격 속도, 재장전 속도가 증가합니다.
    {
        player.AddComponent<A0124>();//A0124에서 화면어둡게 하는 프리팹 만들고 스테이지시작에 ON 끝에 OFF
        playerstatHandler.AtkSpeed.added += 15;
        playerstatHandler.ReloadCoolTime.added += 15;
    }
    private void A0125()
    {
        Debug.Log("미완성");
    }
    private void A0126()
    {
        Debug.Log("미완성");
    }
    private void A0127()
    {
        Debug.Log("미완성");
    }
    private void A0128()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 공용2티어

}
