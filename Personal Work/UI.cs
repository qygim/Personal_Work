using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

class UI
{
	//콘솔크기가 150,40 고정이라 Drawbox 인자값을 매직넘버로 그냥 줬습니다. 
	
	bool[] Check;       //해당 정보가 열람 됐는지 안됐는지를 체크
	int InocenceConfi;     //양심 증가 수치(무죄시)
	int InocenceRepu;      //평판 증가 수치(무죄시)
	int GuiltyConfi;        //양심 증가 수치(유죄시)
	int GuiltyRepu;         //평판 증가 수치(유죄시)

	int DefaultConfi;	//플레이어가 얻을 수 있는 기본 양심 수치
	int DefaultRepu;	//플레이어가 얻을 수 있는 기본 평판 수치

	#region Property

	public bool InocenceCheck	//무죄 버튼을 눌렀는지 체크하는 변수
	{
		get;
		set;
	} = false;

	public bool GuiltyCheck		//유죄 버튼을 눌렀는지 체크하는 변수
	{
		get;
		set;
	} = false;

	public bool[] InfoCheck		//몇 번째 정보가 열렸는지 체크하는 변수
	{
		get { return Check; }
		set { Check = value; }
	}

	public bool PointLack		//정보 열람시 요구 열람 포인트보다 플레이어의 열람 포인트가 적은지 체크하는 변수
	{
		get;
		set;
	} = false;
	
	#endregion

	public UI(int Width, int Height)
	{
		InfoCheck = new bool[3];
		for (int i = 0; i < 3; i++)
		{
			InfoCheck[i] = true;
		}

		InocenceConfi=0;
		InocenceRepu=0;
		GuiltyConfi=0;
		GuiltyRepu=0;
		DefaultConfi = 0;
		DefaultRepu = 0;
	}

	//각종 UI그림
	public void Render(int ID, Player P,Audience A)
	{
		SinnerInformationBox(ID);
		PointBox(P.Point);
		ConfidenceReputationBox(ID, P.Reputation, P.Confidence);
		InnocenceBox(InocenceCheck);
		GuiltyBox(GuiltyCheck);
		AudienceBox(A);
	}

	//박스를 그리는 공통 함수
	private void DrawBox(int X, int Y, int Width, int Height)
	{

		Console.SetCursorPosition(X, Y);
		Console.Write('┌');

		for (int i = 0; i < Width; i++)
		{
			Console.SetCursorPosition(X + 1 + i, Y);
			Console.Write('─');
		}

		Console.SetCursorPosition(X + Width + 2, Y);
		Console.Write('┐');

		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width + 3; j++)
			{
				if (j == 0 || j == Width + 2)
				{
					Console.SetCursorPosition(X + j, Y + 1 + i);
					Console.Write('│');
				}

				else
				{
					Console.Write(' ');
				}
			}
		}

		Console.SetCursorPosition(X, Y + Height + 1);
		Console.Write('└');

		for (int i = 0; i < Width; i++)
		{
			Console.SetCursorPosition(X + i + 1, Y + 1 + Height);
			Console.Write('─');
		}

		Console.SetCursorPosition(X + Width + 2, Y + 1 + Height);
		Console.Write('┘');
	}

	//플레이어가 사용가능한 열람 포인트를 표시하는 박스
	private void PointBox(int Point)
	{
		DrawBox(6, 21, 25, 11);
		Console.SetCursorPosition(8, 22);
		Console.Write("총 열람 포인트 : {0}", Point);
		if(PointLack)
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.SetCursorPosition(8, 24);
			Console.Write("열람 포인트가 부족합니다");
			Console.BackgroundColor = ConsoleColor.Black;
			PointLack = false;
		}

		Console.SetCursorPosition(8, 26);
		Console.Write("현재 재판한 죄인 수 : {0}", Stage.Ins().CurrentNum);
		Console.SetCursorPosition(8, 28);
		Console.Write("총 재판할 죄인 수 : {0}", Stage.Ins().StageQuata);

	}

	//평판,수치를 표시하는 박스
	private void ConfidenceReputationBox(int ID, int Reputation, int Confidence)
	{
		StatTable Temp= Stage.Ins().GetSinnerData().Stat;	//현재 죄인의 스탯을 얻어옴

		//죄인의 스탯을 UI변수에 저장
		InocenceConfi =	Temp.IConfi;
		InocenceRepu = Temp.IRepu;
		GuiltyConfi = Temp.GConfi;
		GuiltyRepu = Temp.GRepu;
		DefaultConfi = Temp.Confidence;
		DefaultRepu = Temp.Reputation;

		DrawBox(96, 2, 37, 10);
		Console.SetCursorPosition(98, 3);

		//현재 플레이어의 평판,무죄,유죄시 얻을 수 있는 값을 계산한 뒤 보여줌
		Console.Write("평판 : {0} + (무죄 {1} 유죄 {2})", Reputation, DefaultRepu+InocenceRepu,DefaultRepu+GuiltyRepu);
		DrawPlayerGauge(ConsoleColor.Cyan, Reputation, 117, 5);

		Console.SetCursorPosition(98, 6);
		Console.Write("최소 평판 : {0}", Stage.Ins().LowestRepu);
		DrawPlayerGauge(ConsoleColor.White, Stage.Ins().LowestRepu, 117, 6);

		Console.SetCursorPosition(98, 8);
		Console.Write("양심 : {0} + (무죄 {1} 유죄 {2})", Confidence,DefaultConfi+InocenceConfi,DefaultConfi+GuiltyConfi);
		DrawPlayerGauge(ConsoleColor.Magenta, Confidence, 117, 10);

		Console.SetCursorPosition(98, 11);
		Console.Write("최소 양심 : {0}", Stage.Ins().LowestConfi);
		DrawPlayerGauge(ConsoleColor.White, Stage.Ins().LowestConfi, 117, 11);

		return;

		//플레이어의 양심과 평판 정도,스테이지 클리어를 위한 최저 양심,최저 평판 수치를 그리는 함수
		void DrawPlayerGauge(ConsoleColor Color, int Value, int X, int Y)
		{
			Console.SetCursorPosition(X, Y);
			Console.BackgroundColor = Color;
			for (int i = 0; i < (int)(Value * 0.1); i++)
			{
				Console.Write(' ');
			}
			Console.BackgroundColor = ConsoleColor.Black;
		}

	}

	//무죄 버튼 박스
	private void InnocenceBox(bool Check)
	{
		if (Check)
		{
			Console.BackgroundColor = ConsoleColor.Blue;
		}
		DrawBox(96, 26, 17, 7);
		Console.SetCursorPosition(102, 30);
		Console.Write("Innocence");
		Console.BackgroundColor = ConsoleColor.Black;
	}

	//유죄 버튼 박스
	private void GuiltyBox(bool Check)
	{
		if (Check)
		{
			Console.BackgroundColor = ConsoleColor.Red;
		}
		DrawBox(117, 26, 16, 7);
		Console.SetCursorPosition(124, 30);
		Console.Write("Guilty");
		Console.BackgroundColor = ConsoleColor.Black;
	}

	//죄인 정보를 표시하는 박스
	private void SinnerInformationBox(int ID)
	{
		DrawBox(3, 2, 89, 31);
		DrawBox(6, 4, 25, 15);
		DrawBox(35, 4, 55, 28);
		Stage.Ins().GetSinnerData().ShowSinnerStory(37, 5);

		if (InfoCheck[0])
		{
			Console.SetCursorPosition(37, 12);
			Console.WriteLine("열람(S) : 소모 포인트 10");
		}

		if (InfoCheck[1])
		{
			Console.SetCursorPosition(37, 18);
			Console.WriteLine("열람(D) : 소모 포인트 20");
		}

		if (InfoCheck[2])
		{
			Console.SetCursorPosition(37, 24);
			Console.WriteLine("열람(F) : 소모 포인트 30");
		}
	}

	//청중들의 상태를 나타내는 박스
	private void AudienceBox(Audience A)
	{
		DrawBox(96, 14, 37, 10);

		if (A.Check)
		{
			A.SetState(Stage.Ins().CurrentSinnerID);
		}

		Console.SetCursorPosition(100, 15);
		A.AudienceBehavior();

		DrawAudienceState(A.AudienceState, 110, 17);

		return;

		//청중들의 상태에 따라 청중 기분을 다른 색으로 그림
		void DrawAudienceState(AudienceState State, int X, int Y)
		{
			Debug.Assert(State != AudienceState.None);

			int PositionX = X;

			switch (State)
			{
				case AudienceState.Anger:
					Console.BackgroundColor = ConsoleColor.Red;
					break;

				case AudienceState.Discomfort:
					Console.BackgroundColor = ConsoleColor.Yellow;
					PositionX += 5;
					break;

				case AudienceState.Regret:
					Console.BackgroundColor = ConsoleColor.Blue;
					PositionX += 10;
					break;

				case AudienceState.Angrevate:
					Console.BackgroundColor = ConsoleColor.DarkRed;
					PositionX += 15;
					break;
			}

			for (int i = 0; i < 8; i++)
			{
				Console.SetCursorPosition(PositionX, Y + i);
				Console.Write(" ");
			}

			Console.BackgroundColor = ConsoleColor.Black;
		}
	}
}

