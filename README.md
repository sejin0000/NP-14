# NP-14
<br>

## 목차
| [목차](#목차) |
| :---: |
|[🕶 게임 설명](#게임-설명)|
|[🕶 기본 조작법](#기본-조작법)|
|[🕶 트레일러](#트레일러)|
|[🕶 씬 구조](#씬-구조)|


## 🕶 게임 설명
로그라이크 탑 다운 슈팅 게임으로 1인에서 부터 최대 3인이 함께 즐길 수 있는 로그라이크 형식의 슈팅 게임입니다.

<br>

![표지](https://github.com/sejin0000/NP-14/assets/129154514/de293708-c6da-4bd2-a27e-01807eecd05d)

<br>

## 🕶 기본 조작법  
![KEY_Setting (1)](https://github.com/sejin0000/NP-14/assets/129154514/a3c91c15-39c7-4ac8-88b1-3998eaa92cf9)

<br>

## 🕶 트레일러

<br>

[![Video Label](http://img.youtube.com/vi/1Tf4Kwndvs0/0.jpg)](https://youtu.be/1Tf4Kwndvs0&ab)

<br>

## 씬 구조

<br>

#### LobbyScene
|기능 이름|기능 설명|스크립트|메서드|
|:---:|:---:|:---:|:---:|
|LobbyScene|플레이어 닉네임 등록, 로비 및 룸 구성을 통해 파티원을 모집한다.|||
|LobbyManager|플레이어가 어떤 패널에 있는 지 (어떤 상태인지) 정의하고, 관련된 상태에 대한 오브젝트들을 호출한다|LobbyManager.cs|https://github.com/sejin0000/NP-14/blob/74c9fd910eb7af5f3fecb5874af04b7550233f83/Assets/Script/Lobby/LobbyManager.cs#L29|
