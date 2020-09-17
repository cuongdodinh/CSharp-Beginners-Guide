# C# 초보자를 위한 학습 가이드
- .NET Core를 설치하면 Windows, Linux, OS X에서 C#을 사용할 수 있다.  
- C# 실습을 할 때는 LinqPad 사용을 추천한다.  [LinqPad 사용 및 예제](/LinqPad)    
- [공식 .NET 설명서](https://docs.microsoft.com/ko-kr/dotnet/) 
- 추천 코딩 규칙[coding_rule.md]  
   

## IDE 설치
[Visual Studio](https://visualstudio.microsoft.com/ko/ ) 혹은 [JetBrains Rider](https://www.jetbrains.com/ko-kr/rider) IDE를 사용한다.
- Visual Studio는 설치할 때 닷넷 SDK까지 다 설치한다.
- JetBrains Rider를 사용하는 경우 아래 순서대로 설치하는 것을 추천.  
    - 닷넷 프레임워크 4.8 설치 [link](https://dotnet.microsoft.com/download/dotnet-framework)  설치 후 재 부팅
    - 닷넷 코어 3.1 설치  [link](https://dotnet.microsoft.com/download/dotnet-core)  설치 후 재 부팅
    - JetBrains Rider 설치)


## 문법 공부
- 빠르게 배우고 싶다면 [예제로 배우는 C# 프로그래밍](http://www.csharpstudy.com/) 사이트의 글을 본다. 아래 항목은 필수로 본다.  
    - [문법](http://www.csharpstudy.com/CSharp/CSharp-intro.aspx) 
    - [6.0 버전 이후의 새 기능](http://www.csharpstudy.com/Latest/CS-new-features.aspx) 
    - [자료구조](http://www.csharpstudy.com/DS/array.aspx) 
    - [멀티쓰레딩](http://www.csharpstudy.com/Threads/thread.aspx)
- 체계적으로 공부하기 위해 책을 본다.
    - [시작하세요! C# 8.0 프로그래밍 기본 문법부터 실전 예제까지](http://www.yes24.com/Product/Goods/82590356)
    - [C# 6.0 완벽 가이드](http://www.yes24.com/Product/Goods/33085047?OzSrank=1)
- 중고급 이상 레벨이 되고 싶다면 아래 책을 본다.
    - [제프리 리처의 CLR via C#](http://www.yes24.com/Product/Goods/15169403) 
    - [고성능 .NET 코드 프로그래밍](http://www.yes24.com/Product/Goods/24020688)
- 닷넷 병렬 프로그래밍의 모든 것.pptx
    
- 실습: WinForm으로 socket을 사용하는 테스트 클라이언트 만들기    
    - 간단한 GUI 프로그래밍을 배우는 것이 목적이다.
    - WinForm 프로그래밍은 [예제로 배우는 C# 프로그래밍 - Winform](http://www.csharpstudy.com/WinForms/WinForms-Intro.aspx)을 참고한다.
    - \Clients\csharp_simple_test_client 와 같은 프로그램을 만들어 본다.
 
  
   
## 추천 라이브러리
- [BenchmarkDotNet 라이브러리 소개](https://www.sysnet.pe.kr/2/0/11547 ) : 코드 성능 테스트를 할 수 있다.
- [RandomStringGenerator4DotNet](/RandomStringGenerator4DotNet) : 랜덤한 문자열을 생성한다. 임시 인증 키 생성에 사용하면 유용하다
- [Pseudo-Random Numbers](http://numerics.mathdotnet.com/Random.html ) :  랜덤한 숫자를 생성한다. 임시 인증 키 생성에 사용하면 유용하다
- [Random Number Generators](http://www.csharpcity.com/reusable-code/random-number-generators/ ) :  랜덤한 숫자를 생성한다. 임시 인증 키 생성에 사용하면 유용하다
- [FluentScheduler](/FluentScheduler) : 서버에서 주기적으로 job을 실행하고 싶을 때 사용하면 유용하다.
- [SimpleMsgPack.Net](/SimpleMsgPack.Net)
    - golang 서버와 msgpack 포맷으로 패킷을 주고 받을 수 있다.
    - 사용 예: [서버](https://github.com/jacking75/golang_socketGameServer_codelab/tree/master/chatServer_msgpack) | [클라이언트](https://github.com/jacking75/golang_socketGameServer_codelab/tree/master/csharp_test_client_msgpack)
- [AsyncCollections](/AsyncCollections) 
    - async-await 기반에서 사용할 수 있는 컬렉션 라이브러리
    - AsyncQueue, AsyncStack, AsyncCollection, AsyncBoundedPriorityQueue, AsyncBatchQueue
- [ZString - Unity / .NET Core의 제로 할당 C # 문자열 생성](https://docs.google.com/document/d/1qV_34N90u3ZPv82w4aj5xSodQ5ON-LFb5Z_D3Kuw0Ic/edit?usp=sharing )
- [고 성능 Json 라이브러리 Utf8Json](https://github.com/neuecc/Utf8Json )  
- [(일어)MasterMemory – Unity와 .NET Core용 읽기 전용 인메모리 데이터베이스](http://tech.cygames.co.jp/archives/3269/ )
  
   
### Logger
- NLog
    - [Fluentd 확장](/NLog/NLog.Targets.Fluentd) | [사용법](https://jacking75.github.io/csharp_nlog_fluentd/ )
    - [기존의 설정을 사용하면서 파일 패스만 동적으로 변경하기](https://jacking75.github.io/csharp_nlog_dynamic_file_path/) 
    - [동적으로 로그 파일 이름 설정하기](https://jacking75.github.io/csharp_nlog_dynamic_log_name/) 
    - [(일어)ASP.NET Core에서 System.Diagnostic.Trace를 NLog에 흘리는 방법](https://qiita.com/ShikaTech/items/0d5c5a0272d0d640bcb3 ) 
    - [(일어)NLog에서 DotNetZip을 조합시켜서 아카이브를 압축한다](https://qiita.com/takiru/items/c8164e84563fea1c701c ) 
    - [ASP.NET Core에 NLog 사용하기](https://docs.google.com/document/d/1Ea8qqRjLdI5aYVSV_Lxu4YhbDAPQ5Rc0jAoqY8ipsTM/edit?usp=sharing)
- ZLogger: [.NET Core와 Unity를위한 제로 할당 구조 로거](https://docs.google.com/document/d/13lQUpoJjHGTqiLASdv21lmMGfGM6iZXlQdsOBzS3yKg/edit?usp=sharing)
    
### DB
- [Dapper.NET](https://github.com/StackExchange/Dapper) : [dapper.net을 사용하여 sql 쿼리](https://gist.github.com/jacking75/21ec0c29bda2af62be9985f628125e00 ) | [(2013년)MySQL용 C# 라이브러리와 Dapper.NET](https://jacking.tistory.com/1117)
- [MySQL 라이브러리](https://github.com/mysql-net/MySqlConnector) | [문서](https://mysqlconnector.net/)  
- Redis: [CloudStructures](https://github.com/neuecc/CloudStructures) : StackExchange.Redis를 사용하기 더 쉽게 랩핑한 라이브러리   
  
  
  
## ASP.NET Core Web API
- [MS Docs: ASP.NET Core로 Web API 만들기](https://docs.microsoft.com/ko-kr/aspnet/core/web-api/?view=aspnetcore-3.1)  
- [MS Docs: 자습서: ASP.NET Core를 사용하여 웹 API 만들기](https://docs.microsoft.com/ko-kr/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio )

  
## 소켓 서버 프로그래밍
- [C# 네트워크 프로그래밍 정리](\SocketProgramming\README.md)    [원본 출처](https://github.com/jacking75/com2usStudy_CSharpNetworkProgramming/tree/hellowoori/_Study )
- 닷넷 네트워크 프로그래밍.pptx
- [서버 프로그래밍 실습](/Server-Tutorial)    
- [더미 클라이언트 라이브러리](https://github.com/jacking75/CSharpTcpNetworkDummy ) 
    - async-await 기반으로 스레드 세이프하기 복수의 더미를 동작 시킨다.
    - [dotnet-script](https://github.com/filipw/dotnet-script )를 사용하여 더미 시나리오를 스크립트로 구현할 수 있다. [dotnet-script 사용 방법](https://docs.google.com/document/d/1JdiISidYRAWnzBxHQBiVZlGTLTvtwR5DsnSt7p-whbI/edit?usp=sharing)
  

### 실습
- [FreeNetLite 분석 후 클론 개발](https://github.com/jacking75/edu_csharp_FreeNetLite )
    - FreeNetLite 라이브러리를 분석하고, 이 라이브러리의 아이디어를 토대로 본인만의 라이브러리를 만든다
    - 채팅 서버 개발하기
    - 온라인 게임 서버 개발하기
- [SuperSocketLite](https://github.com/jacking75/SuperSocketLite) 
    - [오픈소스 네트워크 엔진 SuperSocket 사용하기](https://github.com/jacking75/SuperSocketLite/blob/master/Docs/%EC%98%A4%ED%94%88%EC%86%8C%EC%8A%A4%20%EB%84%A4%ED%8A%B8%EC%9B%8C%ED%81%AC%20%EC%97%94%EC%A7%84%20SuperSocket%20%EC%82%AC%EC%9A%A9%ED%95%98%EA%B8%B0.pdf) 보기
    - [Tutorial](https://github.com/jacking75/SuperSocketLite/tree/master/Tutorials)에 있는 것을 단계 별로 진행한다
    - 온라인 오목 게임을 만든다


### 분석
- C# 오픈 소스 라이브러리 분석: 다양한 오픈 소스들 코드를 분석하면서 네트워크 프로그래밍 기법을 배운다.
    - [edu_csharp_OpenSourceTCPNetworkLib](https://github.com/jacking75/edu_csharp_OpenSourceTCPNetworkLib )
    - [edu_csharp_CowboyNetLite](/edu_csharp_CowboyNetLite)
    - [edu_csharp_FastSocketLite](/edu_csharp_FastSocketLite)
- [분산 서버 구조의 오목 온라인 게임]()       
  
  

## SignalR
- WebSocket 기반
- RPC로 호출
- MesagePack 포맷 사용
- 다양한 언어 지원
- [소개 문서 및 샘플 코드](https://github.com/jacking75/study_signalR )