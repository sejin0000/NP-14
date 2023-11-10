using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.CanvasScaler;

public class AugmentManager : MonoBehaviourPunCallbacks //실질적으로 증강을 불러오는곳 AugmentManager.Instance.Invoke(code,0); 을통해 해당 증강불러옴
{
    public static AugmentManager Instance;//싱긍톤
    public PlayerStatHandler playerstatHandler;//정확히는 이름을 타겟 플레이어 스탯 핸들러가 맞는 표현 같기도함 // 생각할수록 맞음
    int atk = 5;//여기서부터 아래까지  티어별로 *n으로 사용중
    int hp = 8;
    float speed = 1;
    float atkspeed = -1f;
    float bulletSpread = -1f;
    int cooltime = -1;
    int critical = 5;
    int AmmoMax = 1;
    public PlayerInput playerInput;//이것도 사실 타켓플레이어 인풋 잘안쓰기에 함수가 따로 만들지 않음
    public GameObject targetPlayer;//실제 적용되는 타켓 플레이어 99% 경우 이걸 사용함 진짜 진짜 중요함
    public PhotonView PlayerPv;//현재플레이어의 포톤뷰값== 증강매니저의 포톤뷰가 아님 (중요)
    public GameObject player;//처음 세팅값에 필요함
    public int PlayerPvNumber;//현재플레이어의 포톤뷰 넘버
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
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        targetPlayer = photonView.gameObject;
        playerstatHandler = targetPlayer.GetComponent<PlayerStatHandler>();
    }//플레이어스탯핸들러, 타겟플레이어 모두 변하는경우
    private void ChangePlayerStatHandler(int PlayerNumber)// 플레이어스탯핸들러만변하는경우
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    }
    private void ChangeOnlyPlayer(int PlayerNumber) //타겟 플레이어만 변하는경우
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        targetPlayer = photonView.gameObject;
    }
    //PhotonView photonView = PhotonView.Find(PlayerNumber);
    //playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    #region stat
    [PunRPC]
    private void A901(int PlayerNumber)//스탯 공 티어 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.ATK.added += atk;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 공격력증가");
    }
    [PunRPC]
    private void A902(int PlayerNumber)//스탯 체 티어 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.HP.added += hp;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 방어력증가");
    }
    [PunRPC]
    private void A903(int PlayerNumber)//스탯 이속 티어 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Speed.added += speed;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 이속증가");
    }
    [PunRPC]
    private void A904(int PlayerNumber)//스탯 공속 티어 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AtkSpeed.added += atkspeed;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 공속증가");
    }
    [PunRPC]
    private void A905(int PlayerNumber)//스탯 정밀도 티어 1 탄퍼짐이 이상해서 정밀도로 바꿨는데 괜찮겠지? 어차피바꿔도됨
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.BulletSpread.added += bulletSpread;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 정밀도증가");
    }
    [PunRPC]
    private void A906(int PlayerNumber)//스탯 스킬쿨타임 티어1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.SkillCoolTime.added += cooltime;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 쿨타임증가");
    }
    [PunRPC]
    private void A907(int PlayerNumber)//스탯 치명타 티어1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Critical.added += critical;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 치명타증가");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@스탯티어2
    [PunRPC]
    private void A911(int PlayerNumber)//스탯 공 티어 2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.ATK.added += atk*2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 공격력증가");
    }
    [PunRPC]
    private void A912(int PlayerNumber)//스탯 체 티어 2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.HP.added += hp * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 방어력증가");
    }
    [PunRPC]
    private void A913(int PlayerNumber)//스탯 이속 티어 2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Speed.added += speed*2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 이속증가");
    }
    [PunRPC]
    private void A914(int PlayerNumber)//스탯 공속 티어 2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AtkSpeed.added += atkspeed * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 공속증가");
    }
    [PunRPC]
    private void A915(int PlayerNumber)//스탯 정밀도 티어 1 탄퍼짐이 이상해서 정밀도로 바꿨는데 괜찮겠지? 어차피바꿔도됨
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.BulletSpread.added += bulletSpread * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 정밀도증가");
    }
    [PunRPC]
    private void A916(int PlayerNumber)//스탯 스킬쿨타임 티어2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.SkillCoolTime.added += cooltime * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 쿨타임증가");
    }
    [PunRPC]
    private void A917(int PlayerNumber)//스탯 치명타 티어2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Critical.added += critical * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 치명타증가");
    }
    [PunRPC]
    private void A918(int PlayerNumber)//스탯 장탄수 티어2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AmmoMax.added += AmmoMax;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@스탯3티어
    private void A921(int PlayerNumber)//스탯 공 티어 3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.ATK.added += atk * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 공격력증가");
    }
    [PunRPC]
    private void A922(int PlayerNumber)//스탯 체 티어 3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.HP.added += hp * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 방어력증가");
    }
    [PunRPC]
    private void A923(int PlayerNumber)//스탯 이속 티어 3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Speed.added += speed * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 이속증가");
    }
    [PunRPC]
    private void A924(int PlayerNumber)//스탯 공속 티어 3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AtkSpeed.added += atkspeed * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 공속증가");
    }
    [PunRPC]
    private void A925(int PlayerNumber)//스탯 정밀도 티어 3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.BulletSpread.added += bulletSpread * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 정밀도증가");
    }
    [PunRPC]
    private void A926(int PlayerNumber)//스탯 스킬쿨타임 티어3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.SkillCoolTime.added += cooltime * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 쿨타임증가");
    }
    [PunRPC]
    private void A927(int PlayerNumber)//스탯 치명타 티어3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Critical.added += critical * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}의 치명타증가");
    }
    [PunRPC]
    private void A928(int PlayerNumber)//스탯 장탄수 티어3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AmmoMax.added += AmmoMax * 2;
    }
    #endregion
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@공용1티어
    #region ALL1
    [PunRPC]
    private void A101(int PlayerNumber)//아이언스킨
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.defense *= 0.9f;
        Debug.Log($"({playerstatHandler.gameObject.GetPhotonView().ViewID}의 현재 계수 {playerstatHandler.defense})");
    }
    [PunRPC]
    private void A102(int PlayerNumber)//인파이터 사거리 계수 -0.3 공 계수 +0.3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.BulletLifeTime.coefficient *= 0.7f;
        playerstatHandler.ATK.coefficient *= 1.3f;
        Debug.Log($"({playerstatHandler.gameObject.GetPhotonView().ViewID}의 현재 장전 계수 {playerstatHandler.BulletLifeTime.coefficient})");
        Debug.Log($"({playerstatHandler.gameObject.GetPhotonView().ViewID}의 현재 공 계수 {playerstatHandler.ATK.coefficient})");
    }
    [PunRPC]
    private void A103(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A104(int PlayerNumber)//약자멸시 현재 스테이지가 낮을수록 공격력 증가
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0104>();
    }
    [PunRPC]
    private void A105(int PlayerNumber)// 유리대포 //현재 최대 체력을 1로 만들고 그 값 의 절반 만큼 공업
    {
        ChangePlayerStatHandler(PlayerNumber);
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
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        float x = (targetPlayer.transform.localScale.x * 0.75f);//절반
        float y = (targetPlayer.transform.localScale.y * 0.75f);//절반
        targetPlayer.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient *= 0.8f;
        playerstatHandler.Speed.coefficient *= 1.2f;
    }
    [PunRPC]
    private void A110(int PlayerNumber)//대형화 // 테스트안해봄
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        float x = (targetPlayer.transform.localScale.x * 1.25f);
        float y = (targetPlayer.transform.localScale.y * 1.25f);
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
        ChangePlayerStatHandler(PlayerNumber);
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
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetComponent<BreakDownMk>()) //만약 BreakDownMk를 가지고 있다면
        {
            BreakDownMk Mk3 = targetPlayer.GetComponent<BreakDownMk>();
            Mk3.PercentUp(10);
            Debug.Log($"{Mk3.percent}");
        }
        else
        {
            targetPlayer.AddComponent<BreakDownMk>();
            BreakDownMk Mk3 = targetPlayer.GetComponent<BreakDownMk>();
            Mk3.PercentUp(10);
            Debug.Log($"{Mk3.percent}");
        }
    }
    [PunRPC]
    private void A119(int PlayerNumber)// 반전 공격방향 , 이동방향이 반대가되고 공체 대폭 증가 == 현재 이동방향 반대만 구현 A119 A2105는 동일 함수 합치는거 고려
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
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
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        targetPlayer.AddComponent<A0124>();//A0124에서 화면어둡게 하는 프리팹 만들고 스테이지시작에 ON 끝에 OFF
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
    #endregion
    #region All2
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
        ChangePlayerStatHandler(PlayerNumber);
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
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0209>();
    }
    [PunRPC]
    private void A210(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A211(int PlayerNumber)//피해복구 일정확률로 일정 체력 회복
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0211>();
    }
    [PunRPC]
    private void A212(int PlayerNumber)//강자멸시 현재 스테이지가 높을수록 공업
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0104>();
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
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        if (targetPlayer.GetComponent<BreakDownMk>()) //만약 BreakDownMk를 가지고 있다면
        {
            BreakDownMk Mk3 = targetPlayer.GetComponent<BreakDownMk>();
            Mk3.PercentUp(30);
            Debug.Log($"{Mk3.percent}");
        }
        else
        {
            targetPlayer.AddComponent<BreakDownMk>();
            BreakDownMk Mk3 = targetPlayer.GetComponent<BreakDownMk>();
            Mk3.PercentUp(30);
            Debug.Log($"{Mk3.percent}");
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
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0222>();
    }
    [PunRPC]
    private void A223(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    #endregion
    #region All3
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@공용 3티어
    [PunRPC]
    private void A301(int PlayerNumber)//고장내기 mk3 1,2,3 공용 증강 이기에 좀 남다른 코드임 
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        if (targetPlayer.GetComponent<BreakDownMk>()) //만약 BreakDownMk를 가지고 있다면
        {
            BreakDownMk Mk3 = targetPlayer.GetComponent<BreakDownMk>();
            Mk3.PercentUp(60);
            Debug.Log($"{Mk3.percent}");
        }
        else
        {
            targetPlayer.AddComponent<BreakDownMk>();
            BreakDownMk Mk3 = targetPlayer.GetComponent<BreakDownMk>();
            Mk3.PercentUp(60);
            Debug.Log($"{Mk3.percent}");
        }
    }
    [PunRPC]
    private void A302(int PlayerNumber)//인피니티불렛 탄창 9999 획득시점의 총알 값 계산하여 9999로 맞춰줌 많든 적든 같음
    {
        ChangePlayerStatHandler(PlayerNumber);
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
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.LaunchVolume.coefficient *= 2;
    }
    #endregion
    #region Sniper1
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
    [PunRPC]
    private void A1106(int PlayerNumber)
    {
        Debug.Log("미완성");
    }

    [PunRPC]
    private void A1107_1(int PlayerNumber)//오브젝트 생성형 폐기된 디자인
    {
        ChangeOnlyPlayer(PlayerNumber);
        GameObject Prefabs = Resources.Load<GameObject>("AugmentList/A1107");
        GameObject go = Instantiate(Prefabs, targetPlayer.transform);
        go.transform.SetParent(targetPlayer.transform);

    }
    private void A1107(int PlayerNumber) //영역전개 최초의 대상에게 영구적으로 올려주는 타입 아직 세세한 오류가 있을것으로 예상된
    {
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetPhotonView().IsMine)
        {
            GameObject Prefabs = PhotonNetwork.Instantiate("AugmentList/A1107", targetPlayer.transform.localPosition, Quaternion.identity);
            int num = Prefabs.GetPhotonView().ViewID;
            photonView.RPC("setParent", RpcTarget.All, num);
            Prefabs.GetComponent<A1107>().Init();
        }
    }
    [PunRPC]
    private void setParent(int num)
    {
        PhotonView a = PhotonView.Find(num);
        a.transform.SetParent(targetPlayer.transform);
        //Prefabs.transform.SetParent(targetPlayer.transform);
    }
    #endregion
    #region Sniper2
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
    #endregion
    #region Sniper3
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
    #endregion
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@솔져 1티어
    [PunRPC]
    private void A2101(int PlayerNumber) //노련함 = 스킬사용후 공속 증가 테스트 ㄴ
    {                                                             
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A2101>();
    }
    [PunRPC]
    private void A2102(int PlayerNumber) ///와다다다ㅏ다다 테스트안함 근데 스탯이라 상관없을듯함
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AtkSpeed.coefficient *= 2;
        playerstatHandler.ATK.coefficient *= 0.5f;
    }
    [PunRPC]
    private void A2103(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A2104(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A2105(int PlayerNumber)// 반전 공격방향 , 이동방향이 반대가되고 공체 대폭 증가 == 현재 이동방향 반대만 구현 A119 A2105는 동일 함수 합치는거 고려
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
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
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@솔져 2티어
    [PunRPC]
    private void A2201(int PlayerNumber)// 빈틈 만들기 //기본 공격 시 구르기 쿨타임이 감소합니다.
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A2201>();
    }
    [PunRPC]
    private void A2202(int PlayerNumber)//티타임 구른후 스킬 재사용 대기시간 감소
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A2202>();
    }
    [PunRPC]
    private void A2203(int PlayerNumber)//구른자리에힐생성 힐한다는거 자체가 어캐 될지 모르겠음 //미완성
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A2203_1>();
    }
    [PunRPC]
    private void A2204(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A2205(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@솔져 3티어
    [PunRPC]
    private void A2301(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A2302(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A2303(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A2304(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@샷건 1티어
    [PunRPC]
    private void A3101(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3102(int PlayerNumber)
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A3102>();
    }
    [PunRPC]
    private void A3103(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3104(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3105(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3106()
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3107(int PlayerNumber) // 파이어 토네이도 테스트안함
    {
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetPhotonView().IsMine)
        {
            GameObject prefab = PhotonNetwork.Instantiate("AugmentList/A3107", targetPlayer.transform.localPosition, Quaternion.identity);
            prefab.GetComponent<A3107>().Init(targetPlayer);
            int num = prefab.GetPhotonView().ViewID;
            photonView.RPC("SetParent", RpcTarget.All, num);
        }

    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@샷건 2티어
    [PunRPC]
    private void A3201(int PlayerNumber) //굴러서 장전 << 구르기 2초증가 장탄수 +3으로 재장전
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerstatHandler.RollCoolTime.added += 2f;
        targetPlayer.AddComponent<A3201>();
    }
    [PunRPC]
    private void A3202(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3203(int PlayerNumber)//사이즈업 몸2배체력3배
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        float x = (targetPlayer.transform.localScale.x * 2f);//절반
        float y = (targetPlayer.transform.localScale.y * 2f);//절반
        targetPlayer.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient *= 3;
    }
    [PunRPC]
    private void A3204(int PlayerNumber)
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A3204>();
    }
    [PunRPC]
    private void A3205(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3206(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3207(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@샷건 3티어
    [PunRPC]
    private void A3301(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3302(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
    [PunRPC]
    private void A3303(int PlayerNumber)
    {
        Debug.Log("미완성");
    }
}
