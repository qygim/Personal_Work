using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

//게임 인트로
class Intro
{
	int X;
	int Y;

	public Intro(int X, int Y)
	{
		Debug.Assert(X != 0);	//	X,Y가 0이면 터짐
		Debug.Assert(Y != 0);

		if(X<0)
		{
			X *= -1;
		}

		if(Y<0)
		{
			Y *= -1;
		}

		this.X = X;
		this.Y = Y;
	}

	public bool GameIntro()
	{
		ConsoleKeyInfo K;

		Console.SetCursorPosition(X, Y);
		Console.WriteLine("Game Start?");
		Console.SetCursorPosition(X, Y+1);
		Console.WriteLine("Yes->Enter!");
		Console.SetCursorPosition(X, Y+2);
		Console.Write("No->ESC");

		K = Console.ReadKey();

		bool Check = K.Key == ConsoleKey.Enter ? true : false;	//키보드 키를 읽어와서 enter면 시작,esc이면 게임 종료

		return Check;
	}

	public void PrintText()
	{
		//인트로 텍스트 파일에서 내용을 읽어온 뒤 한 줄씩 출력
		
		string[] Text = File.ReadAllLines(@"..\..\Intro.txt", Encoding.Default);
		string[] Text2 = File.ReadAllLines(@"..\..\Tutorial.txt", Encoding.Default);

		for (int i = 0; i < Text.Length; i++)
		{
			Console.SetCursorPosition(X / 2, Y + i-3);
			Console.WriteLine(Text[i]);
		}

		Console.ReadLine();
		Console.Clear();

		for(int i=0;i<Text2.Length;i++)
		{
			Console.SetCursorPosition(X/2, Y / 2+i);
			Console.WriteLine(Text2[i]);
		}
	}
}

