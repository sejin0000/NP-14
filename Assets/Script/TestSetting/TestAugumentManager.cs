using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.CanvasScaler;

public class TestAugmentManager : MonoBehaviourPunCallbacks //���������� ������ �ҷ����°� AugmentManager.Instance.Invoke(code,0); ������ �ش� �����ҷ���
{
    public static TestAugmentManager Instance;
    public PlayerStatHandler playerstatHandler;
    int atk = 5;
    int hp = 8;
    float speed = 1;
    float atkspeed = 1f;
    float bulletSpread = -1f;
    int cooltime = -1;
    int critical = 5;
    int AmmoMax = 1;
    public PlayerInput playerInput;//�̰͵� ��� Ÿ���÷��̾� ��ǲ �߾Ⱦ��⿡ �Լ��� ���� ������ ����
    public GameObject targetPlayer;//���� ����Ǵ� Ÿ�� �÷��̾� 99% ��� �̰� ����� ��¥ ��¥ �߿���
    public PhotonView PlayerPv;//�����÷��̾��� ����䰪== �����Ŵ����� ����䰡 �ƴ� (�߿�)
    public GameObject player;//ó�� ���ð��� �ʿ���
    public int PlayerPvNumber;//�����÷��̾��� ����� �ѹ�
    private void Awake()//�̱���
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
    public void startset(GameObject PlayerObj)//��ŸƮ���� ���ΰ��ӸŴ��� ���� ó�� ���ۺκп� ȣ��Ǹ� ������ ����
    {
        player = PlayerObj;//�÷��̾� �޾ƿ� 
        PlayerPvNumber = player.GetPhotonView().ViewID;//
        PlayerPv = PhotonView.Find(PlayerPvNumber);//�÷��̾�pv Ȯ��
    }
    public void AugmentCall(int code)//slot���� pick���� ȣ���ؼ� punppc�� �����ǻ�Ϳ� �ѷ���
    {
        string callName = "A" + code.ToString();
        photonView.RPC(callName, RpcTarget.All, PlayerPvNumber);
    }
    private void ChangePlayerAndPlayerStatHandler(int PlayerNumber)
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        targetPlayer = photonView.gameObject;
        playerstatHandler = targetPlayer.GetComponent<PlayerStatHandler>();
    }//�÷��̾���ڵ鷯, Ÿ���÷��̾� ��� ���ϴ°��
    private void ChangePlayerStatHandler(int PlayerNumber)// �÷��̾���ڵ鷯�����ϴ°��
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    }
    private void ChangeOnlyPlayer(int PlayerNumber) //Ÿ�� �÷��̾ ���ϴ°��
    {
        PhotonView photonView = PhotonView.Find(PlayerNumber);
        targetPlayer = photonView.gameObject;
    }
    //PhotonView photonView = PhotonView.Find(PlayerNumber);
    //playerstatHandler = photonView.gameObject.GetComponent<PlayerStatHandler>();
    [PunRPC]
    private void FindMaster(int num)
    {
        PhotonView a = PhotonView.Find(num);
        a.transform.SetParent(targetPlayer.transform);
        a.transform.localPosition = Vector3.zero;
        //Prefabs.transform.SetParent(targetPlayer.transform);
    }

    #region stat
    [PunRPC]
    private void A901(int PlayerNumber)//���� �� Ƽ�� 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.ATK.added += atk;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ���ݷ�����");
    }
    [PunRPC]
    private void A902(int PlayerNumber)//���� ü Ƽ�� 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.HP.added += hp;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ��������");
    }
    [PunRPC]
    private void A903(int PlayerNumber)//���� �̼� Ƽ�� 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Speed.added += speed;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� �̼�����");
    }
    [PunRPC]
    private void A904(int PlayerNumber)//���� ���� Ƽ�� 1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AtkSpeed.added += atkspeed;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ��������");
    }
    [PunRPC]
    private void A905(int PlayerNumber)//���� ���е� Ƽ�� 1 ź������ �̻��ؼ� ���е��� �ٲ�µ� ��������? �����ǹٲ㵵��
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.BulletSpread.added += bulletSpread;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ���е�����");
    }
    [PunRPC]
    private void A906(int PlayerNumber)//���� ��ų��Ÿ�� Ƽ��1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.SkillCoolTime.added += cooltime;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ��Ÿ������");
    }
    [PunRPC]
    private void A907(int PlayerNumber)//���� ġ��Ÿ Ƽ��1
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Critical.added += critical;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ġ��Ÿ����");
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@����Ƽ��2
    [PunRPC]
    private void A911(int PlayerNumber)//���� �� Ƽ�� 2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.ATK.added += atk * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ���ݷ�����");
    }
    [PunRPC]
    private void A912(int PlayerNumber)//���� ü Ƽ�� 2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.HP.added += hp * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ��������");
    }
    [PunRPC]
    private void A913(int PlayerNumber)//���� �̼� Ƽ�� 2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Speed.added += speed * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� �̼�����");
    }
    [PunRPC]
    private void A914(int PlayerNumber)//���� ���� Ƽ�� 2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AtkSpeed.added += atkspeed * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ��������");
    }
    [PunRPC]
    private void A915(int PlayerNumber)//���� ���е� Ƽ�� 1 ź������ �̻��ؼ� ���е��� �ٲ�µ� ��������? �����ǹٲ㵵��
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.BulletSpread.added += bulletSpread * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ���е�����");
    }
    [PunRPC]
    private void A916(int PlayerNumber)//���� ��ų��Ÿ�� Ƽ��2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.SkillCoolTime.added += cooltime * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ��Ÿ������");
    }
    [PunRPC]
    private void A917(int PlayerNumber)//���� ġ��Ÿ Ƽ��2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Critical.added += critical * 2;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ġ��Ÿ����");
    }
    [PunRPC]
    private void A918(int PlayerNumber)//���� ��ź�� Ƽ��2
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AmmoMax.added += AmmoMax;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@����3Ƽ��
    private void A921(int PlayerNumber)//���� �� Ƽ�� 3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.ATK.added += atk * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ���ݷ�����");
    }
    [PunRPC]
    private void A922(int PlayerNumber)//���� ü Ƽ�� 3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.HP.added += hp * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ��������");
    }
    [PunRPC]
    private void A923(int PlayerNumber)//���� �̼� Ƽ�� 3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Speed.added += speed * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� �̼�����");
    }
    [PunRPC]
    private void A924(int PlayerNumber)//���� ���� Ƽ�� 3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AtkSpeed.added += atkspeed * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ��������");
    }
    [PunRPC]
    private void A925(int PlayerNumber)//���� ���е� Ƽ�� 3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.BulletSpread.added += bulletSpread * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ���е�����");
    }
    [PunRPC]
    private void A926(int PlayerNumber)//���� ��ų��Ÿ�� Ƽ��3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.SkillCoolTime.added += cooltime * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ��Ÿ������");
    }
    [PunRPC]
    private void A927(int PlayerNumber)//���� ġ��Ÿ Ƽ��3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.Critical.added += critical * 3;
        Debug.Log($"{playerstatHandler.gameObject.GetPhotonView().ViewID}�� ġ��Ÿ����");
    }
    [PunRPC]
    private void A928(int PlayerNumber)//���� ��ź�� Ƽ��3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AmmoMax.added += AmmoMax * 2;
    }
    #endregion
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@����1Ƽ��
    #region ALL1
    [PunRPC]
    private void A101(int PlayerNumber)//���̾�Ų
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.defense *= 0.9f;
        Debug.Log($"({playerstatHandler.gameObject.GetPhotonView().ViewID}�� ���� ��� {playerstatHandler.defense})");
    }
    [PunRPC]
    private void A102(int PlayerNumber)//�������� ��Ÿ� ��� -0.3 �� ��� +0.3
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.BulletLifeTime.coefficient *= 0.7f;
        playerstatHandler.ATK.coefficient *= 1.3f;
        Debug.Log($"({playerstatHandler.gameObject.GetPhotonView().ViewID}�� ���� ���� ��� {playerstatHandler.BulletLifeTime.coefficient})");
        Debug.Log($"({playerstatHandler.gameObject.GetPhotonView().ViewID}�� ���� �� ��� {playerstatHandler.ATK.coefficient})");
    }
    [PunRPC]
    private void A103(int PlayerNumber)//����//ź������ �������� �����ð� ���� //�������� ���۽� ź����*0.2f ��
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0103>();
    }
    [PunRPC]
    private void A104(int PlayerNumber)//���ڸ�� ���� ���������� �������� ���ݷ� ����
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0104>();
    }
    [PunRPC]
    private void A105(int PlayerNumber)// �������� //���� �ִ� ü���� 1�� ����� �� �� �� ���� ��ŭ ����
    {
        ChangePlayerStatHandler(PlayerNumber);
        float up = ((int)playerstatHandler.HP.total - 1);
        playerstatHandler.HP.added -= up;
        playerstatHandler.CurHP = 1;
        playerstatHandler.ATK.added += up * 0.5f;
    }
    [PunRPC]
    private void A106(int PlayerNumber)//óġ�� ������ ����
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0106>();
    }
    [PunRPC]
    private void A107(int PlayerNumber)//�˸��� Ÿ�̹� //������ �ִ� �ð��� ����Ͽ� ����
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0107>();
    }
    [PunRPC]
    private void A108(int PlayerNumber)//Ÿ�ݽ� �Ͻ��� �̼� ���� A0108�� ..Ÿ�ݽ� ���� �˰� ��ũ��Ʈ������Ͼƴ϶� �վȴ�
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        targetPlayer.AddComponent<A0108>();
    }
    [PunRPC]
    private void A109(int PlayerNumber)// ����ȭ //�׽�Ʈ���غ�
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        float x = (targetPlayer.transform.localScale.x * 0.75f);//����
        float y = (targetPlayer.transform.localScale.y * 0.75f);//����
        targetPlayer.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient *= 0.8f;
        playerstatHandler.Speed.coefficient *= 1.2f;
    }
    [PunRPC]
    private void A110(int PlayerNumber)//����ȭ // �׽�Ʈ���غ�
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        float x = (targetPlayer.transform.localScale.x * 1.25f);
        float y = (targetPlayer.transform.localScale.y * 1.25f);
        targetPlayer.transform.localScale = new Vector2(x, y);
        playerstatHandler.HP.coefficient *= 1.5f;
        playerstatHandler.Speed.coefficient *= 0.8f;
    }
    [PunRPC]
    private void A111(int PlayerNumber)//r
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0111>();
    }
    [PunRPC]
    private void A112(int PlayerNumber)//��������
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.ReloadCoolTime.coefficient *= 0.7f;
    }
    [PunRPC]
    private void A113(int PlayerNumber)// �Ӵ�=�Ŀ�
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0113>();
    }
    [PunRPC]
    private void A114(int PlayerNumber)//��
    {
        ChangeOnlyPlayer(PlayerNumber);
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        a.fire = true;
    }
    [PunRPC]
    private void A115(int PlayerNumber)//��
    {
        ChangeOnlyPlayer(PlayerNumber);
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        a.water = true;
    }
    [PunRPC]
    private void A116(int PlayerNumber)//����� ��ũ�� ��� �Ѿ�ũ���
    {
        ChangeOnlyPlayer(PlayerNumber);
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        a.sizeBody = true;
    }
    [PunRPC]
    private void A117(int PlayerNumber)//777 ���� Ȯ�� ���� ���� ���� ���� Ȯ�� ����� �������� ���Եɰ��ɼ��� ����
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        PlayerInputController inputControl = targetPlayer.GetComponent<PlayerInputController>();
        inputControl.atkPercent -= 30;
        playerstatHandler.ATK.coefficient *= 1.3f;
    }
    [PunRPC]
    private void A118(int PlayerNumber)        //���峻�� mk3 1,2,3 ���� ���� �̱⿡ �� ���ٸ� �ڵ���  ���� 10 /30 /60 ���� 100Ȯ���� ������ �ֽ���
    {
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetComponent<BreakDownMk>()) //���� BreakDownMk�� ������ �ִٸ�
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
    private void A119(int PlayerNumber)// ���� ���ݹ��� , �̵������� �ݴ밡�ǰ� ��ü ���� ���� == ���� �̵����� �ݴ븸 ���� A119 A2105�� ���� �Լ� ��ġ�°� ���
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerInput = targetPlayer.GetComponent<PlayerInput>();
        if (playerstatHandler.isNoramlMove)
        {
            playerInput.actions.FindAction("Move2").Enable();
            playerInput.actions.FindAction("Move").Disable();
            playerstatHandler.isNoramlMove = false;
        }
        else
        {
            playerInput.actions.FindAction("Move2").Disable();
            playerInput.actions.FindAction("Move").Enable();
            playerstatHandler.isNoramlMove = true;
        }
        playerstatHandler.HP.coefficient *= 1.5f;
        playerstatHandler.ATK.coefficient *= 1.5f;
    }
    //private void A119(int PlayerNumber)// ���� ��� ���� ����
    //{
    //    ChangePlayerAndPlayerStatHandler(PlayerNumber);
    //    if (targetPlayer.GetComponent<PlayerInput>() == null)
    //    {
    //        Debug.Log("�ΰ��� �������������");
    //    }
    //    playerInput = targetPlayer.GetComponent<PlayerInput>();
    //    if (playerInput.currentActionMap.name == "Player")
    //    {
    //        playerInput.SwitchCurrentActionMap("Player1");
    //    }
    //    else
    //    {
    //        playerInput.SwitchCurrentActionMap("Player");
    //    }
    //    playerstatHandler.HP.coefficient *= 1.5f;
    //    playerstatHandler.ATK.coefficient *= 1.5f;
    //}
    [PunRPC]
    private void A120(int PlayerNumber)//���� ��ũ ���� 122�� ������
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0120>();
    }
    [PunRPC]
    private void A121(int PlayerNumber)
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.MaxRollStack += 1;
        playerstatHandler.CurRollStack += 1;
    }
    [PunRPC]
    private void A122(int PlayerNumber)//ȭ�ٴ� 120�� �ҹ���//122����
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0122>();
    }
    [PunRPC]
    private void A123(int PlayerNumber)//ū��ūå�� //�Ѿ� �ǾƱ��� x
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerstatHandler.ATK.coefficient *= 1.3f;
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        targetPlayer.GetPhotonView().Owner.CustomProperties.TryGetValue("Char_Class", out object classNum);
        if ((int)classNum != 2)
        {
            a.targets["Player"] = (int)BulletTarget.Player;
        }
        else
        {
            playerstatHandler.ATK.coefficient *= 1.1f;
            // �ϴ� ���� : ���� �������� UI�� ����ָ� ���� �� ������,,,? : �������� ����� �����ð���?
        }
    }
    //�׽�Ʈ �Ϸ� �׷��� �̺�Ʈ�� �۵��Ͽ� �߰� �׽�Ʈ�� �ʿ�
    [PunRPC]
    private void A124(int PlayerNumber)//���������� : �þ߰� ���� ���� �ϸ� ���� �ӵ�, ������ �ӵ��� �����մϴ�.<<�ָŸ�ȣ�ѵ�?
    {
        //PhotonView photonView = PhotonView.Find(PlayerPvNumber);
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        targetPlayer.AddComponent<A0124>();//A0124���� ȭ���Ӱ� �ϴ� ������ ����� �����������ۿ� ON ���� OFF
        playerstatHandler.AtkSpeed.added += 15;
        playerstatHandler.ReloadCoolTime.added += 2;
    }

    [PunRPC]
    private void A125(int PlayerNumber)//���� a0125 Ŭ���� ���� �� ��ġ �������� ���� ȸ���� �߰� a0125 ��ũ��Ʈ ���ܵα� �ߴµ� �������
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerstatHandler.evasionPersent = 20;
    }
    [PunRPC]
    private void A126(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A127(int PlayerNumber)//����� �ڷ�ƾ ������
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0127>();
    }
    [PunRPC]
    private void A128(int PlayerNumber)//������ �ǵ� ���� �ӵ� 140
    {
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetPhotonView().IsMine)
        {
            GameObject prefab = PhotonNetwork.Instantiate("AugmentList/A0128", targetPlayer.transform.localPosition, Quaternion.identity);
            int num = prefab.GetPhotonView().ViewID;
            photonView.RPC("FindMaster", RpcTarget.All, num);
        }
    }
    #endregion
    #region All2
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ����2Ƽ��
    [PunRPC]
    private void A201(int PlayerNumber)//���ʺ�
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.BulletLifeTime.coefficient *= 2f;
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        a.canAngle = true;
    }
    [PunRPC]
    private void A202(int PlayerNumber)//����Ʈ������ҷ� �Ѿ��̰Ŵ������ϴ� //���� 1.3��
    {
        ChangeOnlyPlayer(PlayerNumber);
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        a.sizeUp = true;
    }
    [PunRPC]
    private void A203(int PlayerNumber)//����Ŀ �ִ�ü�� / ���� ü�º�� ����
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0203>();
    }
    [PunRPC]
    private void A204(int PlayerNumber)//�������� �Ÿ���ʾ�����
    {
        ChangeOnlyPlayer(PlayerNumber);
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        a.locator = true;
    }
    [PunRPC]
    private void A205(int PlayerNumber)//�۽�Ʈ ���� ������ ù�Ѿ� ������ ����
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0205>();
    }
    [PunRPC]
    private void A206(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A207(int PlayerNumber)//���̸���ũ ���̸���
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.defense *= 0.5f;
        playerstatHandler.ATK.coefficient *= 2f;
    }
    [PunRPC]
    private void A208(int PlayerNumber)//ȸ���Ǵ���
    {
        ChangePlayerStatHandler(PlayerNumber);
        targetPlayer.AddComponent<A0208>();
    }
    [PunRPC]
    private void A209(int PlayerNumber)//������ ������� ������ ����
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0209>();
    }
    [PunRPC]
    private void A210(int PlayerNumber)
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.MaxRegenCoin += 1;
        playerstatHandler.RegenHP += 1;
    }


    [PunRPC]
    private void A211(int PlayerNumber)//���غ��� ����Ȯ���� ���� ü�� ȸ��
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0211>();
    }
    [PunRPC]
    private void A212(int PlayerNumber)//���ڸ�� ���� ���������� �������� ����
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0104>();
    }
    [PunRPC]
    private void A213(int PlayerNumber)//������ �÷��̾� ȥ�� �������� �ɷ�ġ��
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0213>();
    }
    [PunRPC]
    private void A214(int PlayerNumber)// ��Ÿ �ش�ȭ << �̸� �����ȵ� ��ų���� ������ ���� ���� << ���� ���ΰ�? �ױ��� �ƴѰ� ������
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerInput = targetPlayer.GetComponent<PlayerInput>();
        playerInput.actions.FindAction("Skill").Disable();
        playerstatHandler.isCanSkill = false;
        Debug.Log("�� ������ ����� ����� �˴ϴ� ��Ŭ�� üũ �ϰ� �����ּ���");
    }
    [PunRPC]
    private void A215(int PlayerNumber)//ȭ��
    {
        ChangePlayerStatHandler(PlayerNumber);
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        a.burn = true;
    }
    [PunRPC]
    private void A216(int PlayerNumber)//���̽�
    {
        ChangePlayerStatHandler(PlayerNumber);
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        a.ice = true;
    }
    [PunRPC]
    private void A217(int PlayerNumber)//����� ��� ������ �̼� ��������
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0217>();
    }
    [PunRPC]
    private void A218(int PlayerNumber)//������ ��ġ
    {
        ChangePlayerStatHandler(PlayerNumber);
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        a.gravity = true;
    }
    [PunRPC]
    private void A219(int PlayerNumber) //���峻��mk2 1,2,3 ���� ���� �̱⿡ �� ���ٸ� �ڵ��� 30
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        if (targetPlayer.GetComponent<BreakDownMk>()) //���� BreakDownMk�� ������ �ִٸ�
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
        ChangeOnlyPlayer(PlayerNumber);
        A0220 drainComponent = targetPlayer.GetComponent<A0220>();
        drainComponent.PercentUp(30);
        Debug.Log($"{drainComponent.percent}%�� Ȯ���� ���� ��");

    }
    [PunRPC]
    private void A221(int PlayerNumber)
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        targetPlayer.AddComponent<A0221>();
    }
    [PunRPC]
    private void A222(int PlayerNumber)//������ �������� ȸ��
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A0222>();
    }
    [PunRPC]
    private void A223(int PlayerNumber)
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.MaxSkillStack += 1;
        playerstatHandler.CurSkillStack += 1;
    }
    #endregion
    #region All3
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 3Ƽ��
    [PunRPC]
    private void A301(int PlayerNumber)//���峻�� mk3 1,2,3 ���� ���� �̱⿡ �� ���ٸ� �ڵ��� 
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        if (targetPlayer.GetComponent<BreakDownMk>()) //���� BreakDownMk�� ������ �ִٸ�
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
    private void A302(int PlayerNumber)//���Ǵ�Ƽ�ҷ� źâ 9999 ȹ������� �Ѿ� �� ����Ͽ� 9999�� ������ ���� ���� ����
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AmmoMax.added += 9999 - playerstatHandler.AmmoMax.total;
    }
    [PunRPC]
    private void A303(int PlayerNumber)//�н�
    {
        Debug.Log("�̿ϼ�");
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetPhotonView().IsMine)
        {
            GameObject prefab = PhotonNetwork.Instantiate("AugmentList/A0303", targetPlayer.transform.localPosition, Quaternion.identity);
            prefab.GetComponent<A0303>().Initialize(prefab.transform);
            int num = prefab.GetPhotonView().ViewID;
            photonView.RPC("FindMaster", RpcTarget.All, num);
        }
    }
    [PunRPC]
    private void A304(int PlayerNumber)
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.MaxRegenCoin += 1;
        playerstatHandler.HP.coefficient *= 0.5f;
    }
    [PunRPC]
    private void A305(int PlayerNumber)//��Ƽ�� ��2��
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.LaunchVolume.coefficient *= 2;
    }
    #endregion
    #region Sniper1
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@�������� 1Ƽ��
    [PunRPC]
    private void A1101(int PlayerNumber) //��⸸�� �� Ÿ�ݽ� ����
    {
        ChangePlayerStatHandler(PlayerNumber);
        targetPlayer.AddComponent<A1101>();
    }
    [PunRPC]
    private void A1102(int PlayerNumber)//�淮ȭ << �̸����� �̻��� ��ź���� 5 ���� �ϸ� ������ ����, ���� �ӵ� ����, �̵��ӵ� ������ ����ϴ�.
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerstatHandler.AmmoMax.added += 5;
        playerstatHandler.ATK.coefficient *= 0.9f;
        playerstatHandler.AtkSpeed.coefficient *= 1.1f;
        playerstatHandler.Speed.coefficient *= 1.1f;
    }
    [PunRPC]
    private void A1103(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1104(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1105(int PlayerNumber)//���� ����Ʈ  //���� �ڷ�ƾ ���װ� ����;
    {
        ChangePlayerStatHandler(PlayerNumber);
        targetPlayer.AddComponent<A1105>();
        playerInput = targetPlayer.GetComponent<PlayerInput>();
        playerInput.actions.FindAction("Skill").Disable();
        playerstatHandler.isCanSkill = false;
        playerstatHandler.ATK.coefficient *= 1.5f;
    }
    [PunRPC]
    private void A1106(int PlayerNumber)
    {
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetPhotonView().IsMine)
        {
            GameObject prefab = PhotonNetwork.Instantiate("AugmentList/A1106", targetPlayer.transform.localPosition, Quaternion.identity);
            int num = prefab.GetPhotonView().ViewID;
            photonView.RPC("FindMaster", RpcTarget.All, num);
            prefab.GetComponent<A1106>().Init();
        }
    }

    [PunRPC]
    private void A1107_1(int PlayerNumber)//������Ʈ ������ ���� ������
    {
        ChangeOnlyPlayer(PlayerNumber);
        GameObject Prefabs = Resources.Load<GameObject>("AugmentList/A1107");
        GameObject go = Instantiate(Prefabs, targetPlayer.transform);
        go.transform.SetParent(targetPlayer.transform);

    }
    [PunRPC]
    private void A1107(int PlayerNumber) //�������� ������ ��󿡰� ���������� �÷��ִ� Ÿ�� ���� ������ ������ ���������� �����
    {
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetPhotonView().IsMine)
        {
            GameObject Prefabs = PhotonNetwork.Instantiate("AugmentList/A1107", targetPlayer.transform.localPosition, Quaternion.identity);
            int num = Prefabs.GetPhotonView().ViewID;
            photonView.RPC("FindMaster", RpcTarget.All, num);
            Prefabs.GetComponent<A1107>().Init();
        }
    }
    #endregion
    #region Sniper2
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@�������� 2Ƽ��
    [PunRPC]
    private void A1201(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1202(int PlayerNumber)//����Ÿ� ���� ���������� �ݴ����
    {
        ChangeOnlyPlayer(PlayerNumber);
        WeaponSystem a = targetPlayer.GetComponent<WeaponSystem>();
        a.sniping = true;
    }
    [PunRPC]
    private void A1203(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1204(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1205(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1206(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1207(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    #endregion
    #region Sniper3
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@�������� 3Ƽ��
    [PunRPC]
    private void A1301(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1302(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1303(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A1304(int PlayerNumber)// ��ȸ��� ����� ���� x ����� ��������
    {
        ChangePlayerStatHandler(PlayerNumber);
        WeaponSystem weaponSystemA = targetPlayer.GetComponent<WeaponSystem>();
        playerInput = targetPlayer.GetComponent<PlayerInput>();
        playerInput.actions.FindAction("Skill").Disable();
        playerstatHandler.isCanSkill = false;
        weaponSystemA.isDamage = false;
        playerstatHandler.ATK.coefficient *= 1.5f;

    }
    #endregion
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 1Ƽ��
    [PunRPC]
    private void A2101(int PlayerNumber) //����� = ��ų����� ���� ���� �׽�Ʈ ��
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A2101>();
    }
    [PunRPC]
    private void A2102(int PlayerNumber) ///�ʹٴٴ٤��ٴ� �׽�Ʈ���� �ٵ� �����̶� �����������
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AtkSpeed.coefficient *= 2;
        playerstatHandler.ATK.coefficient *= 0.5f;
    }
    [PunRPC]
    private void A2103(int PlayerNumber)//���尨 �ֺ� �� ��� ���� ++//�̰� �������� ��û��ٷӳ�
    {
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetPhotonView().IsMine)
        {
            GameObject prefab = PhotonNetwork.Instantiate("AugmentList/A2103", targetPlayer.transform.localPosition, Quaternion.identity);
            int num = prefab.GetPhotonView().ViewID;
            photonView.RPC("FindMaster", RpcTarget.All, num);
            prefab.GetComponent<A2103>().Init();
        }
    }
    [PunRPC]
    private void A2104(int PlayerNumber)//���ⱳü :  �ڵ�� >> � ��ȯ �ִ� ��ź���� ����������  ������ ȿ���� ������Ű�� �ڵ�����κ���
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerstatHandler.AmmoMax.added -= 5;
        if (targetPlayer.GetComponent<Player1Skill>())
        {
            Player1Skill skill = targetPlayer.GetComponent<Player1Skill>();
            skill.applicationAtkSpeed += 2f;
            skill.applicationspeed += 2f;
        }

    }
    [PunRPC]
    private void A2105(int PlayerNumber)// ���� ���ݹ��� , �̵������� �ݴ밡�ǰ� ��ü ���� ���� == ���� �̵����� �ݴ븸 ���� A119 A2105�� ���� �Լ� ��ġ�°� ���
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerInput = targetPlayer.GetComponent<PlayerInput>();
        if (playerstatHandler.isNoramlMove)
        {
            playerInput.actions.FindAction("Move2").Enable();
            playerInput.actions.FindAction("Move").Disable();
            playerstatHandler.isNoramlMove = false;
        }
        else
        {
            playerInput.actions.FindAction("Move2").Disable();
            playerInput.actions.FindAction("Move").Enable();
            playerstatHandler.isNoramlMove = true;
        }
        playerstatHandler.HP.coefficient *= 1.5f;
        playerstatHandler.ATK.coefficient *= 1.5f;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 2Ƽ��
    [PunRPC]
    private void A2201(int PlayerNumber)// ��ƴ ����� //�⺻ ���� �� ������ ��Ÿ���� �����մϴ�.
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A2201>();
    }
    [PunRPC]
    private void A2202(int PlayerNumber)//ƼŸ�� ������ ��ų ���� ���ð� ����
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A2202>();
    }
    [PunRPC]
    private void A2203(int PlayerNumber)//�����ڸ��������� ���Ѵٴ°� ��ü�� ��ĳ ���� �𸣰��� //�̿ϼ�
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A2203_1>();
    }
    [PunRPC]
    private void A2204(int PlayerNumber)//��������
    {
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetPhotonView().IsMine)
        {
            GameObject prefab = PhotonNetwork.Instantiate("AugmentList/A2204", targetPlayer.transform.localPosition, Quaternion.identity);
            int num = prefab.GetPhotonView().ViewID;
            photonView.RPC("FindMaster", RpcTarget.All, num);
            prefab.GetComponent<A2204>().Init();
        }
    }
    [PunRPC]
    private void A2205(int PlayerNumber)//���ⱳü ���Ʈ������ >> ������ źâ ��ź�� 30+ �̵��ӵ�- ������ ���
    {
        ChangePlayerStatHandler(PlayerNumber);
        playerstatHandler.AmmoMax.added += 30;
        playerstatHandler.Speed.added -= 2;
        playerstatHandler.RollCoolTime.added += 2;
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 3Ƽ��
    [PunRPC]
    private void A2301(int PlayerNumber)// ���� �Ѿ��� 1���� ������ ������ �Ѿ˼� ��� �� ++
    {
        ChangePlayerStatHandler(PlayerNumber);
        float changePower = playerstatHandler.AmmoMax.total - 1;
        playerstatHandler.AmmoMax.added -= playerstatHandler.AmmoMax.total - 1;
        playerstatHandler.ATK.added += changePower * 0.5f;
    }
    [PunRPC]
    private void A2302(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A2303(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A2304(int PlayerNumber)//������ ������ �Ϻ� ��� ����
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerInput = targetPlayer.GetComponent<PlayerInput>();
        playerInput.actions.FindAction("Skill").Disable();
        playerstatHandler.isCanSkill = false;
        if (targetPlayer.GetComponent<Player1Skill>())
        {
            Player1Skill skill = targetPlayer.GetComponent<Player1Skill>();
            skill.applicationAtkSpeed *= 0.5f;
            skill.applicationspeed *= 0.5f;
        }
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 1Ƽ��
    [PunRPC]
    private void A3101(int PlayerNumber)//���½ð�
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A3101>();
    }
    [PunRPC]
    private void A3102(int PlayerNumber)//������ ��ų ����� ü������
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A3102>();
    }
    [PunRPC]
    private void A3103(int PlayerNumber)//������ �����⸦ ������� ���� 
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A3103>();
    }
    [PunRPC]
    private void A3104(int PlayerNumber)
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A3105(int PlayerNumber)//�����¼� ��ų ���� ���� ������ ��ȭ ��Ű�� ��ų�� ��ü #��ų��ü #��������
    {
        ChangeOnlyPlayer(PlayerNumber);
        targetPlayer.AddComponent<A3105>();
    }
    [PunRPC]
    private void A3106()
    {
        Debug.Log("�̿ϼ�");
    }
    [PunRPC]
    private void A3107(int PlayerNumber) // ���̾� ����̵� �׽�Ʈ����
    {
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetPhotonView().IsMine)
        {
            GameObject prefab = PhotonNetwork.Instantiate("AugmentList/A3107", targetPlayer.transform.localPosition, Quaternion.identity);
            prefab.GetComponent<A3107>().Init(targetPlayer);
            int num = prefab.GetPhotonView().ViewID;
            photonView.RPC("FindMaster", RpcTarget.All, num);
        }

    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 2Ƽ��
    [PunRPC]
    private void A3201(int PlayerNumber) //������ ���� << ������ 2������ ��ź�� +3���� ������
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerstatHandler.RollCoolTime.added += 2f;
        targetPlayer.AddComponent<A3201>();
    }
    [PunRPC]
    private void A3202(int PlayerNumber)//���� ���� ���� �׼� ������ �Ⱦ���ִ� ��ź���� 5 ���� �ϸ� ����ӵ� �� ��� ���ݷ��� ���� �ҽ��ϴ�.
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerstatHandler.AmmoMax.added += 5;
        playerstatHandler.AtkSpeed.coefficient *= 1.2f;
        playerstatHandler.ATK.coefficient *= 0.9f;
    }
    [PunRPC]
    private void A3203(int PlayerNumber)//������� ��2��ü��3��
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        float x = (targetPlayer.transform.localScale.x * 2f);//����
        float y = (targetPlayer.transform.localScale.y * 2f);//����
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
    private void A3205(int PlayerNumber)//����� ������ �ι� ���� �ǵ�  //2204�� ���ǹ���
    {
        ChangeOnlyPlayer(PlayerNumber);
        if (targetPlayer.GetPhotonView().IsMine)
        {
            GameObject prefab = PhotonNetwork.Instantiate("AugmentList/A3205", targetPlayer.transform.localPosition, Quaternion.identity);
            int num = prefab.GetPhotonView().ViewID;
            photonView.RPC("FindMaster", RpcTarget.All, num);
            prefab.GetComponent<A3205>().Init();
        }
    }
    [PunRPC]
    private void A3206(int PlayerNumber)//���� ��ų
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        targetPlayer.AddComponent<A3206>();
        playerstatHandler.SkillCoolTime.added += 5f;
    }
    [PunRPC]
    private void A3207(int PlayerNumber)//��ȣ ���
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        if (targetPlayer.GetComponent<Player2Skill>())
        {
            Player2Skill player2 = targetPlayer.GetComponent<Player2Skill>();
            player2.shieldScale += 0.5f;
        }
        playerstatHandler.HP.coefficient *= 0.8f;
        targetPlayer.AddComponent<A3207>();
    }
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@���� 3Ƽ��
    [PunRPC]
    private void A3301(int PlayerNumber)//�Ѿ˺κ� ó������ �Ʊ� �Ѿ˿� ������ ���谡 ���� 
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        targetPlayer.AddComponent<A3301>();
    }
    [PunRPC]
    private void A3302(int PlayerNumber)//���� ���� ����, ���差 ����,  ��Ÿ ��ȭ,  ���� �ȿ� �Ʊ� ����
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        if (targetPlayer.GetComponent<Player2Skill>())
        {
            Player2Skill player2 = targetPlayer.GetComponent<Player2Skill>();
            player2.shieldScale *= 2f;
            player2.shieldHP += 20f;
        }
        playerstatHandler.ATK.coefficient *= 0.8f;
    }
    [PunRPC]
    private void A3303(int PlayerNumber)//��ġ�� ����
    {
        ChangePlayerAndPlayerStatHandler(PlayerNumber);
        playerInput = targetPlayer.GetComponent<PlayerInput>();
        playerstatHandler.AmmoMax.added += 5f;
        playerstatHandler.AtkSpeed.added += 2f;
        playerstatHandler.RollCoolTime.added -= 2f;
    }
}
