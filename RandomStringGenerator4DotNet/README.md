# 랜덤 문자열 생성
임시 비밀번호 등을 만들 때 사용하면 좋을 듯.

사용 방법    
```
var generator = new RandomStringGenerator.StringGenerator();
generator.MinLowerCaseChars = 2;
generator.MinUpperCaseChars = 1;
generator.MinNumericChars = 3;
generator.MinSpecialChars = 2;
generator.FillRest = RandomStringGenerator.CharType.LowerCase;

var token = generator.GenerateString(10);
```
   
`StringGenerator.cs` 파일 1개에 모든 기능이 다 구현 되어 있다.     
    
	
	
	
    
# Random String Generator for .NET
Author: Keyvan Nayyeri  
Blog: http://keyvan.io  
Podcast: http://keyvan.fm  
Twitter: http://twitter.com/keyvan  
Contact Info: http://keyvan.tel  
******************************************************************  
This is a .NET library for generating random strings with fully  
customizable settings to set the number of upper, lower, numeric,  
and special characters along with the length of the string.  
This library can be used for various applications such as random   
password generation.  
  
The implementation is described in details on my blog:  http://keyvan.io/fully-customizable-random-password-generator    
NuGet Package: http://nuget.org/packages/RandomStringGenerator4DotNet  
