<Query Kind="Statements" />

// 로그 읽기
string file = @"D:\com2usRepos\MyDoc\Working\LinqPad\iis.log";
string[] lines = File.ReadAllLines(file);
lines
.Where(str => !str.StartsWith("#"))  
.Dump();


// 20일 로그에서 응답이 20 밀리 초 이상 걸린 요청를 추출
string file = @"D:\com2usRepos\MyDoc\Working\LinqPad\iis.log";
string[] lines = File.ReadAllLines(file);
lines
.Where(str => !str.StartsWith("#"))
.Where(str =>
{
	string[] fields = str.Split(' ');
	DateTime dateTime = DateTime.Parse($"{fields[0]} {fields[1]}");
	int timeTaken = Int32.Parse(fields[14]);
	return dateTime.Day == 20 && timeTaken >= 20;
})
.Dump();


// 특정 열만을 추출해서 파일에 쓰기
string file = @"D:\com2usRepos\MyDoc\Working\LinqPad\iis.log";
string[] lines = File.ReadAllLines(file);
string[] ipaddresses = lines
					   .Where(str => !str.StartsWith("#"))
					   .Select(str =>
					   {
						   string[] fields = str.Split(' ');
						   return fields[8]; // c-ip
						   })
					   .ToArray();
string outfile = @"D:\com2usRepos\MyDoc\Working\LinqPad\cip.log";
File.WriteAllLines(outfile, ipaddresses);