# NP-14
<br>

## 목차
| [목차](#목차) |
| :---: |
|[🕶 게임 설명](#-게임-설명)|
|[🕶 기본 조작법](#-기본-조작법)|
|[🕶 트레일러](#-트레일러)|
|[🕶 기능 설명](#-기능-설명)|
|[🕶 팀원 소개](#-팀원-소개)|


## 🕶 게임 설명
로그라이크 탑 다운 슈팅 게임으로 1인에서 부터 최대 3인이 함께 즐길 수 있는 로그라이크 형식의 슈팅 게임입니다.

<br>

![표지](https://github.com/sejin0000/NP-14/assets/129154514/de293708-c6da-4bd2-a27e-01807eecd05d)

<br>

### 1. 닉네임 입력

닉네임을 입력하여 로비에 입장하세요

(첨에 닉네임 치고 입장하는 장면)

메인로비 - 설정에서 추가적으로 닉네임을 변경할 수 있습니다.

(설정에서 닉네임 바꾸는 장면)

### 2. 룸 입장

메인로비 - 빠른 시작으로 랜덤한 파티원과 게임 플레이가 가능합니다.

(로비에서 빠른시작으로 시작하는 장면)

혹은, 메인로비 - 방 찾기로 특정한 룸에서 게임 플레이가 가능합니다.

(대충 방 만들고 방 들어가는 장면)


### 3. 게임 플레이

좌클릭으로 몬스터에게 총을 쏠 수 있습니다.

(좌클릭하는 장면)

우클릭으로 스킬을 사용할 수 있습니다.
직업별 스킬은 다음과 같습니다.

1. 라이플 : 일정 시간동안 공격속도 및 이동속도를 증가시킵니다.
2. 샷건 : 일정 시간동안 자신 주위의 자신과 팀원을 지켜주는 쉴드를 생성합니다.
3. 스나이퍼 : 힐모드와 딜모드로 상시 변경 가능. 딜모드는 일반 평타, 힐 모드는 파티원에게 공격을 적중시켰을 시, 힐을 부여합니다.

(3명이 스킬 한번에 쓰는 장면)


### 4. 증강 선택

룸 클리어와 스테이지 클리어시마다, 증강을 선택하여 획득할 수 있습니다.

룸 클리어시에는 20% 확률로 사용자의 플레이스타일을 변경시킬 수 있는 증강이 등장합니다. 그 외에는 사용자를 강화시켜주는 일반 스탯 증강이 등장합니다.

(룸 클리어시 증강 화면)

스테이지 클리어 시에는 100% 확률로 사용자의 플레이스타일을 변경시킬 수 있는 증강이 등장합니다.

(스테이지 클리어시 증강 화면)

### 5. 보스

2개의 스테이지를 클리어할 때마다, 보스와 전투를 벌이는 보스 스테이지가 등장합니다.

(대충 보스와 싸우는 장면)

## 🕶 기본 조작법  
![KEY_Setting (1)](https://github.com/sejin0000/NP-14/assets/129154514/a3c91c15-39c7-4ac8-88b1-3998eaa92cf9)

<br>

## 🕶 트레일러

<br>

[![Video Label](http://img.youtube.com/vi/1Tf4Kwndvs0/0.jpg)](https://youtu.be/1Tf4Kwndvs0&ab)

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
|MonsterSpawner|||
|NavMesh2D|||
|MapGenerator|||
|Debuff|||
|MakeAugmentListManager|||
|AugmentManager|||
|ResultManager|||
|GameManager|||
|UIManager|UI_Root에서 사용하는 UI_Base 객체를 Initialize한다. 이때 이벤트 등록 등의 작업을 처리한다.||
|UI_Root|Scene에서 표시되는 Canvas 오브젝트이다.||
|ParticleManager|특정 좌표에 파티클 오브젝트를 생성한다.||
|AudioManager|특정 좌표에 파티클 오브젝트를 생성한다.||
|portal|||
|MainGameNetwork|플레이어 탈퇴 시, 로딩 패널을 노출된다.|[MainGameNetwork.cs](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/MainGameScene/Network/MainGameNetwork.cs#L7)|
|MinimapCamera|||

##### 3. 기능 별 설명

##### 3-1. MonsterSpawner 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|MonsterSpawner||||
|||||

<br>

##### 3-2. NavMesh2D

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|NavMesh2D||||
|||||

<br>

##### 3-3. MapGenerator 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|MonsterSpawner||||
|||||

<br>

##### 3-4. Debuff 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|Debuff||||
|||||

<br>

##### 3-5. MakeAugmentListManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|MakeAugmentListManager||||
|||||

<br>

##### 3-6. AugmentManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|AugmentManager||||
|||||

<br>

##### 3-7. ResultManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|ResultManager||||
|||||

<br>

##### 3-8. GameManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|GameManager||||
|||||

<br>

##### 3-9. UIManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|UIManager|해당 신에서 사용하는 UI_Base 객체를 `Initialize`한다.|||

<br>

##### 3-10. UI_Root 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|StageTransition|스테이지가 시작될 때 탑을 오르는 연출을 표시한다.|||
|PlayerHUD|플레이어의 체력/회피/스킬 게이지와 탄창을 표시한다.|||
|SetupPopupPrefab|환경설정 UI를 표시한다. |||
|GameClearPanel|게임오버, 클리어 시 해당 UI를 표시한다.|||
|LoadingPanel|다른 화면으로 이동할 때(로딩 시) 표시한다.|||
|Minimap|인 게임에서 표시되는 미니맵 관련 UI입니다.|||


<br>

##### 3-11. ParticleManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|ParticleManager|파티클 오브젝트를 특정 좌표(Vector3)에 표시한다.|||\

<br>

##### 3-12. AudioManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|AudioManager|배경음/효과음을 `Dictionary`에 등록하여 캐싱하고 출력한다.|||
|AudioLibrary|AudioManagerTest에서 출력할 효과음을 지정한다.|||
|AudioManagerTest|지정된 배경음/효과음을 타 객체의 이벤트에 등록한다.|||
|BGMPlayer|배경음을 플레이하는 오브젝트를 담아놓는 부모 오브젝트이다.|||
|SEPlayer|효과음을 플레이하는 오브젝트를 담아놓은 부모 오브젝트이다.|||

<br>

##### 3-13. portal 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|portal||||
|||||

<br>

##### 3-14. MainGameNetwork 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|플레이어 퇴장시 실행되는 이벤트|5초간 로딩 패널을 노출하고, LobbyScene으로 이동시킨다.|MainGameNetwork.cs|[OnPlayerLeftRoom()](https://github.com/sejin0000/NP-14/blob/9f03511281827b1875d50f7276dafc155f450de4/Assets/Script/MainGameScene/Network/MainGameNetwork.cs#L12)|
|||||

<br>

##### 3-15. MinimapCamera 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|MinimapCamera||||
|||||

<br>

### C. Enemy AI

##### 1. 설명

설명 본문 입력

##### 2. 상세 설명

####
|기능 이름|기능 설명|스크립트|
|:---:|:---:|:---:|
|EnemyAI|||
|Deade|||

## 🕶 팀원 소개

