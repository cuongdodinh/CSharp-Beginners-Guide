# C# 초보자를 위한 학습 가이드
C# 실습을 할 때는 LinqPad 사용을 추천한다.  [LinqPad 사용 및 예제](/LinqPad)    
  
  
  
## 문법 공부

- 닷넷 병렬 프로그래밍의 모든 것.pptx
  
  
## 추천 라이브러리
- RandomStringGenerator4DotNet
- FluentScheduler
- NLog
- SimpleMsgPack.Net
    - golang 서버와 msgpack 포맷으로 패킷을 주고 받을 수 있다.
    - 사용 예: [서버](https://github.com/jacking75/golang_socketGameServer_codelab/tree/master/chatServer_msgpack) | [클라이언트](https://github.com/jacking75/golang_socketGameServer_codelab/tree/master/csharp_test_client_msgpack)
- [AsyncCollections](/AsyncCollections) 
    - async-await 기반에서 사용할 수 있는 컬렉션 라이브러리
    - AsyncQueue, AsyncStack, AsyncCollection, AsyncBoundedPriorityQueue, AsyncBatchQueue
  

  
## 실습
- WinForm으로 socket을 사용하는 테스트 클라이언트 만들기
    - 간단한 GUI 프로그래밍을 배우는 것이 목적이다.
    - \Clients\csharp_simple_test_client 와 같은 프로그램을 만들어 본다.

## ASP.NET Web API
  

  
## 실시간 서버 프로그래밍
- [C# 네트워크 프로그래밍 정리](\SocketProgramming\README.md)
    - [원본 출처](https://github.com/jacking75/com2usStudy_CSharpNetworkProgramming/tree/hellowoori/_Study )
- 닷넷 네트워크 프로그래밍.pptx
- [서버 프로그래밍 실습](/Server-Tutorial)    
- [더미 클라이언트 라이브러리](https://github.com/jacking75/CSharpTcpNetworkDummy )    

### 실습
- https://github.com/jacking75/SuperSocketLite
- [FreeNetLite 분석 후 클론 개발](https://github.com/jacking75/edu_csharp_FreeNetLite )
    - FreeNetLite 라이브러리를 분석하고, 이 라이브러리의 아이디어를 토대로 본인만의 라이브러리를 만든다
    - 채팅 서버 개발하기
    - 온라인 게임 서버 개발하기
#### 분석
- [분산 서버 구조의 오목 온라인 게임]()
- C# 오픈 소스 라이브러리 분석: 다양한 오픈 소스들 코드를 분석하면서 네트워크 프로그래밍 기법을 배운다.
    - [edu_csharp_OpenSourceTCPNetworkLib](https://github.com/jacking75/edu_csharp_OpenSourceTCPNetworkLib )
    - edu_csharp_CowboyNetLite
    - edu_csharp_FastSocketLite
       
  
  
## SignalR
- WebSocket 기반
- RPC로 호출
- MesagePack 포맷 사용
- 다양한 언어 지원
- [소개 문서 및 샘플 코드](https://github.com/jacking75/study_signalR )