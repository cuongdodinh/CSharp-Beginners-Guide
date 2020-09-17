<Query Kind="Program" />

// You can also write your own methods and classes. Just change the language dropdown to 'C# Program'.
// LINQPad will automatically generate a 'Main' method.

void Main()
{
	MyMethod();	
	new MyClass().GetHelloMessage().Dump();
}

void MyMethod()
{
	"LINQPad is the ultimate .NET code scratchpad!".Dump();
}

class MyClass
{
	public string GetHelloMessage() => 
		"Put an end to those hundreds of Visual Studio Console projects cluttering your source folder!";
	
	// To reference any extra assemblies, or to import namespaces, just press F4!
}