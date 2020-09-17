<Query Kind="Program" />

// 출처: https://qiita.com/kazuhisam3/items/293103991a9f0bb8dcb7
void Main()
{
	"".Dump("표시 관련");

	"".Dump("Progressbar 표시");
	var bar = new Util.ProgressBar("Progressbar 표시");
	bar.Dump();
	foreach (var element in Enumerable.Range(1, 100))
	{
		bar.Percent = element;
		System.Threading.Thread.Sleep(50);
	};
	// Progressbar 표시 부분만 실행할 수 잇도록 「;」를 넣었다


	Util.Metatext("Metatext").Dump("Metatext 표시를（녹색＋이태릭）으로 한다");

	Util.Highlight(new { a = 1, b = 2, c = 3 }).Dump("Highlight Dump 메소드에서의 출력 결과를 하이라이트 표시");

	var a = new DummyData() { Name = "1", Id = 1 };
	var b = new DummyData() { Name = "2", Id = 2 };
	Util.VerticalRun(Util.Highlight(a), b).Dump("VerticalRun 복수의 데이터를 수직 방향으로 모아서 Dump 한다");
	Util.VerticalRun(Util.Highlight("abc"), "abc").Dump();

	Util.WordRun(true, Util.Highlight(a), b).Dump("WordRun 복수의 데이터를 수직 방향으로 모아서 Dump 한다. 첫번째 인수는 표시 영역이 되지 않은 경우는 꺽을지의 플러그.(클래스의 경우는 횡으로는 되지 않는다, Highlight 효과 없음）");
	Util.WordRun(true, Util.Highlight("abc"), "abc").Dump();
	Util.WordRun(false, Enumerable.Range(0, 100)).Dump();

	Util.RawHtml(Util.ToHtmlString(
			Enumerable.Range(0, 10).Select(e => new { e, a = e * e, b = e * 2 })
		)).Dump("RawHtml html 형식으로 표시(ToHtmlString과 조합하여 사용해 보기)");


	"".Dump("문자열 정형");
	Util.ToCsvString
		(
			Enumerable.Range(0, 10).Select(e => new { e, a = e * e, b = e * 2 }),
			new string[] { "e", "a" }
		).Dump("ToCsvString 건네진 오브젝트를 csv 구분 문자열로 돌려준다. 두번째 인수로 출력하는 열 선택 가능");
	"".Dump("WriteCsv 파일에 CSV 형식으로 쓴다. ToCsvString에 쓸 곳의 파일 패스를 더한 것 같다");
	//Util.WriteCsv

	Util.ToHtmlString(Enumerable.Range(0, 10).Select(e => new { e, a = e * e, b = e * 2 })).Dump();


	"".Dump("LINQPad 자체의 정보 취득");
	Util.GetWebProxy().Dump();
	Util.GetMyQueries().Dump("GetMyQueries My Queries 폴더에 있는 LINQ 파일의 리스트 표시");
	//Util.GetSamples().Dump("GetSamples Samples 폴더에 있는 LINQ 파일 리스트를 표시");
	//Util.GetSamplesFolder().Dump("GetSamplesFolder Smaple 파일의 저장소 폴더의 패스를 얻는다");
	Util.CurrentQueryPath.Dump("CurrentQueryPath 현재 파일의 패스를 반환한다. 아직 저장하지 않은 경우는 null을 돌려준다");

	"".Dump("기타");
	var result = Util.Cmd("ping localhost").Dump("Cmd dos 명령어를 실행하는 것이 가능. 실행 결과를 반환 값으로 받는 것도 가능하다");

	Util.CurrentDataContext.Dump("CurrentDataContext 현재의 DataContext를 취득 가능. 오른쪽 위의 Connection 미 선택 시는 null을 반환한다)");

	var sw = Stopwatch.StartNew();
	Util.OnDemand("클릭하면 실행한다", () => sw)
		.Dump("OnDemand 클릭 되었을 때 두번째 인수에서 지정된 Func을 실행하고, Dump 한다.");


	"".Dump("Run 지정한 linq 파일을 실행한다.반환 값 형식은 두번째 인수의 format으로 지정. 실행한 linq 파일 내에서 dump 한 내용을 반환한다");
	//var run = Util.Run(@"", QueryResultFormat.Html);
	//run.Dump();

	"".Dump("ReadLine Console.ReadLine()에 타이틀을 설정할 수 있다");
	//Util.ReadLine("Console.ReadLine()에 타이틀을 설정할 수 있다").Dump("ReadLine Console.ReadLine()에 타이틀을 설정할 수 있다");
	//Console.ReadLine().Dump("단순한 Console.ReadLine은 Title 타이틀 설정할 수 없다");

	"".Dump("DisplayWebPage Web 페이지 표시 가능");
	//Util.DisplayWebPage("http://google.com", "google");
}

// Define other methods and classes here
public class DummyData
{
	public int Id { get; set; }
	public string Name { get; set; }
	public int Value { get; set; }
}