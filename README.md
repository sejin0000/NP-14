# NP-14
<br>

## 목차
| [목차](#목차) |
| :---: |
|[🕶 게임 설명](#-게임-설명)|
|[🕶 기본 조작법](#-기본-조작법)|
|[🕶 트레일러](#-트레일러)|
|[🕶 기술 스택](#-기술-스택)|
|[🕶 기능 설명](#-기능-설명)|
|[🕶 팀원 소개](#-팀원-소개)|


## 🕶 게임 설명


로그라이크 탑 다운 슈팅 게임으로 1인에서 부터 최대 3인이 함께 즐길 수 있는 로그라이크 형식의 슈팅 게임.

<br>

![표지](https://github.com/sejin0000/NP-14/assets/129154514/de293708-c6da-4bd2-a27e-01807eecd05d)

<br>

### 1. 닉네임 입력

닉네임을 입력하여 로비에 입장할 수 있다.


<p align="Left">
<img src="https://github.com/sejin0000/NP-14/assets/129154514/c20a0af4-5f78-40bc-b6c0-6a4fd808943c">
</p>

<br>

메인로비 - 설정에서 추가적으로 닉네임을 변경할 수 있다.

<br>

<p align="Left">
<img src="https://github.com/sejin0000/NP-14/assets/129154514/eaa9af2d-bebc-4c0c-ab75-988896726e77">
</p>
<br>

### 2. 룸 입장

메인로비 - 빠른 시작으로 랜덤한 파티원과 게임 플레이가 가능하다.


<p align="Left">
<img src="https://github.com/sejin0000/NP-14/assets/129154514/12d18575-3b46-4d52-977a-fceb3c291970">
</p>
<br>


혹은, 메인로비 - 방 찾기로 특정한 룸에서 게임 플레이가 가능하다.


<p align="Left">
<img src="https://github.com/sejin0000/NP-14/assets/129154514/2a566241-7794-4caa-91f6-5ed225922727">
</p>
<br>

### 3. 게임 플레이

좌클릭으로 몬스터에게 총을 쏠 수 있다.


<p align="Left">
<img src="https://github.com/sejin0000/NP-14/assets/129154514/1b67cc00-ceb7-4e93-b911-0ad6dfa3da7d">
</p>
<br>
우클릭으로 스킬을 사용할 수 있다.
직업별 스킬은 다음과 같다.

#### 1. 라이플 : 일정 시간동안 공격속도 및 이동속도를 증가시킨다.
#### 2. 샷건 : 일정 시간동안 자신 주위의 자신과 팀원을 지켜주는 쉴드를 생성한다.
#### 3. 스나이퍼 : 힐모드와 딜모드로 상시 변경 가능. 딜모드는 일반 평타, 힐 모드는 파티원에게 공격을 적중시켰을 시, 힐을 부여한다.


<p align="Left">
<img src="https://github.com/sejin0000/NP-14/assets/129154514/c1a830cb-37ef-4d2b-9238-4d58bcdbba35">
</p>
<br>

### 4. 증강 선택

룸 클리어와 스테이지 클리어시마다, 증강을 선택하여 획득할 수 있다.
<br></br>
룸 클리어시에는 20% 확률로 사용자의 플레이스타일을 변경시킬 수 있는 증강이 등장한다.


그 외에는 사용자를 강화시켜주는 일반 스탯 증강이 등장한다.


<p align="Left">
<img src="https://github.com/sejin0000/NP-14/assets/129154514/c1437664-a6f3-401e-a2c7-e5e26610ba10">
</p>
<br>
스테이지 클리어 시에는 100% 확률로 사용자의 플레이스타일을 변경시킬 수 있는 증강이 등장한다.

<p align="Left">
<img src="https://github.com/sejin0000/NP-14/assets/129154514/99ba3781-df10-4e50-8cfd-3956f2197907">
</p>
<br>

### 5. 보스

2개의 스테이지를 클리어할 때마다, 보스와 전투를 벌이는 보스 스테이지가 등장한다.


<p align="Left">
<img src="https://github.com/sejin0000/NP-14/assets/129154514/f18bba51-8168-4013-8849-ca179e5adaaf">
</p>
<br>

## 🕶 기본 조작법  
![KEY_Setting (1)](https://github.com/sejin0000/NP-14/assets/129154514/a3c91c15-39c7-4ac8-88b1-3998eaa92cf9)

<br>

## 🕶 트레일러

<br>

[![Video Label](http://img.youtube.com/vi/1Tf4Kwndvs0/0.jpg)](https://youtu.be/1Tf4Kwndvs0&ab)

<br>

## 🕶 기술 스택

<br>

![image (3)](https://github.com/sejin0000/NP-14/assets/141595995/9cfeaa0f-f2c1-4231-bfde-a91352977e47)

<br>

#### 프레임워크 & 언어

- C#
- .Net 6.0

#### 개발 환경

- VisualStudio
- Windows 10

#### 데이터 관리

- Google SpreadSheet

#### 게임 엔진

- Unity - 2022.3.2f1
- Photon PUN2

#### 디자인

- Aesprite

<br>


## 🕶 기능 설명

<br>

### A. LobbyScene
##### 1. 설명

플레이어 닉네임 등록, 로비 및 룸 구성을 통해 파티원을 모집한다.

##### 2. 상세 설명

####
|기능 이름|기능 설명|스크립트|
|:---:|:---:|:---:|
|LobbyManager|플레이어가 어떤 패널에 있는 지 (어떤 상태인지) 정의하고, 관련된 상태에 대한 오브젝트들을 호출한다.|[LobbyManager.cs](https://github.com/sejin0000/NP-14/blob/74c9fd910eb7af5f3fecb5874af04b7550233f83/Assets/Script/Lobby/LobbyManager.cs#L29-L31)|
|NetworkManager|플레이어의 네트워크 상태를 통해 MonoBehaviorPuncallbacks를 상속받아, 특정 상태에서의 이벤트를 관리한다.|[NetworkManager.cs](https://github.com/sejin0000/NP-14/blob/b5465df3a705bf84b3ad18c4c8ea192d84f6eb40/Assets/Script/Lobby/NetworkManager.cs#L13)|
|LoginPanel|시작 시, 게임 화면 및 닉네임 입력 팝업을 노출시킨다.|[LoginPanel.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/LoginPanel.cs#L12)|
|MainLobbyPanel|설정, 빠른 시작, 방 탐색 패널을 선택하여 입장시킨다.|[MainLobbyPanel.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/MainLobbyPanel.cs#L14)|
|RoomFindPanel|특정 이름의 방을 검색하여 참여하거나, 방을 직접 만든다.|[RoomFindPanel.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/RoomFindPanel.cs#L14)|
|RoomPanel|방 인원 수를 조정하거나, 플레이어의 레디 여부에 따라 호스트에게 게임을 시작하게 하여 MainGameScene으로 전환한다.|[RoomPanel.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/RoomPanel.cs#L18)|
|LoadingPanel|특정 패널간의 이동시, 노출한다.|[LoadingPanel.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/LoadingPanel.cs#L8)|

<br>

##### 3. 스크립트 별 설명

##### 3-1. LobbyManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|패널간 이동|특정 패널을 보여주고, 특정 패널의 상태임을 저장한다.|LobbyManager.cs|[SetPanel()](https://github.com/sejin0000/NP-14/blob/b5465df3a705bf84b3ad18c4c8ea192d84f6eb40/Assets/Script/Lobby/LobbyManager.cs#L206)|
|캐릭터 변경 팝업 추가|캐릭터 변경 팝업을 인스턴스화 함으로써, UI에 가장 마지막에 배치되게 하여, 제일 상단에 팝업이 노출되게 한다.|LobbyManager.cs|[InstantiateCharacterSelectPopup()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/LobbyManager.cs#L187)|

<br>

##### 3-2. NetworkManager
####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|로비 입장 시 이벤트|플레이어가 로비에 입장했을 때, 실행되어야 하는 코드|NetworkManager.cs|[OnJoinedLobby()](https://github.com/sejin0000/NP-14/blob/b5465df3a705bf84b3ad18c4c8ea192d84f6eb40/Assets/Script/Lobby/NetworkManager.cs#L58)|
|룸 입장 시 이벤트|플레이어가 룸에 입장했을 때, 로비매니저에서의 상태에 따라 실행되는 코드. 파티를 보여주는 파티박스를 최신화하고 , 다른 플레이어의 상태(선택한 직업)를 적용시킨다.|NetworkManager.cs|[OnJoinedRoom()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/NetworkManager.cs#L99)|
|타 플레이어가 룸 입장 시 이벤트|다른 플레이어가 룸에 입장했을 때, 실행되는 코드. 파티를 보여주는 파티박스를 최신화하고, 레디 여부를 |NetworkManager.cs|[OnPlayerEnteredRoom()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/NetworkManager.cs#L136)|
|룸 퇴장 시 이벤트|플레이어가 룸에서 퇴장했을 때, 로비매니저에서의 상태에 따라 실행되는 코드. 룸에서 탈퇴시킨 후, 다시 연결을 함.|NetworkManager.cs|[OnLeftRoom()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/NetworkManager.cs#L166C26-L166C38)|
|타 플레이어가 룸 퇴장 시 이벤트|다른 플레이어가 룸에서 퇴장했을 때, 실행되는 코드. 플레이어 ActorNumber를 key, 플레이어 오브젝트를 value로 하는 딕셔너리에서 해당 플레이어를 삭제시키고, 레디 여부를 확인하는 코드를 재실행한다.|NetworkManager.cs|[OnPlayerLeftRoom()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/NetworkManager.cs#L158)|
|룸 프로퍼티 변경 시 이벤트|룸 프로퍼티가 변경됬을 때, 해당 게임에서는 룸의 최대 인원수가 조정되었을 때, 실행되는 코드.|NetworkManager.cs|[OnRoomPropertiesUpdate()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/NetworkManager.cs#L212C26-L212C48)|
|플레이어 프로퍼티 변경 시 이벤트|플레이어의 프로퍼티가 변경됬을 때, 해당 게임에서는 플레이어가 선택한 직업, 레디 여부가 변경되었을 때 실행되는 코드.|NetworkManager.cs|[OnPlayerPropertiesUpdate()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/NetworkManager.cs#L237)|


<br>


##### 3-4. LoginPanel

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|클릭 시 게임 시작|클릭 시, 닉네임 변경 팝업을 인스턴스화 시킨다.|LoginPanel.cs|[OnPointerClick()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/LoginPanel.cs#L44)|
|닉네임 팝업 안내 문구 조정|닉네임 팝업의 상태에 따라, 알맞은 안내문구를 노출시킨다.|NickNamePopup.cs|[SetAnnouncement()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Setup/NickNamePopup.cs#L57)|
|닉네임 체크|입력한 닉네임이 적합한지, 정규식을 통해 확인하고, 그 여부에 따라 적합한 닉네임 팝업의 상태로 전환하며, 버튼 별 기능을 연결한다.|NickNamePopup.cs|[CheckNickname()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Setup/NickNamePopup.cs#L78)|

<br>

##### 3-5. MainLobbyPanel 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|버튼 별 이벤트트리거 설정|ButtonList에 담겨 있는 버튼들에 이벤트 트리거 컴포넌트를 추가하여, 해당 버튼에 마우스가 들어왔을 시, 설명 팝업이 노출되도록 한다.|MainLobbyPanel.cs|[GetButtonEventTrigger()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/MainLobbyPanel.cs#L89)|
|팝업 안내 문구 수정|ButtonList에 담겨 있는 버튼에 따라 안내 문구를 변경시킨다.|MainLobbyPanel.cs|[ActivateMLPopup()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/MainLobbyPanel.cs#L122)|
|빠른 시작 버튼 입력|랜덤한 룸에 진입하거나, 룸에 입장한 플레이어가 없을 경우, 방을 생성한다.|MainLobbyPanel.cs|[OnQuickStartButtonClicked()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/MainLobbyPanel.cs#L154C18-L154C45)|
|방 찾기 버튼 입력|방을 직접 생성하거나, 선택하여 입장할 수 있는 로비로 이동한다.|MainLobbyPanel.cs|[OnFindRoomButtonClicked()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/MainLobbyPanel.cs#L177)|
|설정 버튼 입력|설정 팝업을 인스턴스화 한다.|MainLobbyPanel.cs|[OnSettingButtonClicked()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/MainLobbyPanel.cs#L187)|



<br>

##### 3-6. RoomFindPanel 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|방 생성 버튼 입력|방을 직접 생성한다. 방 제목과, 랜덤 참여 여부를 설정할 수 있다.|RoomFindPanel.cs|[OnRoomCreateButtonClicked()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/RoomFindPanel.cs#L116)|
|방 제목 검색|검색한 문구가 포함되어 있는 방을 검색한다.|RoomFindPanel.cs|[OnRoomSearchButtonClicked()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/RoomFindPanel.cs#L173)|

<br>

##### 3-7. RoomPanel 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|파티원 정보 초기화|파티원이 선택한 캐릭터, 준비 여부가 변할 때마다, 파티박스를 최신화한다.|RoomPanel.cs|[SetPartyPlayerInfo()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/RoomPanel.cs#L303)|
|플레이어 인스턴스화|다른 클라이언트와 해당 클라이언트의 플레이어 오브젝트를 인스턴스화 한다.|RoomPanel.cs|[InstantiatePlayer()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/RoomPanel.cs#L367)|
|플레이어 준비 상태 반영|플레이어가 준비 상태에 따라, 맞는 화면과 커스텀 프로퍼티를 등록한다.|RoomPanel.cs|[OnReadyButtonClicked()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/RoomPanel.cs#L394)|


<br>

##### 3-7. LoadingPanel 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|로딩 패널 반영|해당 로딩 패널을 초기화한다. 파라미터로 로딩패널이 진행되는 시간을 받을 수 있다.|LoadingPanel.cs|[Initialize()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Lobby/Panel/LoadingPanel.cs#L20C17-L20C27)|

<br>


### B. MainGameScene
##### 1. 설명

설명 본문 입력

##### 2. 상세 설명

####
|기능 이름|기능 설명|스크립트|
|:---:|:---:|:---:|
|MonsterSpawner|방마다 스크립터블 오브젝트를 통해 몬스터를 생성 및 삭제해주는 역할을 수행합니다.|[MonsterSpawner.cs](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/MonsterSpawner)|
|MapGenerator|매 스테이지에서 랜덤으로 맵을 만들어주는 역할을 수행합니다.|[MapGenerator.cs](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Map/MapGenerator)|
|MakeAugmentListManager|게임 시작시 플레이어의 정보를 받아 캐릭터(직업)의 정보를 외부 csv파일로 불러 리스트로 만들어 줍니다.|[MakeAugmentListManager.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Park/AugmentControl/MakeAugmentListManager.cs#L9)||
|AugmentManager|증강(아이템)의 데이터 베이스로 해당 증강의 Code를 호출시 해당 효과를 플레이어에게 적용 시켜줍니다.|[AugmentManager.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Park/AugmentControl/AugmentManager.cs#L11)||
|ResultManager|옵저버 패턴을 통해 룸,스테이지 클리어시 호출 되어 플레이어는 보상을 선택 할 수 있습니다.|[ResultManager.cs](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager)(https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Park/AugmentControl/ResultManager.cs#L14)||
|GameManager|게임의 기본적인 흐름을 관리하며 옵저버 패턴을 이용해 특정상황에 맞는 이벤트를 뿌려줍니다.|[GameManager.cs]()|
|UIManager|UI_Root에서 사용하는 UI_Base 객체를 Initialize한다. 이때 이벤트 등록 등의 작업을 처리한다.|[UIManager.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/Manager/UIManager.cs)|
|UI_Root|Scene에서 표시되는 Canvas 오브젝트이다.|GameObject|
|ParticleManager|특정 좌표에 파티클 오브젝트를 생성한다. 파티클 오브젝트를 캐싱하여 관리한다.|[ParticleManager.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/Manager/ParticleManager.cs)|
|AudioManager|BGM/SE를 출력한다. 배경음/효과음을 `Dictionary`에 등록하여 캐싱하고 출력한다.|[AudioManager.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/Manager/AudioManager.cs)|
|portal|스테이지의 종료를 알리는 역할을 수행합니다.|[Portal.cs](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Map/Portal.cs)|
|MainGameNetwork|플레이어 탈퇴 시, 로딩 패널을 노출된다.|[MainGameNetwork.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/MainGameScene/Network/MainGameNetwork.cs#L7)|

##### 3. 기능 별 설명

##### 3-1. MonsterSpawner 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|MonsterSpawn|현재 스테이지 생성된 방의 개수를 플레이어수를 고려해 몬스터들을 방안에 생성해 줍니다.|MonsterSpawner.cs|[MonsterSpawn()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/MonsterSpawner.cs#L20)|
|BossSpawn|현재 스테이지에 맞는 보스를 생성합니다.|MonsterSpawner.cs|[BossSpawn()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/MonsterSpawner.cs#L59)|
|Spawn|몬스터의 종류를 enum 형태로 갖고 있는 스크립터블 오브젝트에서 enum 값을 string으로 변환해 PhotonNetwork.Instantiate()를 통해 몬스터를 생성하는 역할을 합니다|MonsterSpawner.cs|[Spawn()]https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/MonsterSpawner.cs#L90C1-L90C1|
|StageMonsterClear|스테이지가 종료후 클리어하지 않은 방에 남아있는 몬스터들을 전부 삭제해주는 역할을 수행합니다|MonsterSpawner.cs|[StageMonsterClear()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/MonsterSpawner.cs#L140C17-L140C34)|
<br>

##### 3-2. MapGenerator 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|MapMake|랜덤한 크기로 방을 만들고 그방에 통로를 연결한 후 통로 사이에 문을 만들어 랜덤맵을 만드는 역할을 수행합니다.|MapGenerator.cs|[MapMake()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Map/MapGenerator.cs#L95C17-L95C26)|
|Divide|원하는 만큼 이진분활을 해 맵을 쪼개고 그정보를 노드를 만들어 담습니다. 그리고 나누어진 노드들은 이진분활 되기전 부모 노드가 변수로 갖고 있게 됩니다.|MapGenerator.cs|[Divide()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Map/MapGenerator.cs#L141C2-L141C2)|
|GenerateRoom|나누어진 노드 안에 생성될 방의 크기를 랜덤으로 정해줍니다.|MapGenerator.cs|[GenerateRoom()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Map/MapGenerator.cs#L165C21-L165C33)|
|FindNodeChildren|부모 노드를 매개 변수로 받아 그 하위 노드들을 전부 찾아 자식 노드 리스트에 담습니다|MapGenerator.cs|[FindNodeChildren()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Map/MapGenerator.cs#L202C18-L202C34)|
|GenerateLoad|이어줄 두개의 부모 노드의 하의 노드가 담긴 두개의 리스트에서 최소 거리를 비교해 가장 짦은 길을 만들어 줍니다.|MapGenerator.cs|GenerateLoad[()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Map/MapGenerator.cs#L231C18-L231C30)|

<br>

##### 3-3. MakeAugmentListManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|makeList|플레이어의 정보를 받아 리스트를 만들어 주는 메서드|MakeAugmentListManager.cs|[makeList()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Park/AugmentControl/MakeAugmentListManager.cs#L71)|
|SpecialAugmentSetting|csv파일의 이름,설명,레어도,코드를 리스트에 담아주는 메서드|MakeAugmentListManager.cs|[SpecialAugmentSetting()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Park/AugmentControl/MakeAugmentListManager.cs#L116)|

<br>

##### 3-4. AugmentManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|AugmentCall|ResultManager에게서 전달받은 Code를 해당 플레이어를 찾아 적용 시켜 줍니다.|AugmentManager.cs|[AugmentCall()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Park/AugmentControl/AugmentManager.cs#L49)|
|A901|증강이 실질적으로 적용되는 코드로 함수명을 숫자로만 지을수 없기에 A + Code번호로 구성되며 900번대는스탯, 100~300번대는 공용, 1000,2000,3000번대는 각각 직업 스나이퍼, 솔져, 샷건의 증강 번호를 가지고 있습니다. |AugmentManager.cs|[A901()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Park/AugmentControl/AugmentManager.cs#L80)|

<br>

##### 3-5. ResultManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|PickStatList|룸(게임의 작은 스테이지 단위)클리어시 일반 보상인 스탯을 고를수 있게 해줍니다.|ResultManager.cs|[PickStatList()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Park/AugmentControl/ResultManager.cs#L232)|
|PickSpecialList|스테이지클리어시 스페셜 증강을 고를수 있게 해줍니다 일반 스탯 증강과 다르게 모든 플레이어가 다음 스테이지로 갈 준비가 끝났는지 판단하는 ready함수를 가지고 있습니다|ResultManager.cs|[PickSpecialList()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Park/AugmentControl/ResultManager.cs#L255)|
|close|'PickStatList'와 'PickSpecialList'공용으로 사용되며 플레이어가 선택시 나온 UI의 액티브를 꺼주며 만약 스페셜 리스트였다면 중복뽑기를 방지해줍니다.|ResultManager.cs|[close()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/Park/AugmentControl/ResultManager.cs#L283)|

<br>

##### 3-6. GameManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|Awake|게임 내의 초기화 및 이벤트들을 순서에 맞게 연결해주는 역할을 수행 합니다.|GameManager.cs|[Awake()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L64C18-L64C23)|
|Start|스테이지가 시작할때마다 호출되어 스크립터블 오브젝트에서 현재 스테이지 정보를 읽어와 그에 맞는 이벤트들 뿌려주는 역할을 수행합니다.|GameManager.cs|[Start()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L125C18-L125C23)|
|CallStageSettingEvent|노말 스테이지를 세팅하는 이벤트를 뿌려줍니다.|GameManager.cs|[CallStageSettingEvent()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L158)|
|CallStageStartEvent|스테이지의 시작을 알리는 이벤트를 뿌려줍니다.|GameManager.cs|[CallStageStartEvent()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L169C38-L169C38)|
|CallRoomStartEvent|플레이어가 방에 들어가면 뿌려줘야 하는 이벤트를 뿌려줍니다.|GameManager.cs|[CallRoomStartEvent()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L186C17-L186C35)|
|CallStageEndEvent|스테이지의 종료 이벤트를 뿌려줍니다.|GameManager.cs|[CallStageEndEvent()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L211C17-L211C34)|
|StageClear|스테이지가 클리어되면 다음 스테이지가 있는지 확인하고 있다면 Start 함수를 호출하고 없다면 CallGameClearEvent 를 호출합니다.|GameManager.cs|[StageClear()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L243C17-L243C27)|
|CallBossStageSettingEvent|보스 스테이지를 세팅하는 이벤트를 뿌려줍니다.|GameManager.cs|[CallBossStageSettingEvent()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L261C17-L261C42)|
|CallBossStageStartEvent|보스 스테이지의 시작을 알리는 이벤트를 뿌려줍니다|GameManager.cs|[CallBossStageStartEvent()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L269C17-L269C40)|
|CallBossStageEndEvent|보스 스테이지의 종료 이벤트를 뿌려줍니다.|GameManager.cs|[CallBossStageEndEvent()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L275C17-L275C38)|
|CallGameClearEvent|게임이 클리어 되면 실행되야하는 이벤트를 실행해줍니다|GameManager.cs|[CallGameClearEvent()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L281C17-L281C35)|
|CallGameOverEvent|게임 오버가 되면 실행해야하는 이벤트를 시행해줍니다|GameManager.cs|[CallGameOverEvent()](https://github.com/sejin0000/NP-14/blob/c5244a0a87431c254125e0ad63ca174f6284c831/Assets/Script/Sejin/Manager/GameManager.cs#L292C17-L292C34)|
|||GameManager.cs|[()]()|


<br>

##### 3-7. UIManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|Layer|`UIManager`에서 관리할 UI 오브젝트를 담아놓는 리스트|[UIManager.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/Manager/UIManager.cs)||
|Initialize|Layer에 담겨있는 UI_Base 객체의 Initialize 함수를 실행한다.|UIManager.cs|[Initialize()](https://github.com/sejin0000/NP-14/blob/e771c254484ecb7ef191ebf19c1a7fb79b270577/Assets/Script/Manager/UIManager.cs#L29)|
|Open|해당 UI 오브젝트의 Active를 True로 변경한다.|UIManager.cs|[Open()](https://github.com/sejin0000/NP-14/blob/e771c254484ecb7ef191ebf19c1a7fb79b270577/Assets/Script/Manager/UIManager.cs#L50C5-L50C5)|
|Close|해당 UI 오브젝트의 Active를 False로 변경한다.|UIManager.cs|[Close()](https://github.com/sejin0000/NP-14/blob/e771c254484ecb7ef191ebf19c1a7fb79b270577/Assets/Script/Manager/UIManager.cs#L60C7-L60C7)|
|GetUIComponent|UI 매니저에 등록된 UI에서 특정 컴포넌트를 찾아서 반환한다.|UIManager.cs|[GetUIComponent()](https://github.com/sejin0000/NP-14/blob/e771c254484ecb7ef191ebf19c1a7fb79b270577/Assets/Script/Manager/UIManager.cs#L84C14-L84C28)|
|GetUIObject|UI 매니저에 등록된 UI에서 특정 이름의 오브젝트를 찾아서 반환한다.|UIManager.cs|[GetUIObject()](https://github.com/sejin0000/NP-14/blob/e771c254484ecb7ef191ebf19c1a7fb79b270577/Assets/Script/Manager/UIManager.cs#L100)|


<br>

##### 3-8. UI_Root 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|UIStageTransition|스테이지가 시작될 때 탑을 오르는 연출을 표시한다.|[UITransition.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/UI/Transition/UIStageTransition.cs)||
|UIStageBlock|`UIStageTransition`에서 사용하는 블럭을 관리한다.|[UIStageBlock.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/UI/Transition/UIStageBlock.cs)||
|UIPlayerHUD|플레이어의 체력/회피/스킬 게이지를 관리한다.|[UIPlayerHUD.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/UI/Player/UIPlayerHUD.cs)||
|UIPlayerHP|플레이어의 체력 게이지를 관리한다.|[UIPlayerHP.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/UI/Player/UIPlayerHP.cs)||
|UIPlayerDodge|플레이어의 회피 쿨타임 게이지를 관리한다.|[UIPlayerDodge.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/UI/Player/UIPlayerDodge.cs)||
|UIPlayerSkill|플레이어의 스킬 쿨타임 게이지를 관리한다.|[UIPlayerSkill.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/UI/Player/UIPlayerSkill.cs)||
|UIBulletIndicator|플레이어의 탄창을 표시한다.|[UIBulletIndicator.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/UI/Player/UIBulletIndicator.cs)||
|UIPlayerMiniHUD|플레이어의 머리 위에 표시되는 MiniHUD를 관리한다.|[UIPlayerMiniHUD.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/UI/Player/UIPlayerMiniHUD.cs)||
|UIPlayerMiniHP|플레이어의 머리 위에 표시되는 체력 게이지를 관리한다.|[UIPlayerMiniHP.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/UI/Player/UIPlayerMiniHP.cs)||
|UIReloadHUD|플레이어의 머리 위에 표시되는 재장전 게이지를 관리한다.|[UIReloadHUD.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/UI/Player/UIReloadHUD.cs)||
|SetupPopupPrefab|환경설정 UI를 표시한다. |[SetupPopup.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/Setup/SetupPopup.cs)||
|GameClearPanel|게임오버, 클리어 시 해당 UI를 표시한다.|[GameClearPanel.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/MainGameScene/UI/GameClearPanel.cs)||
|LoadingPanel|다른 화면으로 이동할 때(로딩 시) 표시한다.|[LoadingPanel.cs](https://github.com/sejin0000/NP-14/blob/main/Assets/Script/Lobby/Panel/LoadingPanel.cs)||
|Minimap|인 게임에서 표시되는 미니맵 관련 UI입니다.|GameObject||


<br>

##### 3-9. ParticleManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|PlayEffectLocal|파티클 오브젝트를 특정 좌표(Vector3)에 로컬 플레이어에게 표시한다.||[PlayEffectLocal()](https://github.com/sejin0000/NP-14/blob/79ebeacfed23a3de591d273c01396b0c271cd740/Assets/Script/Manager/ParticleManager.cs#L41C8-L41C8)|
|SendEffect|파티클 오브젝트를 특정 좌표(Vector3)에 모든 플레이어에게 표시한다.||[SendEffect()](https://github.com/sejin0000/NP-14/blob/79ebeacfed23a3de591d273c01396b0c271cd740/Assets/Script/Manager/ParticleManager.cs#L91C16-L91C16)|


<br>

##### 3-10. AudioManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|PlayBGM|`Dictionary`에 등록된 BGM을 출력한다.|AudioManager.cs|[PlayBGM()](https://github.com/sejin0000/NP-14/blob/79ebeacfed23a3de591d273c01396b0c271cd740/Assets/Script/Manager/AudioManager.cs#L106C24-L106C31)|
|PlaySE|`Dictionary`에 등록된 BGM을 출력한다.|AudioManager.cs|[PlaySE()](https://github.com/sejin0000/NP-14/blob/79ebeacfed23a3de591d273c01396b0c271cd740/Assets/Script/Manager/AudioManager.cs#L160)|
|PlayClip|인자로 전달된 `AudioClip`을 출력한다.|AudioManager.cs|[PlayClip()](https://github.com/sejin0000/NP-14/blob/79ebeacfed23a3de591d273c01396b0c271cd740/Assets/Script/Manager/AudioManager.cs#L191)|
|AudioLibrary|AudioManagerTest에서 출력할 효과음을 지정한다.|AudioLibrary.cs||
|SetupPlayerSE|플레이어가 사용할 사운드 출력 함수를 이벤트에 등록한다.|AudioLibrary.cs|[SetupPlayerSE()](https://github.com/sejin0000/NP-14/blob/79ebeacfed23a3de591d273c01396b0c271cd740/Assets/Script/Dohyun/AudioLibrary.cs#L71C17-L71C30)|
|PlayClip|모든 플레이어에게 사운드를 출력한다.|AudioLibrary.cs|[PlayClip()](https://github.com/sejin0000/NP-14/blob/79ebeacfed23a3de591d273c01396b0c271cd740/Assets/Script/Dohyun/AudioLibrary.cs#L96C12-L96C12)|
|AudioManagerTest|지정된 배경음/효과음을 타 객체의 이벤트에 등록한다.|AudioManagerTest.cs||
|PlayStageBGM|지정된 배경음/효과음을 타 객체의 이벤트에 등록한다.|AudioManagerTest.cs|[PlayStageBGM()](https://github.com/sejin0000/NP-14/blob/79ebeacfed23a3de591d273c01396b0c271cd740/Assets/Script/Dohyun/AudioManagerTest.cs#L32C24-L32C24)|
|BGMPlayer|배경음을 플레이하는 오브젝트를 담아놓는 부모 오브젝트이다.|GameObject||
|SEPlayer|효과음을 플레이하는 오브젝트를 담아놓은 부모 오브젝트이다.|GameObject||

<br>

##### 3-11. portal 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|portal||||
|||||

<br>

##### 3-12. MainGameNetwork 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|플레이어 퇴장시 실행되는 이벤트|5초간 로딩 패널을 노출하고, LobbyScene으로 이동시킨다.|MainGameNetwork.cs|[OnPlayerLeftRoom()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/MainGameScene/Network/MainGameNetwork.cs#L12)|
|||||

<br>

### C. Enemy

##### 1. 설명

일반 몬스터 및 보스 몬스터의 Behaviour Tree, Navmesh Agent, 패턴 메서드, 동기화, 정보SO 를 관리한다.

##### 2. 상세 설명

####
|기능 이름|기능 설명|스크립트|
|:---:|:---:|:---:|
|EnemyAI|일반 몬스터의 행동과 스프라이트를 일괄적으로 관리한다.|[EnemyAI.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Enemy_States/EnemyAI.cs#L21)|
|BossAI_Dragon|보스 몬스터인 용의 행동을 관리한다.|[BossAI_Dragon.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Dragon/BossAI_Dragon.cs#L32)|
|BossAI_Turtle|보스 몬스터인 거북이의 행동을 관리한다.|[BossAI_Turtle.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Turtle/BossAI_Turtle.cs#L14C14-L14C27)|


##### 3. 스크립트 별 설명

##### 3-1. EnemyAI.cs

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|몬스터 초기화(모든 Enemy에 적용)|Navmesh Agent를 호스트만 사용하도록 제한한다. 또한 모든 플레이어의 정보를 가져온다.|[EnemyAI.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Enemy_States/EnemyAI.cs#L21)|[Awake()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Enemy_States/EnemyAI.cs#L164)|
|몬스터 동기화(모든 Enemy에 적용)|게스트는 호스트가 보내주는 위치 정보를 통해 보간 작업만을 업데이트 한다. 몬스터의 모든 행동 실행은 호스트만 진행하도록 제한 한다.|[EnemyAI.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Enemy_States/EnemyAI.cs#L21)|[Update()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Enemy_States/EnemyAI.cs#L194)|
|시야각|지정된 각도 내에 플레이어가 들어온다면 상태 bool값 전환과 타겟 변경을 진행한다.|[EnemyAI.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Enemy_States/EnemyAI.cs#L21)|[FindPlayer()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Enemy_States/EnemyAI.cs#L552)|
|일반 몬스터BT|일반 몬스터의 행동 트리를 생성한다. 첫 트리이며 2개의 계층으로 구성되어 있다.|[EnemyAI.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Enemy_States/EnemyAI.cs#L21)|[CreateTreeAIState()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Enemy_States/EnemyAI.cs#L738C10-L738C29)|

<br>

##### 3-2. BossAI_Dragon.cs

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|브레스 패턴|용의 패턴인 브레스를 시작하고, 플레이어의 피격 여부를 판단한다.|[BossAI_Dragon.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Dragon/BossAI_Dragon.cs#L32)|[StartBreath()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Dragon/BossAI_Dragon.cs#L379) & [UpdateBreath()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Dragon/BossAI_Dragon.cs#L407C17-L407C29)|
|보스 몬스터(용) BT|보스 몬스터인 용의 행동 트리를 생성한다. 두 번째 트리이며 3개의 계층으로 구성되어 있다.|[BossAI_Dragon.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Dragon/BossAI_Dragon.cs#L32)|[CreateTreeAIState()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Dragon/BossAI_Dragon.cs#L875C10-L875C27)|

<br>

##### 3-3. BossAI_Turtle.cs 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|구르기 패턴|거북이의 패턴인 구르기를 시작한다.|[BossAI_Turtle.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Turtle/BossAI_Turtle.cs#L14C14-L14C27)|[RollStart()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Turtle/BossAI_Turtle.cs#L719) & [OnCollisionEnter2D()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Turtle/BossAI_Turtle.cs#L818C18-L818C36) & [Update()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Turtle/BossAI_Turtle.cs#L181)|
|보스 몬스터(거북이) BT|보스 몬스터인 거북이의 행동 트리를 생성한다. 세 번째 트리이며 3개의 계층으로 구성되어 있으며 BossAI_Dragon의 트리보다 더 완성된 구조의 행동 트리이다.|[BossAI_Turtle.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Turtle/BossAI_Turtle.cs#L14C14-L14C27)|[CreateTreeAIState()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/BTScript/BT_Boss_Turtle/BossAI_Turtle.cs#L520)|

<br>

## 🕶 팀원 소개
<a href="https://github.com/sejin0000/NP-14/graphs/contributors">
<img src = "https://contrib.rocks/image?repo=sejin0000/NP-14">
</a>
