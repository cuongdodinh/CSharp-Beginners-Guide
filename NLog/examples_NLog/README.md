# .NET Core NLog 예제
여기 글은 본인 경험 및 인터넷 여기 저기에 있는 것을 정리한 것이다.    
  
## 설치
Nuget에서 NLog.Extensions.Logging를 설치한다.  
  
NLog.config 파일에 로그 설정을 기록한다.  
  

## examples
### 파일에 로그 출력
nlog_01  
```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

    <targets>
        <target name="logfile" xsi:type="File" fileName="file.txt" />
    </targets>

    <rules>
        <logger name="*" minlevel="Debug" writeTo="logfile" />
    </rules>
</nlog>
```
  
### 로그 레이아웃 커스텀마이즈
nlog_02  
```
<targets>
    <target name="logfile" xsi:type="File" fileName="file.txt" 
            layout="${level:uppercase=true:padding=-5} ${longdate} &quot;${message}&quot; ${callsite}#${callsite-linenumber}" />
</targets>
```  
  
### JSon 포맷으로 출력
nlog_03  
```
<targets>
    <target name="logfile" xsi:type="File" fileName="file.txt">
        <layout xsi:type="JsonLayout">
            <attribute name="level" layout="${level}" />
            <attribute name="timestamp" layout="${longdate}" />
            <attribute name="message" layout="${message}" />
            <attribute name="callsite" layout="${callsite}#${callsite-linenumber}" />
        </layout>
    </target>
</targets>
```

### 로그 출력을 복수로
nlog_04  
```
<targets>
    <target name="logfile" xsi:type="File" fileName="log.txt" />
    <target name="errorlogfile" xsi:type="File" fileName="errorlog.txt" />
</targets>
<rules>
    <logger name="*" minlevel="Debug" writeTo="logfile" />
    <logger name="*" minlevel="Error" writeTo="errorlogfile" />
</rules>
```
  
  
## ASP.NET Core에서 NLog 사용하기
Startup.cs   
```
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{ 

	// ...
    loggerFactory.AddNLog();
	
    // ...
	
	app.UseMvc();
}
```
  
Program.cs  
```
public class Program
{
	public static void Main(string[] args)
	{

		var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

		try
		{
			logger.Debug("init main");
			BuildWebHost(args).Run();
		}
		catch (Exception e)
		{
			logger.Error(e, "Stopped program because of exception");
			throw;
		}
	}
```  
  
HomeController.cs  
```
using Microsoft.Extensions.Logging;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Index page says hello");
        return View();
    }
```
    
NLog.config  
```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

    <extensions>
        <add assembly="NLog.Web.AspNetCore"/>
    </extensions>

    <targets>
        <target name="logfile" xsi:type="File" fileName="file.txt" />
    </targets>

    <rules>
        <logger name="*" minlevel="Debug" writeTo="logfile" />
    </rules>
</nlog>
```
  
  
## NLog.config 샘플
### 애플리케이션 시작마다 로그 파일 이름 다르게 하기 
NLog.config  
```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

  <!-- 
  See https://github.com/nlog/nlog/wiki/Configuration-file 
  for information on customizing logging rules and outputs.
   -->
  <targets>
    <target name="console" xsi:type="ColoredConsole" layout="${date:format=HH\:mm\:ss}| [TID:${threadid}] | ${message}" />
    <target name="InfoFile" xsi:type="File"
            fileName="${basedir}/Logs/Info_${logger}.log"
            archiveAboveSize="128000000"
            archiveFileName="${basedir}/Logs/Info_${date:format=MMddHHmm}/${shortdate}.{#####}.log"
            archiveNumbering="Sequence"
            concurrentWrites="false"
            layout="[${date}] [TID:${threadid}] [${stacktrace}]: ${message}" />
    <target name="ErrorFile" xsi:type="File"
            fileName="${basedir}/Logs/Error_${logger}.log"
            archiveAboveSize="128000000"
            archiveFileName="${basedir}/Logs/Error_${date:format=MMddHHmm}/${shortdate}.{#####}.log"
            archiveNumbering="Sequence"
            concurrentWrites="false"
            layout="[${date}] [TID:${threadid}] [${stacktrace}]: ${message}" />
  </targets>

  <rules>
    <logger name="*" minlevel="Info" maxlevel="Info" writeTo="InfoFile" />
    <logger name="*" minlevel="Error" writeTo="ErrorFile" />
    <logger name="*" minlevel="Info" writeTo="console" />
  </rules>
</nlog>
``` 
  
코드에서도 아래처럼 이름을 부여해야 하는지 기억나지 않음..아마 아래 코드가 없어도 될듯  
```
public static NLog.Logger FileLogger = NLog.LogManager.GetLogger(DateTime.Now.ToString("yyyyMMddHHmm"));
```  
    

### 파일과 콘솔(칼라 문자열) 출력

```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="c:\temp\nlog-internal.log">

  <targets>
    <target xsi:type="File" name="logfile" fileName="file.txt"
            layout="${date:format=yyyyMMddHHmmss} ${message}" />
    <target xsi:type="ColoredConsole" name="console" 
            layout="${longdate} ${uppercase:${level}} ${message}" />
  </targets>

  <rules>
    <logger name="*" minlevel="Info" writeTo="logfile" />
    <logger name="*" minlevel="Trace" writeTo="console" />
  </rules>
</nlog>
```
  
### gmail 사용

```
<target xsi:type="Mail" name="gmail"
        smtpServer="smtp.gmail.com"
        smtpPort="587"
        smtpAuthentication="Basic"
        smtpUserName="user@gmail.com"
        smtpPassword="password"
        enableSsl="true"
        from="user@gmail.com"
        to="user@gmail.com"
        cc="user@gmail.com"
        />

<rules>
    <logger name="*" minlevel="Fatal" writeTo="gmail" />        
</rules>    
```
    
### 동적으로 파일이름 설정하기
NLog.config  
```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

  <targets>
  <target
    name="LogFile"
    xsi:type="File"
    layout="${longdate} [${uppercase:${level:padding=-5}}] ${message} ${exception:format=tostring}"
    fileName="${basedir}Logs\${var:runtime}\${date:format=yyyyMMdd}.log"
    encoding="UTF-8"
    archiveFileName="${basedir}Logs\archives\${var:runtime}\archive.{#}.log"
    archiveEvery="Day"
    archiveNumbering="Rolling"
    maxArchiveFiles="7"
    header="[Start Logging]"
    footer="[End Logging]${newline}"  />
  </targets>

  <rules>
    <logger name="Log" minlevel="Trace" writeTo="LogFile" />
  </rules>
</nlog>
```

애플리케이션 코드  
```
var logger = LogManager.GetLogger("Log");
logger.Factory.Configuration.Variables.Add("runtime", "test");
logger.Factory.ReconfigExistingLoggers();
```    
  
실행 파일패스\Logs\test\yyyyMMdd.log 라는 로그가 만들어진다.  
      

### 출력 파일 나누기 
NLog.config  
```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

  <targets>
    <target
      name="fooFile"
      xsi:type="File"
      layout="${longdate} ${uppercase:${level}} ${message}"
      fileName="${basedir}/logs/${shortdate}.log"
      archiveAboveSize ="5000000"
      maxArchiveFiles="2"/>
    <target
      name="barFile"
      xsi:type="File"
      layout="${longdate} ${uppercase:${level}} ${message}"
      fileName="${basedir}/logs/${longdate}.log"
      archiveAboveSize ="5000000"
      maxArchiveFiles="2"/>
  </targets>

  <rules>
    <logger name="fooLogger" minlevel="Trace" writeTo="fooFile" />
    <logger name="barLogger" minlevel="Trace" writeTo="barFile" />
  </rules>
</nlog>
```
    
Program.cs  
```
class NLog01
{
	private static NLog.Logger logger1 = NLog.LogManager.GetLogger("fooLogger");
	private static NLog.Logger logger2 = NLog.LogManager.GetLogger("barLogger");

	static void Main(string[] args)
	{
		logger1.Info("fooLoggerに出力");
		logger2.Info("barLoggerに出力");
	}
}
```
  
### CVS 포맷, 로테이션, 최대 보관 파일 수
NLog.config  
```
<?xml version="1.0" encoding="utf-8" ?>
<!--
  포맷 등의 자세한 설명은 http://nlog-project.org/ 을 참조
  프로젝트⇒프로퍼티⇒빌드이벤트⇒빌드 후에 실행하는 명령라인
  copy $(ProjectDir)NLog.config $(TargetDir)

  아래는 잘 안됨... XCOPY 종료 코드가 '2'로 되는 경우가 있기 때문
  xcopy $(ProjectDir)NLog.config $(TargetDir) /D

  아래에서는 잘 되지 않음... ROBOCOPY의 복사 시 종료 코드가 '1'이기 때문 
  ROBOCOPY $(ProjectDir) $(TargetDir) NLog.config /XO
-->
<nlog autoReload="true"
      xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <variable name="logFilename" value="${replace:searchFor=\.vshost$:replaceWith=\.log:regex=true:ignoreCase=true:inner=${processname}}"/>
  <variable name="logDir" value="${basedir:dir=../logs/}"/>
  <variable name="logFile" value="${logDir}${logFilename}"/>
  <targets async="true">
    <target name="console"
            xsi:type="Console"
            layout='${time} ${level:uppercase=true} "${message}"' />
    <target name="Debugger"
            xsi:type="Debugger"
            layout='${time} ${level:uppercase=true} "${message}"' />
    <target name ="logfile"
            xsi:type="File"
            filename="${logDir}${logFilename}"
            archiveFileName="${logDir}${logFilename}.{#}"
            archiveEvery="Day"
            archiveNumbering="Rolling"
            maxArchiveFiles="7"
            keepFileOpen="false"
            header="# -- encoding:utf-8 -- ＵＴＦ－８${newline}hahaha"
            layout='${counter:padding=8} ${longdate} [${threadid:padding=8}] [${uppercase:${level:padding=-5}}] ${callsite}:${callsite-linenumber} ${message} ${exception:format=tostring}'
            encoding="UTF-8" />
    <target name ="InfoLogFile"
            xsi:type="File"
            filename="${logDir}${logFilename}"
            archiveFileName="${logDir}${logFilename}.{#}"
            archiveEvery="Day"
            archiveNumbering="Rolling"
            maxArchiveFiles="7"
            keepFileOpen="false"
            encoding="utf-8" >
      <layout xsi:type="CSVLayout">
        <!-- CSV Options -->
        <!--         <header># - - encoding:utf-8 - - ＵＴＦ－８</header>-->
        <quoting>Auto</quoting>
        <withHeader>True</withHeader>
        <delimiter>Tab</delimiter>
        <column name="Counter" layout="${counter:padding=8}" />
        <column name="Date" layout="${longdate}" />
        <column name="ThreadName:id" layout="[${threadname}:${threadid}]" />
        <column name="Level" layout="[${uppercase:${level:padding=-5}}]" />
        <column name="Message" layout="${message}" />
        <column name="CallSite:LineNo" layout=" " />
        <column name="Exception" layout="${exception:format=tostring}" />
      </layout>
    </target>
    <target name ="FatalLogFile"
            xsi:type="File"
            filename="${logDir}${logFilename}"
            archiveFileName="${logDir}${logFilename}.{#}"
            archiveEvery="Day"
            archiveNumbering="Rolling"
            maxArchiveFiles="7"
            keepFileOpen="false"
            encoding="utf-8" >
      <layout xsi:type="CSVLayout">
        <!-- CSV Options -->
        <!--<header># - - encoding:utf-8 - - ＵＴＦ－８</header>-->
        <quoting>Auto</quoting>
        <withHeader>True</withHeader>
        <delimiter>Tab</delimiter>
        <column name="Counter" layout="${counter:padding=8}" />
        <column name="Date" layout="${longdate}" />
        <column name="ThreadName:id" layout="[${threadname}:${threadid}]" />
        <column name="Level" layout="[${uppercase:${level:padding=-5}}]" />
        <column name="Message" layout="${message}" />
        <column name="CallSite:LineNo" layout="${callsite}:${callsite-linenumber}" />
        <column name="Exception" layout="${exception:format=tostring}" />
      </layout>
    </target>
  </targets>
  <rules>
    <logger name="*" minlevel="Trace" writeTo="Debugger" />
<!--
    <logger name="*" minlevel="Warn" writeTo="FatalLogFile" />
    <logger name="*" minlevel="Info"  maxlevel="Info" writeTo="InfoLogFile" />
    <logger name="*" minlevel="Debug" maxlevel="Debug" writeTo="InfoLogFile" />
    <logger name="*" minlevel="Trace" maxlevel="Trace" writeTo="InfoLogFile" />
-->
    <logger name="*" minlevel="Trace" writeTo="FatalLogFile" />
  </rules>
</nlog>
```  
  
### Fluentd 노드에 로그 보내기
NLog.Targets.Fluentd: https://www.nuget.org/packages/NLog.Targets.Fluentd/  
2017년 글에는 설정 파일에서 할 수는 없고, 코드로 해야 한다고 한다.   
```
public class Configurator
{
    // Fluentd 설정을 반환한다
    static NLog.Targets.Target GetFluentdTarget()
    {
        var target = new NLog.Targets.Fluentd();
        target.Layout = new NLog.Layouts.SimpleLayout("${message}");
        target.Host = "１２７．０．０．１"; // TODO:환경에 따라 다르다
        target.Port = 24224; // TODO:환경에 따라 다르다
        target.Tag = "tag.tag"; // TODO:환경에 따라 다르다
        target.NoDelay = true;
        target.LingerEnabled = false;
        target.LingerTime = 2;
        target.EmitStackTraceWhenAvailable = false;
        return WrapTarget(target);
    }

    // Fluentd 설정만으로는 동기적으로 로그를 보내므로 비동기＆리트라이 설정을 한다
    static NLog.Targets.Target WrapTarget(NLog.Targets.Target target)
    {
        var retryWrapper = new NLog.Targets.Wrappers.RetryingTargetWrapper("RetryingWrapper", target, 3, 1000);
        var asyncWrapper = new NLog.Targets.Wrappers.AsyncTargetWrapper("AsyncWrapper", retryWrapper);
        return asyncWrapper;
    }

    public static NLog.Config.LoggingConfiguration GetConfig()
    {
        var config = new NLog.Config.LoggingConfiguration();
        var target = GetFluentdTarget();
        config.AddTarget("fluentd", target);
        config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Debug, target));
        return config;
    }
}

// Global.asax.cs(애플리케이션 초기화 시에 설정한다)
public class Global : HttpApplication
{
    protected void Application_Start()
    {
        // Fluentdの設定を行ったConfigを設定する
        LogManager.Configuration = LogConfigurator.GetConfig();
    }
}
```  
    
### DotNetZip으로 압축하기
로테이트 할 파일 이름을 zip으로 한다.  
FileTarget.FileCompressor 에 ZipFileCompressor을 지정한다.  
ZipFileCompressor 에서 DotNetZip을 이용하여 로테이트된 파일을 압축한다.  
  
NLog.config     
```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="c:\temp\nlog-internal.log">

  <targets>
    <target xsi:type="File"
            name="file"
            fileName="${basedir}/logs/${var:bhtid}_${shortdate}.log"
            layout="${longdate} [${uppercase:${level}}] ${message}"
            archiveFileName="${basedir}/logs/archives/archive_rolling.{#}.zip"
            archiveEvery="Day"
            archiveAboveSize="200"
            archiveNumbering="Rolling"
            maxArchiveFiles="7"
            enableArchiveFileCompression="true" />
  </targets>

  <rules>
    <logger name="*" minlevel="Debug" writeTo="file" />
  </rules>
</nlog>
```

ZipFileCompressor.cs  
```
internal class ZipFileCompressor : IFileCompressor
{
    internal ZipFileCompressor() { }

    public void CompressFile(string fileName, string archiveFileName)
    {
        var zip = new Ionic.Zip.ZipFile(Encoding.UTF8);
        var entry = zip.AddFile(fileName);
        entry.FileName = Path.GetFileName(fileName);
        zip.Save(archiveFileName);
    }
}
```    
  
Test.cs  
```
var logger = NLog.LogManager.GetCurrentClassLogger();
NLog.Targets.FileTarget.FileCompressor = new ZipFileCompressor();

logger.Info("이것은 테스트 이다. " + DateTime.Now.ToLongTimeString());
```  
  
### 아카이브 기능을 날짜시간 파일 이름으로 
NLOG의 아카이브 기능과 파일 이름의 {shotdate}는 병용 할 수 없다.  
파일 이름은 고정, 아카이브의 파일 이름을 날짜 붙임은 가능.  
archiveNumbering="Date", archiveFileName의 날짜 붙임 부분은 {#}을 사용한다.   
  
nlog.config   
```
<targets>
    <target
      xsi:type="File"
      name="TraceLog"
      fileName="${basedir}/logs/trace.log"
      encoding="UTF-8"
      lineEnding="CRLF"
      archiveFileName="${basedir}/logs/trace_{#}.log.zip"
      archiveEvery="Day"
      archiveNumbering="Date"
      enableArchiveFileCompression="true"
      layout="${longdate} [${threadid:padding=8}] [${uppercase:${level:padding=-5}}] ${message} ${exception:format=Message, Type, ToString:separator=\r\n}" />
</targets> 
```
    
  
  
### 코드에서 특정 로거 이름 선택해서 사용 
NLog.config  
```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

  <targets>
    <target name="logfile" xsi:type="File" fileName="file.txt" />
    <target name="logconsole" xsi:type="Console" />
  </targets>

  <rules>
    <logger name="LEVEL_INFO" minlevel="Info" writeTo="logconsole" />
    <logger name="LEVEL_DEBUG" minlevel="Debug" writeTo="logfile" />
  </rules>
</nlog>  
```  
  
Program.cs  
```
namespace NLogService
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = NLog.LogManager.GetLogger("LEVEL_INFO");
            logger.Info("Hello_World");
            Console.ReadKey();
        }
    }
}  
```  
결과: 콘솔에는 로그 메시지가 출력하지만 로그 파일에는 출력되지 않는다.
      
   
     
## 설정
[Target](https://nlog-project.org/config/?tab=targets )  
   
출처: http://dotnetcsharptips.seesaa.net/article/419883987.html  
### 구성 파일의 형식
NLog는 두 가지 구성 파일 형식이 있다.     
1. 표준 응용 프로그램 구성 파일에 작성 하는 서식   
2. 별도의 파일에 기술되는 간단한 서식  
  
```  
<configuration>
  <configSections>
    <section name="nlog" type="NLog.Config.ConfigSectionHandler, NLog"/>
  </configSections>
  <nlog>
  </nlog>
</configuration>
```  
  
VisualStudio에서 IntelliSense를 사용하려면 다음을 지정한다.  
```
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
</nlog>
```
  
**주의**  
네임 스페이스를 사용하지 않을 경우 대소 문자 구분 없음.   
네임 스페이스를 사용하는 경우 대소 문자 구분.     
  
<nlog />의　태그가 루트가 되고, 이 자식 요소로 아래의 태그로 지정한다. 처음 두 개는 필요하고, 이외는 옵션이다.   

<targets />　로그 대상 / 출력을 정의한다   
<rules />　로그 라우팅 규칙을 정의한다   
<extensions />　* .dll 파일에서 확장을 로드한다   
<include />　다른 구성 파일을 로드한다   
<variable />　구성 파일의 변수 값을 설정한다  

**Target**  
<targets />　섹션에 로그 대상을 정의한다. 다음의 두 가지 속성을 포함한다.  
- name : 대상 이름
- type : 대상 유형 (파일이나 데이터베이스 등)  
  

이러한 두 가지 특성 이외에, 분석 및 추적에 도움이 되는 매개 변수를 지정할 수 있다.   
각 대상마다 지정 가능한 파라미터는 다르므로 자세한 사양은 공식 사이트를 참조.   
또한 인텔리 센스도 VisualStudio에서 구할 수 있다.   
  
※ Target API ⇒　https://github.com/nlog/nlog/wiki/Targets  
※ Target의 구현 방법 ⇒　https://github.com/nlog/nlog/wiki/How%20to%20write%20a%20Target  

#### 규칙
<rules />　섹션에 라우팅 규칙을 정의한다. 간단한 테이블 구성이며, 소스와 로그 이름과 로그 레벨로 이루어지는 대상 목록을 정의한다.   
여러 규칙이 정의되어 있는 경우, 목록의 순서대로 규칙에 적합한지 확인한다.   
검사를 통과한 경우에 이 규칙에 정의된 Target에 기록된다.   
규칙에 Final 지정이 있는 경우는 이하의 규칙은 적용되지 않는다.    
  
각각의 라우팅 테이블 항목은 <logger /> 태그 요소이며, 다음과 같은 속성을 갖는다.  
- name : 소스 또는 로거의 이름 (와일드 카드 *도 OK)
- minlevel : 최소 로깅 수준
- maxlevel : 최대 로그 수준
- level :( 단수) 로그 수준
- levels :( 여러) 로그 수준. 쉼표로 구분하기
- writeTo : 쉼표로 구분 된 대상 목록.
- final : 마지막 부합되는 규칙. 이후에는 적용되지 않는다.
- enabled : enabled를 diable에 지정하여 규칙이 적용되지 않게된다.
  
수준에 대한 키워드(level, levels, minlevel, maxlevel) 중 하나 이상을 반드시 포함 할 필요가 있다.   
이들은 다음 순서로 평가된다.   
1. level 
2. levels 
3. minlevel / maxlevel (이 둘은 동일한 우선 순위) 
4. 키워드 없음 (모든 레벨이 대상)
  
    
#### 레이아웃과 레이아웃 렌더링
NLog의 가장 강력한 기능 중 하나가 레이아웃을 사용하는 것.   
이것은 ${} 로 묶인 태그로 작성된 텍스트이다.   
이 태그는 레이아웃 렌더러 라는 로그 출력에 관해 지정된 정보를 삽입하기 위해 사용된다.   
곳곳에 사용된 예를 들어 화면에 표시하거나 파일로 보내거나 정보를 제어하고 출력 파일 이름을 제어하기도 한다.   
  
예를 들어, 콘솔에 현재 시간 클래스 명과 메소드와 로그 수준과 메시지를 출력하려면 다음과 같이 기술한다.  
```
<target name="c" xsi:type="Console" layout="${longdate} ${callsite} ${level} ${message}" />
```
  
파일 이름 자체를 정의하려면 다음과 같이 기술한다.  
```
<target name="f" xsi:type="File" fileName="${logger}.txt"/>
```  
  
    
#### include 파일
<include /> 섹션을 사용한다. 설정이 여러 파일에 정의된 경우 다른 파일을 읽어 들이기 위한 설정이다.  
```
<include file="${basedir}/${machinename}.config"/>
```  
  
**ignoreError = "true"** 지정을 할 수 있다.   
기본값은 false이며, 명시적으로 true로 설정된 경우에는 지정 파일이 없거나 형식 오류다 라고 예외가 통지된다. 구성 파일 자체의 결함 분석 시에 사용  
  

변수  
복잡하거나 반복 사용하거나 같은 경우에 변수를 정의 할 수 있다  
```
<variable name="var" value="xxx" />
```
  
일단 정의 된 것은 ${var} 에 의해 사용할 수 있다.  
  

#### 자동 재구성
구성 파일에 정의된 설정은 응용 프로그램 시작 시 자동으로 로드된다.   
WindowsService 나 Web 시스템처럼 프로세스가 장기간 계속 실행되는 경우에 응용 프로그램을 다시 시작하지 않고도 설정을 다시로드 하기 위한 기능.   
다음 설정은 NLog가 구성 파일을 모니터링 하고 변경이 있을 경우 자동으로 읽기를 실시한다.  
  
```
<nlog autoReload="true" />
```  
  
수정 감시의 대처는 포함 파일도 포함된다. 어느 것 하나라도 파일이 변경된 경우에 전체를 다시 로드한다.  
  

#### 문제 해결을 위한 로깅
제대로 설정 하였음에도 불구하고 로그 출력이 되지 않는 경우가 있다.   
원인은 여러가지 상정 될 수 있으나, 가장 많은 것이 권한에 기인.   
예를 들어 ASP.NET의 경우 aspnet_wp.exe/w3wp.exe이 물리적 디스크에 대한 쓰기 권한을 부여되지 않은 경우 등.   
NLog은 NLog 자신이 원인인 경우에 예외를 출력하지 않지만, 다음의 설정에 따라 출력시킬 수 있다.  
- <nlog throwExceptions = "true"/> 이 설정은 NLog으로 인해 예외가 발생되게 된다. 배포 시 등 빠른 고장 원인의 추구가 가능해진다. 원인 판명 되는대로 NLog 유래에서 응용 프로그램 충돌을 일으키지 않도록 False 설정한다.
- <nlog internalLogFile = "file.txt"/>　이 설정은 NLog는 내부 디버깅 메시지를 지정된 특수 파일로 출력한다
- <nlog internalLogLevel = "Trace | Debug | Info | Warn | Error | Fatal "/>　내부 로그 레벨을 지정한다. 로그 레벨을 올릴수록 더 많은 정보가 출력된다.
- <nlog internalLogToConsole = "false | true"/>　 내부 로그 메시지를 콘솔 출력 여부
- <internalLogToConsoleError = "false | true"/>　내부 로그 메시지를 콘솔 오류 출력 여부
  
  
### 비동기 프로세스와 래퍼 대상
NLog는 다음과 같은 기능을 추가하여 다른 Target의 기능을 래핑하거나 합성 할 수 있다.   
- 비동기 프로세싱 (대상을 다른 스레드에서 실행시키는) 
- 예외시 재 시도 
- 로드 밸런싱 
- 버퍼링 
- 필터링 
- 페일 오버 
  
구성 파일에서 래퍼를 정의하려면 단순히 대상 노드를 중첩 시키면 되고, 계층 제한이 없다.   
다음은 리트라이 기능 및 비동기를 추가한 샘플.  
```
<targets>
  <target name="n" xsi:type="AsyncWrapper">
    <target xsi:type="RetryingWrapper">
      <target xsi:type="File" fileName="${file}.txt" />
    </target>
  </target>
</targets>
```
  
비동기는 자주 사용되므로 다음과 같이 단축 가능.  
```
<nlog>
  <targets async="true">
    <!-- all targets in this section will automatically be asynchronous -->
  </targets>
</nlog>
```
  
#### 기본 랩퍼
여러 대상에 동일한 래퍼를 부여하는 기능  
```
<nlog>  
  <targets>  
    <default-wrapper xsi:type="AsyncWrapper">  
      <wrapper-target xsi:type="RetryingWrapper"/>  
    </default-wrapper>  
    <target name="n1" xsi:type="Network" address="tcp://localhost:4001"/>  
    <target name="n2" xsi:type="Network" address="tcp://localhost:4002"/>  
    <target name="n3" xsi:type="Network" address="tcp://localhost:4003"/>  
  </targets>  
</nlog>
```
  
#### default Target 매개 변수
모든 대상에 동일한 파라미터 설정을 부여한다.  
```
<nlog>
  <targets>
    <default-target-parameters xsi:type="File" keepFileOpen="false"/>
    <target name="f1" xsi:type="File" fileName="f1.txt"/>
    <target name="f2" xsi:type="File" fileName="f2.txt"/>
    <target name="f3" xsi:type="File" fileName="f3.txt"/>
  </targets>
</nlog>
```
  
#### 이스케이프
설정 파일은 XML 형식이므로 "<" 와 ">" 등은 "&lt;" "&gt;" 로 이스케이프 한다.   
레이아웃 내에서는 "}" 이스케이프가 필요하다. 중첩되고 있는 경우는 불 필요.   
- ${appdomain:format={1\} {0\}}
- ${rot13:inner=${ndc:topFrames=3:separator=x}}
  
    
### 구성용 API
구성 파일에서 정의하는 내용은 모두 프로그램에서도 기술 할 수있다.  
  
#### 사용법
NLog를 코드로 구성하려면 다음 단계를 수행해야한다    
1. LoggingConfiguration 인스턴스를 생성하고 이것에 구성을 유지시킨다 
2. Target을 상속하는 1개 이상의 타겟의 인스턴스를 생성 
3. Target의 속성을 설정 
4. LoggingRule 클래스를 통해 규칙을 정의하고 이것을 confinguration의 LoggingRules에 추가 
5. Configuration 인스턴스를 LogManager.Configuration로 설정하게 하는 것으로 유효하게 한다.  
  
#### 샘플
다음은 두 가지 목표를 가진 샘플 코드.   
하나는 컬러 콘솔에, 다른 하나는 Debug 이상의 레벨의 로그를 파일 출력  
  
```
using NLog;
using NLog.Targets;
using NLog.Config;
using NLog.Win32.Targets;
 
class Example
{
    static void Main(string[] args)
    {
        // １．configuration을 생성
        var config = new LoggingConfiguration();
 
        // ２．target을 생성하고 configuration에 설정
        var consoleTarget = new ColoredConsoleTarget();
        config.AddTarget("console", consoleTarget);
 
        var fileTarget = new FileTarget();
        config.AddTarget("file", fileTarget);
 
        // ３．target의 프로퍼티를 설정
        consoleTarget.Layout = @"${date:format=HH\\:MM\\:ss} ${logger} ${message}";
        fileTarget.FileName = "${basedir}/file.txt";
        fileTarget.Layout = "${message}";
 
        // ４．규칙을 정의
        var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
        config.LoggingRules.Add(rule1);
 
        var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
        config.LoggingRules.Add(rule2);
 
        // ５．구성을 유효화
        LogManager.Configuration = config;
    }
}
```   
  
### API
 
#### Target
사용법  
Target은 메시지를 표시하거나 기록하거나 다른 목적지로 보내거나 하는데 사용된다.   
두 가지 있고, 하나는 메시지를 수신하여 처리한다. 다른 하나는 메시지를 배분하거나 버퍼링하기도 한다. 후자는 래퍼로 불린다.  
  
#### 제공되는 Target
※ 각 항목의 자세한 설정에 대한 링크가 있습니다. https://github.com/nlog/nlog/wiki/Targets  
- AspNetTrace - 로그 메시지를 ASP.NET trace 쓰기
- AspResponse - ASP Response object를 통해 로그 메시지를 출력한다
- Chainsaw - Log4J의 뷰어 인 Chainsow의 원격 인스턴스에 로그 메시지 보내기
- ColoredConsole - 콘솔 사용자 정의 색으로 로그 메시지 쓰기
- Console - 콘솔에 로그 메시지 쓰기
- Database - ADO.NET 공급자를 사용하여 데이터베이스에 로그 메시지 쓰기
- Debug - 모의 (테스트 용)
- Debugger - 연결된 디버거에 로그 메시지 쓰기
- EventLog - 이벤트 로그에 메시지 쓰기
- File - 로그 메시지를 파일에 쓰기
- FormControl - 지정된 이름의 Windows.Forms.Control의 Text 속성에 기록하기
- LogReceiverService - WCF 및 Web Services에 따르면 NLog Receiver Service에 로그 메시지 보내기
- Mail - SMTP 프로토콜을 사용하여 전자 메일로 로그 메시지 보내기
- Memory - 프로그램에서 검색 할 수 있도록 메모리에 ArrayList에 로그 메시지 쓰기
- MessageBox - 로그 메시지를 메시지 상자를 사용하여 팝업보기
- MethodCall - 로그 메시지마다 파라미터를 부여하여 지정된 정적 메소드를 호출
- MSMQ - MSMQ에서 관리되는 지정된 메시지 큐에 로그 메시지 쓰기
- Network - 네트워크를 통해 로그 메시지 보내기
- NLogViewer - 원격 NLog Viewer에 로그 메시지 보내기
- Null - 로그 메시지를 파기한다. 주로 디버깅이나 벤치 마크에 사용
- OutputDebugString - Win32 API를 OutputDebugString ()에 로그 메시지를 출력한다
- PerfCounter - 쓰기 작업마다 지정된 성능 카운터를 증가
- RichTextBox - 존재, 혹은 새로 만들기 RichTextBox에 로그 텍스트를 로깅
- Trace - System.Diagnostics.Trace에 로그 메시지 보내기
- WebService - 로그 메시지마다 지정된 WebService를 호출   
  
#### 래퍼 Target
- AspNetBufferingWrapper - ASP.NET 요청 중에 로그 이벤트를 버퍼링 요청 끝에 랩 된 대상에 쓰기
- AsyncWrapper - 대상이 쓰는 동안은 비동기 적으로 버퍼링을 수행하는
- AutoFlushWrapper - 랩 된 대상에 쓰기마다 플래시
- BufferingWrapper - 로그 이벤트를 버퍼링, 랩 된 대상에 모아 보낸다. 이메일 대상과 함께 유용
- FallbackGroup - 오류시 장애 복구 기능을 제공
- FilteringWrapper - 조건에 따라 로그 항목을 필터링
- ImpersonatingWrapper - 쓰기 할 때 다른 사용자로 위장
- PostFilteringWrapper - 이벤트의 그룹에 의해 평가 된 여러 조건에 따라 버퍼 된 로그를 필터링하기
- RandomizeGroup - 무작위로 선택된 대상에 로그 쓰기
- RepeatingWrapper - 지정 횟수만큼 각각의 소리의 로그 이벤트를 반복
- RetryingWrapper - 쓰기 실패하면 다시 도전
- RoundRobinGroup - 대상에 차례로 로그 이벤트를 배분
- SplitGroup - 모든 대상에 로그 이벤트를 쓴다
  
    
### 레이아웃
레이아웃은 대부분의 대상에있는 속성의 하나.   
레이아웃 속성은 로깅되는 정보의 형식을 결정하는 데 사용된다.   
많은 정의 된 매크로 즉 레이아웃 렌더러가 있다.  
  
### 기본 레이아웃
대상이 레이아웃 속성을 가지는 경우는 자유롭게 지정할 수 있지만, 명시적인 지정이없는 경우는 다음의 것이 사용된다. 
```
${longdate}|${level:uppercase=true}|${logger}|${message}
```  
  

### 정의된 레이아웃
※ 각 레이아웃에 대한 자세한 사양은이 페이지에서 링크되어 있습니다. https://github.com/nlog/nlog/wiki/Layouts
CsvLayout - CSV 포맷 이벤트 용 특수 레이아웃
LayoutWithHeaderAndFooter - 머리글과 바닥 글과를 지원하는 특수 레이아웃
Log4JXmlEventLayout - Log4j와 공유 할 수있는 XML 에벤토 용 특수 레이아웃
SimpleLayout - 출력시 정보에서 대체되는 부분을 포함 된 문자열을 나타내는  
  
    
### 레이아웃 렌더러
레이아웃 렌더러는 각 레이아웃에 사용되는 템플릿 매크로 것.
  

### 레이아웃 렌더러
※ 각 렌더러의 상세한 사양은이 페이지에 링크가 있다. https://github.com/nlog/nlog/wiki/Layout-Renderers  
${appsetting} - 응용 프로그램 정의 값   
${asp-application} - ASP 응용 프로그램의 변수  
${aspnet-application} - ASP.NET 응용 프로그램의 변수  
${aspnet-request} - ASP.NET 요청 변수  
${aspnet-session} - ASP.NET 세션 변수  
${aspnet-sessionid} - ASP.NET 세션 ID  
${aspnet-user-authtype} - ASP.NET 사용자 변수  
${aspnet-user-identity} - ASP.NET 사용자 변수  
${asp-request} - ASP 요청 변수  
${asp-session} - ASP 세션 변수  
${assembly-version} - 어셈블리의 버전  
${basedir} - 실행중인 응용 프로그램의 기본 디렉토리  
${callsite} - 호출 부분 (클래스 명, 메소드 명, 소스 정보)  
${counter} - 카운트 값 (각 레이아웃 렌더링 장비마다 증가)  
${date} - Current date and time.  
${document-uri} - Silverlight 응용 프로그램을 말리는하고있는 HTML 페이지의 URI  
${environment} - 환경 변수  
${event-context} - 로그 이벤트의 컨텍스트 데이터  
${exception} - 예외 정보  
${file-contents} - 지정된 파일의 내용  
${gc} - 가비지 컬렉터에 대한 정보  
${gdc} - Global Diagnostic Context item. log4net.와 공용으로 사용  
${guid} - GUID (Globally-unique identifier)  
${identity} - 스레드 정보 (스레드 이름 및 인증 정보)  
${install-context} - 설치 매개 변수 (passed to InstallNLogConfig).  
${level} - 로그 수준  
${literal} - 문자열  
${log4jxmlevent} - log4의 Chainsaw와 NLogViewer과 공유 할 수있는 XML 형식의 정보  
${logger} - 로거 이름  
${longdate} - 로그시의 시간 (형식은 yyyy-MM-dd HH : mm : ss.mmm)  
${machinename} - 프로세스가 실행중인 컴퓨터 이름  
${mdc} - Mapped Diagnostic Context item. log4net.와 공용으로 사용  
${message} - The formatted log message.  
${ndc} - Nested Diagnostic Context item. log4net.와 공용으로 사용.  
${newline} - 개행 문자  
${nlogdir} - NLog.dll가 배치되어있는 폴더 이름  
${performancecounter} - 성능 카운터  
${processid} - 프로세스 ID  
${processinfo} - 실행중인 프로세스 정보  
${processname} - 프로세스 이름  
${processtime} - 프로세스 시간 (서식 HH : mm : ss.mmm)  
${qpc} - QueryPerformanceCounter ()의 반환 값 인 정밀 시간. 초 변환도 가능  
${registry} - 레지스트리 값  
${shortdate} - 날짜 (형식 yyyy-MM-dd)   
${sl-appinfo} - Silverlight 응용 프로그램 정보 
${specialfolder} - 특수 폴더 (내 문서와 프로그램 파일 스 등) 경로  
${stacktrace} - 스택 트레이스  
${tempdir} - 임시 폴더  
${threadid} - 현재 쓰레드 ID  
${threadname} - 현재 스레드 이름  
${ticks} - 현재 날짜 Ticks 값  
${time} - 시간 (서식 HH : mm : ss.mmm)  
${windows-identity} - TWI (Thread Windows identity) 정보 (사용자 이름).  
    
  
### 래퍼 레이아웃 렌더러
※ 각 래퍼의 상세한 사양은이 페이지에 링크가 있다 ⇒ https://github.com/nlog/nlog/wiki/Layout-Renderers
- ${cached} - 캐싱 적용
- ${filesystem-normalize} - 파일 이름으로 사용할 수없는 문자를 안전한 문자로 대체
- ${json-encode} - JSON 규칙에서 탈출 출력
- ${lowercase} - 문자 변환
- ${onexception} - 예외 만 출력
- ${pad} - 패딩 적용
- ${replace} - 대체
- ${rot13} - ROT-13에서 암호화 된 텍스트 해독
- ${trim-whitespace} - 공백 제거
- ${uppercase} - 대문자 변환
- ${url-encode} - URL을 사용하여 인코딩
- ${when} - 지정된 조건을 만족하는 경우에만 출력
- ${whenEmpty} - 다른 레이아웃의 출력 결과가 빈 경우에 다른 레이아웃을 적용
- ${xml-encode} - XML 호환으로 대체
  
   
### 사용자 정의 레이아웃 렌더러
${gelf} - GELF(Graylog Extended Log Format)로 변환
  
   
### 레이아웃에 값 전달
많은 렌더러가 정의되어 제공되고 있지만, 자체 구현하는 것도 가능하다. 
레이아웃 렌더러에 특정 값을 넘겨 이벤트에 고유한 속성을 코드 측에서 추가한다. 자세한 내용은 ${event-context}를 참조.
  

### 필터
로그 출력은 구성 파일의 정의에서 필터링 가능   
※ 각 필터의 상세한 사양은이 페이지에 링크가 있습니다 ⇒ https://github.com/nlog/nlog/wiki/Filters  
  

### 제공되는 필터
- when filter - 지정된 조건에 부합하는 경우
  
  
### 비추천 필터
다음 필터가 아닌 when filter를 사용하라    
- whenContains filter - 특정 문자열이 포함된 경우
- whenEqual filter - 특정 문자열과 동일한 경우
- whenNotContains filter - 특정 문자열이 포함되지 않는 경우
- whenNotEqual filter - 특정 문자열과 동일하지 않은 경우
  

### 조건
when filter 와 함께 사용되는 조건식.   
When 필터가 동작을 일으키는 여부를 결정하는데 쓰인다.  
    

### 언어 사양
조건식은 특별한 미니 언어로 작성된다.
  

- 관계 연산자 - ==,! =, <, <=,> =,> (이스케이프가 필요한 문자 수)
- 부울 연산자 - and, or not
- 리터럴 문자열 - & {somerenderer} 렌더러를 사용
- 부울 문자열 - true, false
- 수치 형 문자열 - 예를 들어 12345는 Int 형 문자열 1234.56 부동 소수점 형 문자열
- 로그 레벨 문자열 - LogLevel.Trace, LogLevel.Debug .....
- 예약어 문자열 - level, message, logger
- 괄호 - 기본 속성과 그룹 식 및 덮어 쓰기
- 함수 - string 및 object 와 테스트
    

### 함수
다음 함수를 사용할 수 있다.  
- contains (s1, s2) - 첫 번째 인수 문자열에 두 번째 인수가 포함 된 경우 true, 없으면 false를 반환
- end-with (s1, s2) - 첫 번째 인수 문자열이 두 번째 인수의 문자열로 끝나는 경우 true, 없으면 false를 반환
- equals (o1, o2) - 두 인수가 동일한 경우는 true, 그렇지 않으면 false를 반환
- length (s) - 문자열의 길이를 반환
- start-with (s1, s2) - 첫 번째 인수 문자열이 두 번째 인수 문자열로 시작 경우 true, 없으면 false를 반환  

예  
```
<rules>
    <logger name="*" writeTo="file">
        <filters>
            <when condition="length('${message}') > 100" action="Ignore" />
            <when condition="equals('${logger}','MyApps.SomeClass')" action="Ignore" />
            <when condition="(level >= LogLevel.Debug and contains('${message}','PleaseDontLogThis')) or level==LogLevel.Warn" action="Ignore" />
            <when condition="not starts-with('${message}','PleaseLogThis')" action="Ignore" />
        </filters>
    </logger>
</rules>
```
  

### 확장
정적 공개 클래스에 정적 메소드를 정의하고 다음과 같이 ConditionMethods, ConditionMethod 특성을 각각 부여.  
```
namespace MyExtensionNamespace 
{ 
    using System; 
    using NLog.Conditions; 
 
    [ConditionMethods] 
    public static class MyConditionMethods 
    { 
        [ConditionMethod("myrandom")] 
        public static int Random(int max) 
        { 
            return new Random().Next(max); 
        } 
    } 
} 
 
<rules>
    <logger name="*" writeTo="file">
        <filters>
            <when condition="length('${message}') > 100" action="Ignore" />
            <when condition="equals('${logger}','MyApps.SomeClass')" action="Ignore" />
            <when condition="(level >= LogLevel.Debug and contains('${message}','PleaseDontLogThis')) or level==LogLevel.Warn" action="Ignore" />
            <when condition="not starts-with('${message}','PleaseLogThis')" action="Ignore" />
        </filters>
    </logger>
</rules>
```  
  
### 로그 수준 
다음의 6종류 (Log4net와 동일)
- off
- Fatal
- Error
- Warn
- Info
- Debug
- Trace
  

## 볼 글
- [(일어)ASP.NET Core에서 NLog 설정하기(EF Migration 추가)](https://qiita.com/GCnaoto/items/d5d9743d09aaccb82d82)  

  
    
## 참고한 문서
- https://ohke.hateblo.jp/entry/2017/02/24/221702




  
