<Query Kind="Statements" />

var redisDB = new RedisDB("test", "192.168.0.10");

var result1 =MyExtensions.RedisSet<string>(redisDB, "test1", "32452", null);
result1.Result.Dump();

var result2 = MyExtensions.RedisGet<string>(redisDB, "test1");
result2.Result.Dump();

var user = new GameUser()
{
	Level = 12,	Money = 34567
};
var result3 =MyExtensions.RedisSet<GameUser>(redisDB, "test1", user, null);
result3.Result.Dump();