using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AugmentManager : MonoBehaviourPunCallbacks //실질적으로 증강을 불러오는곳 AugmentManager.Instance.Invoke(code,0); 을통해 해당 증강불러옴
{
    public static AugmentManager Instance;//싱긍톤
    public PlayerStatHandler playerstatHandler;//정확히는 이름을 타겟 플레이어 스탯 핸들러가 맞는 표현 같기도함 // 생각할수록 맞음
    public GameObject player;//처음 세팅값에 필요함
    int atk = 5;//여기서부터 아래까지  티어별로 *n으로 사용중
    int hp = 8;
    float speed = 1;
    float atkspeed = -1f;
    float bulletSpread = -1f;
    int cooltime = -1;
    int critical = 5;
    int AmmoMax = 1;
    public PlayerInput playerInput;//내가 넣은건가 ? 왜 있는것인지 당신은?? //내가넣은거 맞음 키보드상하좌우반대
    public PhotonView PlayerPv;
    public GameObject targetPlayer;//실제 적용되는 타켓
    public int PlayerPvNumber;
    private void Awake()//싱글톤
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
    public void startset(GameObject PlayerObj)//스타트세팅 메인게임매니저 게임 처음 시작부분에 호출되면 값셋팅 해줌
    {
        player = PlayerObj;//플레이어 받아옴 
        PlayerPvNumber = player.GetPhotonView().ViewID;//
        PlayerPv = PhotonView.Find(PlayerPvNumber);//플레이어pv 확보
    }
    public void AugmentCall(int code)//slot에서 pick으로 호출해서 punppc로 모든컴퓨터에 뿌려줌
    {
        string callName = "A" + code.ToString();
        photonView.RPC(callName, RpcTarget.All, PlayerPvNumber);
    }
    private void ChangePlayerAndPlayerStatHandler(int PlayerNumber)
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        targetPlayer = photonView.gameObject;
        playerstatHandler = targetPlayer.GetComponent<PlayerStatHandler>();
    }
    private void ChangePlayerStatHandler(int PlayerNumber)
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    }
    [PunRPC]
    private void A901(int PlayerNumber)//스탯 공 티어 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        //PhotonView photonView = PhotonView.Find(PlayerNumber);
        //playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.ATK.added += atk;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 공격력증가");
    }
    [PunRPC]
    private void A902(int PlayerNumber)//스탯 체 티어 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        //PhotonView photonView = PhotonView.Find(PlayerNumber);
        //playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.HP.added += hp;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 방어력증가");
    }
    [PunRPC]
    private void A903(int PlayerNumber)//스탯 이속 티어 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        //PhotonView photonView = PhotonView.Find(PlayerNumber);
        //playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.Speed.added += speed;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 이속증가");
    }
    [PunRPC]
    private void A904(int PlayerNumber)//스탯 공속 티어 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        //PhotonView photonView = PhotonView.Find(PlayerNumber);
        //playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        //photonView.RPC("A904_1", RpcTarget.All, PlayerPv);
        playerstatHandler.AtkSpeed.added += atkspeed;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 공속증가");
    }
    [PunRPC]
    private void A905(int PlayerNumber)//스탯 정밀도 티어 1 탄퍼짐이 이상해서 정밀도로 바꿨는데 괜찮겠지? 어차피바꿔도됨
    {
        ChangePlayerStatHandler(PlayerNumber);
        //PhotonView photonView = PhotonView.Find(PlayerNumber);
        //playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.BulletSpread.added += bulletSpread;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 정밀도증가");
    }
    [PunRPC]
    private void A906(int PlayerNumber)//스탯 스킬쿨타임 티어1
    {
        ChangePlayerStatHandler(PlayerNumber);
        //PhotonView photonView = PhotonView.Find(PlayerNumber);
        //playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.SkillCoolTime.added += bulletSpread;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 쿨타임증가");
    }
    [PunRPC]
    private void A907(int PlayerNumber)//스탯 치명타 티어1
    {
        ChangePlayerStatHandler(PlayerNumber);
        //PhotonView photonView = PhotonView.Find(PlayerNumber);
        //playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.Critical.added += critical;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 치명타증가");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@스탯티어2
    private void A911(int PlayerNumber)//스탯 공 티어 2
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.ATK.added += atk*2;
    }
    [PunRPC]
    private void A912(int PlayerNumber)//스탯 체 티어 2
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.HP.added += hp*2;
        Debug.Log($"{targetPlayer.gameObject.GetPhotonView().ViewID}의 방어력증가");
    }
    [PunRPC]
    private void A913(int PlayerNumber)//스탯 이속 티어 2
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.Speed.added += speed * 2;
        Debug.Log($"{targetPlayer.gameObject.GetPhotonView().ViewID}의 이속증가");
    }
    [PunRPC]
    private void A914(int PlayerNumber)//스탯 공속 티어 2
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.AtkSpeed.added += atkspeed * 2;
        Debug.Log($"{targetPlayer.gameObject.GetPhotonView().ViewID}의 공속증가");
    }
    [PunRPC]
    private void A915(int PlayerNumber)//스탯 정밀도 티어 2 탄퍼짐이 이상해서 정밀도로 바꿨는데 괜찮겠지? 어차피바꿔도됨
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.BulletSpread.added += bulletSpread*2;
        Debug.Log($"{targetPlayer.gameObject.GetPhotonView().ViewID}의 정밀도증가");
    }
    [PunRPC]
    private void A916(int PlayerNumber)//스탯 스킬쿨타임 티어2
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.SkillCoolTime.added += bulletSpread*2;
        Debug.Log($"{targetPlayer.gameObject.GetPhotonView().ViewID}의 쿨타임증가");
    }

    [PunRPC]
    private void A917(int PlayerNumber)//스탯 치명타 티어2
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.Critical.added += critical*2;
    }
    private void A918(int PlayerNumber)//스탯 장탄수 티어2
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.AmmoMax.added += AmmoMax;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@스탯3티어
    private void A921(int PlayerNumber)//스탯 공 티어 3
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.ATK.added += atk * 3;
    }
    [PunRPC]
    private void A922(int PlayerNumber)//스탯 체 티어 3
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.HP.added += hp * 3;
    }
    [PunRPC]
    private void A923(int PlayerNumber)//스탯 이속 티어 3
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.Speed.added += speed * 3;
        Debug.Log($"{photonView.gameObject.GetPhotonView().ViewID}의 이속증가");
    }
    [PunRPC]
    private void A924(int PlayerNumber)//스탯 공속 티어 3
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.AtkSpeed.added += atkspeed * 3;
        Debug.Log($"{photonView.gameObject.GetPhotonView().ViewID}의 공속증가");
    }
    [PunRPC]
    private void A925(int PlayerNumber)//스탯 정밀도 티어 3
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.BulletSpread.added += bulletSpread * 3;
        Debug.Log($"{photonView.gameObject.GetPhotonView().ViewID}의 정밀도증가");
    }
    [PunRPC]//단순 수치 올려주는 경우 공략
    private void A926(int PlayerNumber)//스탯 스킬쿨타임 티어3
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);// 현재 int 값으로 자기 자신의 포톤뷰 값을 넣어줍니다 모든 컴퓨터의 나에게 적용된다는 의미죠
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();//모든 사람이 현재 '나'의 스탯을 가져옵니다
        playerstatHandler.SkillCoolTime.added += bulletSpread * 3;                  // 모든 사람이 '나'의 스탯에 +를 해주는 중
    }

    [PunRPC]
    private void A927(int PlayerNumber)//스탯 치명타 티어3
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.Critical.added += critical * 3;
    }
    private void A928(int PlayerNumber)//스탯 장탄수 티어3
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.AmmoMax.added += AmmoMax*2;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@공용1티어
    [PunRPC]
    private void A101(int PlayerNumber)//아이언스킨
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.defense *= 0.9f;
    }
    [PunRPC]
    private void A102()//인파이터 사거리 계수 -0.3 공 계수 +0.3
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.BulletLifeTime.coefficient *= 0.7f;
        playerstatHandler.ATK.coefficient *= 1.3f;
    }
    [PunRPC]
    private void A103(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A104(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A105(int PlayerNumber)// 유리대포 //현재 최대 체력을 1로 만들고 그 값 의 절반 만큼 공업
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        float up = ((int)playerstatHandler.HP.total - 1);
        playerstatHandler.HP.added -= up;
        playerstatHandler.ATK.added += up * 0.5f;
    }
    [PunRPC]
    private void A106(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A107(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A108(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A109(int PlayerNumber)// 소형화 //테스트안해봄
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        targetPlayer = playerstatHandler.gameObject;//의문인게 스케일은 트랜스폼 뷰에서 바뀔텐데 의미가 있는것인지 ? ==모두 rpc화 하기로 했기에 하는게 맞다고봄
        float x = (targetPlayer.transform.localScale.x * 0.75f);//절반
        float y = (targetPlayer.transform.localScale.y * 0.75f);//절반
        targetPlayer.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient *= 0.8f;
        playerstatHandler.Speed.coefficient *= 1.2f;
    }
    [PunRPC]
    private void A110(int PlayerNumber)//대형화 // 테스트안해봄
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        targetPlayer = playerstatHandler.gameObject;
        float x = (player.transform.localScale.x * 1.25f);
        float y = (player.transform.localScale.y * 1.25f);
        targetPlayer.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient *= 1.5f;
        playerstatHandler.Speed.coefficient *= 0.8f;
    }
    [PunRPC]
    private void A111(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A112(int PlayerNumber)//빠른장전
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        playerstatHandler.ReloadCoolTime.added -= 0.3f;
    }
    [PunRPC]
    private void A113(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A114(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A115(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A116(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A117(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A118(int PlayerNumber)        //고장내기 mk3 1,2,3 공용 증강 이기에 좀 남다른 코드임  현재 10 /30 /60 총합 100확률을 가지고 있습죠
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        targetPlayer = playerstatHandler.gameObject;
        if (targetPlayer.GetComponent<BreakDownMk>()) //만약 BreakDownMk를 가지고 있다면
        {
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(10);
            Debug.Log($"{Mk3.percent}");
        }
        else
        {
            targetPlayer.AddComponent<BreakDownMk>();
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(10);
            Debug.Log($"{Mk3.percent}");
        }
    }
    [PunRPC]
    private void A119(int PlayerNumber)// 반전 공격방향 , 이동방향이 반대가되고 공체 대폭 증가 == 현재 이동방향 반대만 구현
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        targetPlayer = playerstatHandler.gameObject;
        playerInput = targetPlayer.GetComponent<PlayerInput>();
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
    [PunRPC]
    private void A120(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A121(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A122(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A123(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    //테스트 완료 그러나 이벤트로 작동하여 추가 테스트가 필요
    [PunRPC]
    private void A124(int PlayerNumber)//눈먼총잡이 : 시야가 대폭 감소 하며 공격 속도, 재장전 속도가 증가합니다.
    {
        //PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        //ChangePlayer(PlayerNumber);
        player.AddComponent<A0124>();//A0124에서 화면어둡게 하는 프리팹 만들고 스테이지시작에 ON 끝에 OFF
        playerstatHandler.AtkSpeed.added += 15;
        playerstatHandler.ReloadCoolTime.added += 15;
    }
    
    [PunRPC]
    private void A125(int PlayerNumber)
    {
        player.AddComponent<A0125>();
    }
    [PunRPC]
    private void A126(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A127(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A128(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 공용2티어
    [PunRPC]
    private void A201(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A202(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A203(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A204(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A205(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A206(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A207(int PlayerNumber)//하이리스크 로우리턴
    {
        playerstatHandler.defense = playerstatHandler.defense * 0.5f;
        playerstatHandler.ATK.coefficient *= 2f;
    }
    [PunRPC]
    private void A208(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A209(int PlayerNumber)//재정비 구르기시 재장전 수행
    {
        player.AddComponent<A0209>();
    }
    [PunRPC]
    private void A210(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A211(int PlayerNumber)//피해복구 일정확률로 일정 체력 회복
    {
        player.AddComponent<A0211>();
    }
    [PunRPC]
    private void A212(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A213(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A214(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A215(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A216(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A217(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A218(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A219(int PlayerNumber) //고장내기mk2 1,2,3 공용 증강 이기에 좀 남다른 코드임 30
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        targetPlayer = playerstatHandler.gameObject;
        if (targetPlayer.GetComponent<BreakDownMk>()) //만약 BreakDownMk를 가지고 있다면
        {
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(30);
        }
        else
        {
            targetPlayer.AddComponent<BreakDownMk>();
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(30);
        }
    }
    [PunRPC]
    private void A220(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A221(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A222(int PlayerNumber)//재정비 구르기후 회복
    {
        //if()
        player.AddComponent<A0222>();
    }
    [PunRPC]
    private void A223(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@공용 3티어
    [PunRPC]
    private void A301(int PlayerNumber)//고장내기 mk3 1,2,3 공용 증강 이기에 좀 남다른 코드임 
    {
        PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
        targetPlayer = playerstatHandler.gameObject;
        if (targetPlayer.GetComponent<BreakDownMk>()) //만약 BreakDownMk를 가지고 있다면
        {
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(60);
        }
        else
        {
            targetPlayer.AddComponent<BreakDownMk>();
            BreakDownMk Mk3 = player.GetComponent<BreakDownMk>();
            Mk3.PercentUp(60);
        }
    }
    [PunRPC]
    private void A302(int PlayerNumber)//인피니티불렛 탄창 9999 획득시점의 총알 값 계산하여 9999로 맞춰줌 많든 적든 같음
    {
        playerstatHandler.AmmoMax.added += 9999 - playerstatHandler.AmmoMax.total;
    }
    [PunRPC]
    private void A303(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A304(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A305(int PlayerNumber)//멀티샷 샷2배
    {
        playerstatHandler.LaunchVolume.coefficient *= 2;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@스나이퍼 1티어
    [PunRPC]
    private void A1101(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1102(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1103(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1104(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1105(int PlayerNumber)
    {
        Debug.Log("미완성");

    }
    private void A1106(int PlayerNumber)
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
    [PunRPC]
    private void A1107(int PlayerNumber)
    {
        photonView.RPC("A1107_1", RpcTarget.All, PlayerPv);
    }

    //@@@@@@@@@@@@@@@@@@@@@@@@@@@스나이퍼 2티어
    [PunRPC]
    private void A1201(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1202(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1203(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1204(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1205(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1206(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1207(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@스나이퍼 3티어
    [PunRPC]
    private void A1301(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1302(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1303(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A1304(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@솔져 1티어
    [PunRPC]
    private void A2101_1(int viewID)//수정필요 허나 이친구는 add컴포넌트의 중요한 몰라 나둬
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
