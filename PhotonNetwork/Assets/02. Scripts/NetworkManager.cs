using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;       //포톤 네트워크 핵심기능
using Photon.Realtime;  //포톤 서비스 관련

public class NetworkManager : Photon.Pun.MonoBehaviourPunCallbacks
{
    public Text infoText;   //네트워크 상태를 보여줄 텍스트
    public Button connectButton;    //룸접속 버튼
    string gameVersion = "1";

    private void Awake()
    {
        Screen.SetResolution(800, 600, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //접속에 필요한 정보(게임버전) 설정
        PhotonNetwork.GameVersion = gameVersion;
        //마스터 서버에 접속하는 함수 (이 부분이 제일 중요)
        PhotonNetwork.ConnectUsingSettings();
        //접속 시도중임을 텍스트로 표시하기
        infoText.text = "마스터 서버에 접속중...";
        //룸(게임공간) 접속 비활성화
        connectButton.GetComponent<Button>().interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        //접속 시도중임을 텍스트로 표시하기
        infoText.text = "온라인 : 마스터 서버와 연결됨.";
        //룸(게임공간) 접속 비활성화
        connectButton.GetComponent<Button>().interactable = true;
    }

    //혹시 시작하면서 마스터 서버에 접속에 실패했을시 자동으로 실행된다.
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
        //접속 시도중임을 텍스트로 표시하기
        infoText.text = "마스터 서버에 접속중...";
        //룸(게임공간) 접속 비활성화
        connectButton.GetComponent<Button>().interactable = false;
    }

    //접속 버튼 클릭시 이 함수 발동하기
    public void OnConnect()
    {
        connectButton.GetComponent<Button>().interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            //룸(게임세상)으로 바로 접속 실행
            infoText.text = "랜덤방에 접속...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            //접속 시도중임을 텍스트로 표시하기
            infoText.text = "마스터 서버에 접속중...";
        }
    }

    public override void OnJoinedRoom()
    {
        //접속 정보 표시하기
        infoText.text = "방 참가 성공";
        //모든 룸 참가자들이 "GameScene"을 로드함
        PhotonNetwork.LoadLevel("GameScene");
    }


    //(빈 방 없이)랜덤 룸 참가에 실패한 경우 자동실행
    public override void OnJoinRandomFailed(short returnCode, string messages)
    {
        //접속 정보 표시하기
        infoText.text = "빈 방이 없으니 새로운 방생성중...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
}
