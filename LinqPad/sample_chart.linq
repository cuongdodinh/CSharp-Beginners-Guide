<Query Kind="Statements" />

var data = new[] {
	new { Subject = "국어", Point = 95 },
	new { Subject = "수학", Point = 65 },
	new { Subject = "물리", Point = 85 },
	new { Subject = "사회", Point = 85 },
	new { Subject = "영어", Point = 70 }
};

data.Chart(d => d.Subject, d => d.Point, LINQPad.Util.SeriesType.Column).Dump();