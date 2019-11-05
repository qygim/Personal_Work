using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

class Outtro
{
	int X;
	int Y;

	public Outtro(int X, int Y)
	{
		Debug.Assert(X != 0);   //	X,Y가 0이면 터짐
		Debug.Assert(Y != 0);

		if (X < 0)
		{
			X *= -1;
		}

		if (Y < 0)
		{
			Y *= -1;
		}

		this.X = X;
		this.Y = Y;
	}

	public void GameOver()
	{
		//중간에 게임 오버되면 출력
		Console.SetCursorPosition(X / 2, Y);
		Console.WriteLine("안타깝습니다...그만 직장을 잃고 말았습니다...");
	}

	public void Clear()
	{
		//모든 스테이지를 클리어하면 출력
		Console.SetCursorPosition(X / 2, Y);
		Console.WriteLine("축하합니다!성실히 일해 승진하였습니다!");
	}
}

