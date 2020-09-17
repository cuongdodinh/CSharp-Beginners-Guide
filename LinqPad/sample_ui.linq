<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.Formatters.Soap.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\UIAutomationProvider.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\UIAutomationTypes.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\ReachFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationUI.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\System.Printing.dll</Reference>
</Query>

enum TextBoxMode
{
	SingleLine,
	MultiLine
}

System.Windows.Controls.TextBox CreateTextBox(string label, TextBoxMode textBoxMode)
{
	var lbl = new System.Windows.Controls.Label();
	lbl.Content = $"{label}:";
	lbl.Dump();

	var textBox = new System.Windows.Controls.TextBox();
	if (textBoxMode == TextBoxMode.MultiLine)
	{
		textBox.AcceptsReturn = true;
		textBox.AcceptsTab = true;
		textBox.Height = 150;
		textBox.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
		textBox.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
	}
	textBox.FontFamily = new System.Windows.Media.FontFamily("Courier New");
	textBox.Dump();

	return textBox;
}

System.Windows.Controls.Button CreateButton(string caption)
{
	var button = new System.Windows.Controls.Button();
	button.Content = caption;
	button.Width = 100;
	button.Margin = new System.Windows.Thickness(5);
	button.Dump();

	return button;
}

void Main()
{
	var textName = CreateTextBox("Name", TextBoxMode.SingleLine);
	var textInput = CreateTextBox("Input", TextBoxMode.MultiLine);
	var btnGo = CreateButton("Go");
	var textOutput = CreateTextBox("Output", TextBoxMode.MultiLine);

	btnGo.Click += (s, e) => textOutput.Text = Process(textInput.Text);
}

string Process(string input)
{
	return input;
}

// Define other methods and classes here
