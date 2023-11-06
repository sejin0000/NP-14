using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : MonoBehaviourPunCallbacks //실질적으로 증강을 불러오는곳 AugmentManager.Instance.Invoke(code,0); 을통해 해당 증강불러옴
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
    //처음의도 어차피 스탯은 걍 다 + 하는것이니 다 섞어버려야지 >> 0+0보다 코드 나눠서 한번에 하는게 좋은거아님??
    //{
        //StatAugment pick = MakeAugmentListManager.stat1[0] as StatAugment;
        //PlayerStat.HP.added = pick.Health;
        //PlayerStat.ATK.added = pick.Atk;
        //PlayerStat.AtkSpeed.added = pick.AtkSpeed;
        //여기서 적다가 위에 결론나서 안해둠 나중에 지울 함수 
    //}
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
    private void A101()
    {
        Debug.Log("미완성");
    }
    private void A102()
    {
        Debug.Log("미완성");
    }
    private void A103()
    {
        Debug.Log("미완성");
    }
    private void A104()
    {
        Debug.Log("미완성");
    }
    private void A105()// 유리대포 //현재 최대 체력을 1로 만들고 그 값 의 절반 만큼 공업
    {
        float up = ((int)playerstatHandler.HP.total - 1);
        playerstatHandler.HP.added -= up;
        playerstatHandler.ATK.added += up * 0.5f;
    }
    private void A106()
    {
        Debug.Log("미완성");
    }
    private void A107()
    {
        Debug.Log("미완성");
    }
    private void A108()
    {
        Debug.Log("미완성");
    }
    private void A109()// 소형화 //테스트안해봄
    {
        float x = (player.transform.localScale.x * 0.75f);//절반
        float y = (player.transform.localScale.y * 0.75f);//절반
        player.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient -= 0.1f;
        playerstatHandler.Speed.coefficient += 0.2f;
    }
    private void A110()//대형화 // 테스트안해봄
    {
        float x = (player.transform.localScale.x * 1.25f);
        float y = (player.transform.localScale.y * 1.25f);
        player.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient += 0.5f;
        playerstatHandler.Speed.coefficient += 0.2f;
    }
    private void A111()
    {
        Debug.Log("미완성");
    }
    private void A112()//빠른장전
    {
        playerstatHandler.ReloadCoolTime.added -= 0.3f;
    }
    private void A113()
    {
        Debug.Log("미완성");
    }
    private void A114()
    {
        Debug.Log("미완성");
    }
    private void A115()
    {
        Debug.Log("미완성");
    }
    private void A116()
    {
        Debug.Log("미완성");
    }
    private void A117()
    {
        Debug.Log("미완성");
    }
    private void A118()
    {
        Debug.Log("미완성");
    }
    private void A119()
    {
        Debug.Log("미완성");
    }
    private void A120()
    {
        Debug.Log("미완성");
    }
    private void A121()
    {
        Debug.Log("미완성");
    }
    private void A122()
    {
        Debug.Log("미완성");
    }
    private void A123()
    {
        Debug.Log("미완성");
    }
    //테스트 완료 그러나 이벤트로 작동하여 추가 테스트가 필요
    private void A124()//눈먼총잡이 : 시야가 대폭 감소 하며 공격 속도, 재장전 속도가 증가합니다.
    {
        player.AddComponent<A0124>();//A0124에서 화면어둡게 하는 프리팹 만들고 스테이지시작에 ON 끝에 OFF
        playerstatHandler.AtkSpeed.added += 15;
        playerstatHandler.ReloadCoolTime.added += 15;
    }
    private void A125()
    {
        Debug.Log("미완성");
    }
    private void A126()
    {
        Debug.Log("미완성");
    }
    private void A127()
    {
        Debug.Log("미완성");
    }
    private void A128()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 공용2티어
    private void A201()
    {
        Debug.Log("미완성");
    }
    private void A202()
    {
        Debug.Log("미완성");
    }
    private void A203()
    {
        Debug.Log("미완성");
    }
    private void A204()
    {
        Debug.Log("미완성");
    }
    private void A205()
    {
        Debug.Log("미완성");
    }
    private void A206()
    {
        Debug.Log("미완성");
    }
    private void A207()
    {
        Debug.Log("미완성");
    }
    private void A208()
    {
        Debug.Log("미완성");
    }
    private void A209()
    {
        Debug.Log("미완성");
    }
    private void A210()
    {
        Debug.Log("미완성");
    }
    private void A211()
    {
        Debug.Log("미완성");
    }
    private void A212()
    {
        Debug.Log("미완성");
    }
    private void A213()
    {
        Debug.Log("미완성");
    }
    private void A214()
    {
        Debug.Log("미완성");
    }
    private void A215()
    {
        Debug.Log("미완성");
    }
    private void A216()
    {
        Debug.Log("미완성");
    }
    private void A217()
    {
        Debug.Log("미완성");
    }
    private void A218()
    {
        Debug.Log("미완성");
    }
    private void A219()
    {
        Debug.Log("미완성");
    }
    private void A220()
    {
        Debug.Log("미완성");
    }
    private void A221()
    {
        Debug.Log("미완성");
    }
    private void A222()
    {
        Debug.Log("미완성");
    }
    private void A223()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@공용 3티어
    private void A301()
    {
        Debug.Log("미완성");
    }
    private void A302()
    {
        Debug.Log("미완성");
    }
    private void A303()
    {
        Debug.Log("미완성");
    }
    private void A304()
    {
        Debug.Log("미완성");
    }
    private void A305()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@스나이퍼 1티어
    private void A1101()
    {
        Debug.Log("미완성");
    }
    private void A1102()
    {
        Debug.Log("미완성");
    }
    private void A1103()
    {
        Debug.Log("미완성");
    }
    private void A1104()
    {
        Debug.Log("미완성");
    }
    private void A1105() 
    {
        Debug.Log("미완성");

    }
    private void A1106()
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1107()
    {
        if (PV.IsMine)
        {
            GameObject Prefabs = Resources.Load<GameObject>("AugmentList/A1107");
            //없으면 로드하는걸로 리소스 로드 = 불러오기가 엄청무거움 =시작때 처음할것, 혹은 최소한만 플레이하게 장치를 해둘것
            //프리팹은 한번만 로드한다 스타트 혹은 어웨이크에서 로드해서 관리할것
            GameObject fire = Instantiate(Prefabs, player.transform);
            //fire.transform.SetParent(player.transform);
            A1107 a1107 = fire.GetComponent<A1107>();
            a1107.Init(playerstatHandler);
            // 난 지금까지 가서 값을 받아왔는데 생성할때 값을 줘서 관리를 할것 -
        }
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@스나이퍼 2티어
    private void A1201()
    {
        Debug.Log("미완성");
    }
    private void A1202()
    {
        Debug.Log("미완성");
    }
    private void A1203()
    {
        Debug.Log("미완성");
    }
    private void A1204()
    {
        Debug.Log("미완성");
    }
    private void A1205()
    {
        Debug.Log("미완성");
    }
    private void A1206()
    {
        Debug.Log("미완성");
    }
    private void A1207()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@스나이퍼 3티어
    private void A1301()
    {
        Debug.Log("미완성");
    }
    private void A1302()
    {
        Debug.Log("미완성");
    }
    private void A1303()
    {
        Debug.Log("미완성");
    }
    private void A1304()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@솔져 1티어
    private void A2101()//노련함 = 스킬사용후 공속 증가 테스트 ㄴ
    {
        player.AddComponent<A2101>();
    }
    private void A2102() ///와다다다ㅏ다다 테스트안함 근데 스탯이라 상관없을듯함
    {
        playerstatHandler.AtkSpeed.coefficient *= 2;
        playerstatHandler.ATK.coefficient *= 0.5f;
    }
    private void A2103()
    {
        Debug.Log("미완성");
    }
    private void A2104()
    {
        Debug.Log("미완성");
    }
    private void A2105()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@솔져 2티어
    private void A2201()// 빈틈 만들기 //기본 공격 시 구르기 쿨타임이 감소합니다.
    {
        player.AddComponent<A2201>();
    }
    private void A2202()//티타임 구른후 스킬 재사용 대기시간 감소
    {
        player.AddComponent<A2202>();
    }
    private void A2203()//구른자리에힐생성 테스트 ㄴ
    {
        player.AddComponent<A2203_1>();
    }
    private void A2204()
    {
        Debug.Log("미완성");
    }
    private void A2205()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@솔져 3티어
    private void A2301()
    {
        Debug.Log("미완성");
    }
    private void A2302()
    {
        Debug.Log("미완성");
    }
    private void A2303()
    {
        Debug.Log("미완성");
    }
    private void A2304()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@샷건 1티어
    private void A3101()
    {
        Debug.Log("미완성");
    }
    private void A3102()
    {
        player.AddComponent<A3102>();
    }
    private void A3103()
    {
        Debug.Log("미완성");
    }
    private void A3104()
    {
        Debug.Log("미완성");
    }
    private void A3105()
    {
        Debug.Log("미완성");
    }
    private void A3106()
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3107() // 파이어 토네이도 테스트안함
    {
        if (PV.IsMine)
        {
            GameObject Prefabs = Resources.Load<GameObject>("AugmentList/A3107");
            GameObject fire = Instantiate(Prefabs);
            fire.transform.SetParent(player.transform);
        }

    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@샷건 2티어
    private void A3201() //굴러서 장전 << 구르기 2초증가 장탄수 +3으로 재장전
    {
        playerstatHandler.RollCoolTime.added += 2f;
        player.AddComponent<A3201>();
    }
    private void A3202()
    {
        Debug.Log("미완성");
    }
    private void A3203()//사이즈업 몸2배체력3배
    {
        float x = (player.transform.localScale.x * 2f);//절반
        float y = (player.transform.localScale.y * 2f);//절반
        player.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient *= 3;
    }
    private void A3204()
    {
        player.AddComponent<A3204>();
    }
    private void A3205()
    {
        Debug.Log("미완성");
    }
    private void A3206()
    {
        Debug.Log("미완성");
    }
    private void A3207()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@샷건 3티어
    private void A3301()
    {
        Debug.Log("미완성");
    }
    private void A3302()
    {
        Debug.Log("미완성");
    }
    private void A3303()
    {
        Debug.Log("미완성");
    }
}
