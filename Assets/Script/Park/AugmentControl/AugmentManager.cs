using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AugmentManager : MonoBehaviourPunCallbacks //실질적으로 증강을 불러오는곳 AugmentManager.Instance.Invoke(code,0); 을통해 해당 증강불러옴
{
    public static AugmentManager Instance;
    public PlayerStatHandler playerstatHandler;
    public GameObject player;


    int atk = 5;
    int hp = 8;
    float speed = 1;
    float atkspeed = -1f;
    float bulletSpread = -1f;
    int cooltime = -1;
    int critical = 5;
    int AmmoMax = 1;
    public PlayerInput playerInput;
    public PhotonView PlayerPv;
    public int PlayerPvNumber;



    // Start is called before the first frame update
    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    public void startset(GameObject PlayerObj)
    {

        player = PlayerObj;//플레이어 받아옴 
        PlayerPvNumber = player.GetPhotonView().ViewID;//
        PlayerPv = PhotonView.Find(PlayerPvNumber);//플레이어pv 확보
                                                   //player = PlayerPv.gameObject; 생각해보니 이걸 다시바꾸는 의미가 없음
                                                   //playerstatHandler = player.GetComponent<PlayerStatHandler>();그건 애도 마찬가지
                                                   //playerInput = player.GetComponent<PlayerInput>();

    }
    public void AugmentCall(int code)
    {
        string callName = "A" + code.ToString();
        photonView.RPC(callName, RpcTarget.All, PlayerPv);
    }
    [PunRPC]
    private void A901()//스탯 공 티어 1
    {
        //photonView.RPC("A901_1", RpcTarget.All, PlayerPv);
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.ATK.added += atk;
        Debug.Log($"{photonView.gameObject.GetPhotonView().ViewID}의 공격력증가");

    }
    //[PunRPC]
    //private void A901_1(int PlayerPvNumber)//스탯 공 티어 1
    //{
    //    PhotonView photonView = PhotonView.Find(PlayerPvNumber);
    //    playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    //    playerstatHandler.ATK.added += atk;
    //}
    private void A902()//스탯 체 티어 1
    {
        //photonView.RPC("A902_1", RpcTarget.All, PlayerPv);
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.HP.added += hp;
        Debug.Log($"{photonView.gameObject.GetPhotonView().ViewID}의 방어력증가");
    }
    //private void A902_1(int PlayerPvNumber)//스탯 체 티어 1
    //{
    //    PhotonView photonView = PhotonView.Find(PlayerPvNumber);
    //    playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    //    playerstatHandler.HP.added += hp;
    //}
    [PunRPC]
    private void A903()//스탯 이속 티어 1
    {
        //photonView.RPC("A903_1", RpcTarget.All, PlayerPv);
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.Speed.added += speed;
        Debug.Log($"{photonView.gameObject.GetPhotonView().ViewID}의 이속증가");
    }
    //[PunRPC]
    //private void A903_1()//스탯 이속 티어 1
    //{
    //    PhotonView photonView = PhotonView.Find(PlayerPvNumber);
    //    playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    //    playerstatHandler.Speed.added += speed;
    //}
    [PunRPC]
    private void A904()//스탯 공속 티어 1
    {
        //photonView.RPC("A904_1", RpcTarget.All, PlayerPv);
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.AtkSpeed.added += atkspeed;
        Debug.Log($"{photonView.gameObject.GetPhotonView().ViewID}의 공속증가");
    }
    //[PunRPC]
    //private void A904_1()//스탯 공속 티어 1
    //{
    //    PhotonView photonView = PhotonView.Find(PlayerPvNumber);
    //    playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    //    playerstatHandler.AtkSpeed.added += atkspeed;
    //}
    [PunRPC]
    private void A905()//스탯 정밀도 티어 1 탄퍼짐이 이상해서 정밀도로 바꿨는데 괜찮겠지? 어차피바꿔도됨
    {
        //photonView.RPC("A905_1", RpcTarget.All, PlayerPv);
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.BulletSpread.added += bulletSpread;
        Debug.Log($"{photonView.gameObject.GetPhotonView().ViewID}의 정밀도증가");
    }
    //[PunRPC]
    //private void A905_1()//스탯 정밀도 티어 1
    //{
    //    PhotonView photonView = PhotonView.Find(PlayerPvNumber);
    //    playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    //    playerstatHandler.BulletSpread.added += bulletSpread;
    //}
    [PunRPC]
    private void A906()//스탯 스킬쿨타임 티어1
    {
        //photonView.RPC("A906_1", RpcTarget.All, PlayerPv);
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.SkillCoolTime.added += bulletSpread;
        Debug.Log($"{photonView.gameObject.GetPhotonView().ViewID}의 쿨타임증가");
    }
    //[PunRPC]
    //private void A906_1()//스탯 스킬쿨타임 티어 1
    //{
    //    PhotonView photonView = PhotonView.Find(PlayerPvNumber);
    //    playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    //    playerstatHandler.SkillCoolTime.added += bulletSpread;
    //}
    [PunRPC]
    private void A907()//스탯 치명타 티어1
    {
        //photonView.RPC("A906_1", RpcTarget.All, PlayerPv);
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.Critical.added += critical;
    }
    [PunRPC]
    //private void A907_1()//스탯 치명타 티어 1
    //{
    //    PhotonView photonView = PhotonView.Find(PlayerPvNumber);
    //    playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    //    playerstatHandler.Critical.added += critical;
    //}
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 스탯2티어
    private void A911()//스탯 공 티어 1
    {
        photonView.RPC("A911_1", RpcTarget.All, PlayerPv);
    }
    [PunRPC]
    private void A911_1(int PlayerPvNumber)//스탯 공 티어 1
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.ATK.added += atk;
    }
    private void A912()//스탯 체 티어 1
    {
        photonView.RPC("A912_1", RpcTarget.All, PlayerPv);
    }
    [PunRPC]
    private void A912_1(int PlayerPvNumber)//스탯 체 티어 1
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.HP.added += hp;
    }
    private void A913()//스탯 이속 티어 1
    {
        photonView.RPC("A903_1", RpcTarget.All, PlayerPv);
    }
    [PunRPC]
    private void A913_1()//스탯 이속 티어 1
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.Speed.added += speed;
    }
    private void A914()//스탯 공속 티어 1
    {
        photonView.RPC("A904_1", RpcTarget.All, PlayerPv);
    }
    [PunRPC]
    private void A914_1()//스탯 공속 티어 1
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.AtkSpeed.added += atkspeed;
    }
    private void A915()//스탯 정밀도 티어 1 탄퍼짐이 이상해서 정밀도로 바꿨는데 괜찮겠지? 어차피바꿔도됨
    {
        photonView.RPC("A905_1", RpcTarget.All, PlayerPv);
    }
    [PunRPC]
    private void A915_1()//스탯 정밀도 티어 1
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.BulletSpread.added += bulletSpread;
    }
    private void A916()//스탯 스킬쿨타임 티어2
    {
        photonView.RPC("A916_1", RpcTarget.All, PlayerPv);
    }
    [PunRPC]
    private void A916_1()//스탯 스킬쿨타임 티어 2
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.SkillCoolTime.added += bulletSpread;
    }
    private void A917()//스탯 치명타 티어2
    {
        photonView.RPC("A917_1", RpcTarget.All, PlayerPv);
    }
    [PunRPC]
    private void A917_1()//스탯 치명타 티어 2
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.Critical.added += critical * 2;
    }
    private void A918()//스탯 장탄수 티어2
    {
        photonView.RPC("A918_1", RpcTarget.All, PlayerPv);
    }
    private void A918_1()//스탯 장탄수 티어 2
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
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
    private void A101()//아이언스킨
    {
        playerstatHandler.defense *= 0.9f;
    }
    private void A102()//사거리 계수 -0.3 공 계수 +0.3
    {
        playerstatHandler.BulletLifeTime.coefficient *= 0.7f;
        playerstatHandler.ATK.coefficient *= 1.3f;
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
        playerstatHandler.HP.coefficient *= 0.8f;
        playerstatHandler.Speed.coefficient *= 1.2f;
    }
    private void A110()//대형화 // 테스트안해봄
    {
        float x = (player.transform.localScale.x * 1.25f);
        float y = (player.transform.localScale.y * 1.25f);
        player.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient *= 1.5f;
        playerstatHandler.Speed.coefficient *= 0.8f;
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
    private void A118()        //고장내기 mk3 1,2,3 공용 증강 이기에 좀 남다른 코드임  현재 10 /30 /60 총합 100확률을 가지고 있습죠
    {
        if (player.GetComponent<BreakDownMk>()) //만약 BreakDownMk를 가지고 있다면
        {
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(10);
        }
        else
        {
            player.AddComponent<BreakDownMk>();
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(10);
        }
    }
    private void A119()// 반전 공격방향 , 이동방향이 반대가되고 공체 대폭 증가 == 현재 이동방향 반대만 구현
    {
        if ("Player" == playerInput.currentActionMap.name)
        {
            playerInput.SwitchCurrentActionMap("Player1");
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
        playerstatHandler.HP.coefficient *= 1.5f;
        playerstatHandler.ATK.coefficient *= 1.5f;
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
        player.AddComponent<A0125>();
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
    private void A207()//하이리스크 로우리턴
    {
        playerstatHandler.defense = playerstatHandler.defense * 0.5f;
        playerstatHandler.ATK.coefficient *= 2f;
    }
    private void A208()
    {
        Debug.Log("미완성");
    }
    private void A209()//재정비 구르기시 재장전 수행
    {
        player.AddComponent<A0209>();
    }
    private void A210()
    {
        Debug.Log("미완성");
    }
    private void A211()//피해복구 일정확률로 일정 체력 회복
    {
        player.AddComponent<A0211>();
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
    private void A219() //고장내기mk2 1,2,3 공용 증강 이기에 좀 남다른 코드임 30
    {
        //고장내기 mk3 1,2,3 공용 증강 이기에 좀 남다른 코드임 
        if (player.GetComponent<BreakDownMk>()) //만약 BreakDownMk를 가지고 있다면
        {
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(30);
        }
        else
        {
            player.AddComponent<BreakDownMk>();
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(30);
        }
    }
    private void A220()
    {
        Debug.Log("미완성");
    }
    private void A221()
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A222()//재정비 구르기후 회복
    {
        //if()
        player.AddComponent<A0222>();
    }
    private void A223()
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@공용 3티어
    private void A301()//고장내기 mk3 1,2,3 공용 증강 이기에 좀 남다른 코드임 
    {
        if (player.GetComponent<BreakDownMk>()) //만약 BreakDownMk를 가지고 있다면
        {
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(60);
        }
        else
        {
            player.AddComponent<BreakDownMk>();
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(60);
        }
    }
    private void A302()//인피니티불렛 탄창 9999 획득시점의 총알 값 계산하여 9999로 맞춰줌 많든 적든 같음
    {
        playerstatHandler.AmmoMax.added += 9999 - playerstatHandler.AmmoMax.total;
    }
    private void A303()
    {
        Debug.Log("미완성");
    }
    private void A304()
    {
        Debug.Log("미완성");
    }
    private void A305()//멀티샷 샷2배
    {
        playerstatHandler.LaunchVolume.coefficient *= 2;
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
    private void A1107_1(int viewID)//영역전개
    {
        GameObject Prefabs = Resources.Load<GameObject>("AugmentList/A1107");
        GameObject go = Instantiate(Prefabs);
        PhotonView photonView = PhotonView.Find(viewID);
        go.transform.SetParent(photonView.transform);
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<A1107>().Init();

    }
    private void A1107()
    {
        photonView.RPC("A1107_1", RpcTarget.All, PlayerPv);
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
    [PunRPC]
    private void A2101_1(int viewID)
    {
        //player.AddComponent<A2101>();
        PhotonView photonView = PhotonView.Find(viewID);
        photonView.gameObject.AddComponent<A2101>();
    }
    private void A2101() //노련함 = 스킬사용후 공속 증가 테스트 ㄴ //테스트 결과 addcomponent는 다른사람에게 보여줄필요없음 
    {                                                              //함수에서 해줘야할듯

        int viewID1 = player.GetPhotonView().ViewID;
        photonView.RPC("A2101_1", RpcTarget.All, viewID1);
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
    private void A2105()// 반전 공격방향 , 이동방향이 반대가되고 공체 대폭 증가 == 현재 이동방향 반대만 구현
    {
        if ("Player" == playerInput.currentActionMap.name)
        {
            playerInput.SwitchCurrentActionMap("Player1");
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
        playerstatHandler.HP.coefficient *= 1.5f;
        playerstatHandler.ATK.coefficient *= 1.5f;
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
        //if (PV.IsMine)
        //{
        //    GameObject Prefabs = Resources.Load<GameObject>("AugmentList/A3107");
        //    GameObject fire = Instantiate(Prefabs);
        //    fire.transform.SetParent(player.transform);
        //}

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
