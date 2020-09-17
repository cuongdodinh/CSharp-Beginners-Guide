#Snowflake.Net
https://github.com/ccollie/snowflake-net  
  
A port of  Twitter's [Snowflake](https://github.com/twitter/snowflake)  algorithm to C#.

Snowflake is a service for generating unique ID numbers at high scale.

## Hot to

```
const int workerId = 1; // Max 31
const int datacenterId = 1; // Max 31
var worker = new IdWorker(workerId, datacenterId);
var v = worker.NextId();
```

###License
Apache 2.0
