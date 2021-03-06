### 목적
- 실시간 랭킹을 집계하고 싶을 때 사용한다.
- 지정한 순위, 범위에 있는 유저를 알고 싶을 때
- 특정 유저의 순위를 알고 싶을 때
- 이 라이브러리는 'Simple Massive Realtime Ranking Server' 프로젝트에서 정렬 부분만 가져왔다.
    - 원 프로젝트는 https://github.com/soywiz/smrr-server 이다.


### 기능
- key는 유저의 unique 번호(정수), value는 점수(Int64)로 유저의 정보가 추가되거나 업데이트 되면 오림, 내림으로 실시간으로 정렬한다.
- 주의
    - 넣은 요소를 삭제할 수는 없다. -_-;

  
### 사용

```
class PerformanceTest
{
    var RankingMgr = new C3SockNetUtil.RtRanking.Manager();

    public void 유저_추가하면서_정렬(int userCount)
    {
        Console.WriteLine("랭킹 성능 테스트 - 더미 데이터 생성 --->");
        
        var stopWatchWork = new System.Diagnostics.Stopwatch();
        stopWatchWork.Start();


        var random = new Random();

        var rankId= RankingMgr.AddOrGetRankingIdByName(true, "test1");

        for (int i = 0; i < userCount; ++i)
        {
            var userScore = new C3SockNetUtil.RtRankingLib.UserScoreInfo()
            {
                UserId = i + 1,
                ScoreValue = random.Next(1, 2000000000),
            };

            RankingMgr.AddOrUpdateUserScore(rankId, userScore);
        }

        stopWatchWork.Stop();
        Console.WriteLine("랭킹 성능 테스트 - 더미 데이터 생성 완료. 데이터 개수: {0}, 걸린 시간: {1}", userCount, stopWatchWork.Elapsed.TotalSeconds);

        

        Console.WriteLine("랭킹 성능 테스트 - 정렬 검증 --->");

        var rankingList = RankingMgr.GetUserRankingList(rankId, 0, userCount);

        for (int i = 1; i < userCount; i++)
        {
            if (rankingList[i - 0].ScoreValue < rankingList[i].ScoreValue)
            {
                Console.WriteLine("랭킹 성능 테스트 - 정렬 실패. index: {0}, {1}", i - 1, i);
                return;
            }
        }

        Console.WriteLine("랭킹 성능 테스트 - 정렬 검증 완료");



        Console.WriteLine("랭킹 성능 테스트 - 임의의 유저 업데이트 후 정렬 --->");

        for (int i = 0; i < 100; ++i)
        {
            var userScore1 = new C3SockNetUtil.RtRankingLib.UserScoreInfo()
            {
                UserId = random.Next(1, userCount),
                ScoreTimeStamp = (uint)DateTime.Now.Ticks,
                ScoreValue = random.Next(1, 2000000000),
            };

            var stopWatchWork1 = new System.Diagnostics.Stopwatch();
            stopWatchWork1.Start();

            RankingMgr.AddOrUpdateUserScore(rankId, userScore1);

            stopWatchWork1.Stop();


            var newData = RankingMgr.GetUserRanking(rankId, userScore1.UserId);
            if (newData.UserId != userScore1.UserId || newData.ScoreValue != userScore1.ScoreValue)
            {
                Console.WriteLine(string.Format("C3SockNetUtil.RtRankingLib 랭킹 성능 테스트 - 임의의 유저 업데이트 후 정렬. 데이터 변경이 적용되지 않았음"));
                return;
            }
        }

        for (int i = 0; i < 100; ++i)
        {
            var userScore1 = new C3SockNetUtil.RtRankingLib.UserScoreInfo()
            {
                UserId = random.Next(userCount, userCount + userCount),
                ScoreTimeStamp = (uint)DateTime.Now.Ticks,
                ScoreValue = random.Next(1, 2000000000),
            };

            var stopWatchWork1 = new System.Diagnostics.Stopwatch();
            stopWatchWork1.Start();

            RankingMgr.AddOrUpdateUserScore(rankId, userScore1);

            stopWatchWork1.Stop();


            var newData = RankingMgr.GetUserRanking(rankId, userScore1.UserId);
            if (newData.UserId != userScore1.UserId || newData.ScoreValue != userScore1.ScoreValue)
            {
                Console.WriteLine(string.Format("C3SockNetUtil.RtRankingLib 랭킹 성능 테스트 - 임의의 유저 추가 후 정렬. 데이터 변경이 적용되지 않았음"));
                return;
            }
        }


        Console.WriteLine("랭킹 성능 테스트 임의의 유저 업데이트 후 정렬- 정렬 검증 --->");

        rankingList = RankingMgr.GetUserRankingList(rankId, 0, userCount);

        for (int i = 1; i < userCount; i++)
        {
            if (rankingList[i - 0].ScoreValue < rankingList[i].ScoreValue)
            {
                Console.WriteLine("랭킹 성능 테스트 임의의 유저 업데이트 후 정렬- 정렬 실패. index: {0}, {1}", i - 1, i);
                return;
            }
        }

        Console.WriteLine("랭킹 성능 테스트 임의의 유저 업데이트 후 정렬 - 정렬 검증 완료");
    }
}
```


### 성능 테스트
- 하드웨어 사양: i7-2600(3.4GHz), 16기가 RAM
- 시간 단위는 초

| Collection | 100,000 | 1,000,000 | 5,000,000 | 1,000,000 에서 요소 1개 변경 |
|:-----------|------------:|:------------:|:------------:|:------------:|
| List|  0.03 |  0.48|  3.24|  0.35 ~ 0.04|
| ParallelQuickSort|  0.01|  0.23 ~0.24|  1.4 ~ 1.5|  0.21 ~ 0.27|
| RtRankingLib |  0.51|  7.1|  45|  0.0002 이하|

- 요소가 추가(혹은 수정)할 때 정렬하므로 한번에 대량의 데이터를 넣을 때는 느림. 그러나 요소별 갱신은 꽤 빠름

