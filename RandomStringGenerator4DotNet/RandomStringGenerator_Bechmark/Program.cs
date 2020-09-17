using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace RandomStringGenerator_Bechmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();

            //BenchmarkRunner.Run<RandomStringGeneratorBenchmark>();
        }

        static void Test()
        {
            Console.WriteLine("랜덤 문자열 생성하기");

            var generator = new RandomStringGenerator.StringGenerator();
            generator.MinLowerCaseChars = 2;
            generator.MinUpperCaseChars = 1;
            generator.MinNumericChars = 3;
            generator.MinSpecialChars = 2;
            generator.FillRest = RandomStringGenerator.CharType.LowerCase;

            var token = generator.GenerateString(10);
            Console.WriteLine(token);
        }
    }


    // BenchmarkDotNet 라이브러리 소개   https://www.sysnet.pe.kr/2/0/11547
    public class RandomStringGeneratorBenchmark 
    {
        [Benchmark]
        public void ClassVectorTest()
        {
            var generator = new RandomStringGenerator.StringGenerator();

            generator.MinLowerCaseChars = 2;
            generator.MinUpperCaseChars = 1;
            generator.MinNumericChars = 3;
            generator.MinSpecialChars = 2;
            generator.FillRest = RandomStringGenerator.CharType.LowerCase;

            var token = generator.GenerateString(10);
        }
    }
}
