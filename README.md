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
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|LobbyManager|플레이어가 어떤 패널에 있는 지 (어떤 상태인지) 정의하고, 관련된 상태에 대한 오브젝트들을 호출한다|[LobbyManager.cs](https://github.com/sejin0000/NP-14/blob/74c9fd910eb7af5f3fecb5874af04b7550233f83/Assets/Script/Lobby/LobbyManager.cs#L29-L31)||
|SetPanel|특정 패널을 보여주고, 특정 패널의 상태임을 저장한다.|LobbyManager.cs|[SetPanel()](https://github.com/sejin0000/NP-14/blob/b5465df3a705bf84b3ad18c4c8ea192d84f6eb40/Assets/Script/Lobby/LobbyManager.cs#L206)|
|NetworkManager|플레이어의 네트워크 상태를 통해 MonoBehaviorPuncallbacks를 상속받아, 특정 상태에서의 이벤트를 관리한다.|[NetworkManager.cs](https://github.com/sejin0000/NP-14/blob/b5465df3a705bf84b3ad18c4c8ea192d84f6eb40/Assets/Script/Lobby/NetworkManager.cs#L13)||
|로비 입장시 이벤트|플레이어가 로비에 입장했을 때, 실행되어야 하는 코드|NetworkManager.cs|[OnJoinedLobby()](https://github.com/sejin0000/NP-14/blob/b5465df3a705bf84b3ad18c4c8ea192d84f6eb40/Assets/Script/Lobby/NetworkManager.cs#L58)|

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
|UIManager|||
|UI_Root|||
|ParticleManager|||
|portal|||
|MainGameNetwork|||
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
|UIManager||||
|||||

<br>

##### 3-10. UI_Root 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|UI_Root||||
|||||

<br>

##### 3-11. ParticleManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|ParticleManager||||
|||||

<br>

##### 3-12. ParticleManager 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|ParticleManager||||
|||||

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
|MainGameNetwork||||
|||||

<br>

##### 3-15. MinimapCamera 

####
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|MinimapCamera||||
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

