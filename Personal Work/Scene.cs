using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Scene
{
	Player Player;
	Intro Intro;
	Outtro Outtro;
	//Image Image;
	UI UI;
	Audience Audience;

	public Scene()
	{
		Console.SetWindowSize(150, 40);
		TableManager.CreateInstance();
		Stage.CreateInstance(70, 35, '■');
		Player = new Player(1, 0, 0, 0);
		Intro = new Intro(70, 20);
		Outtro = new Outtro(70, 20);
		//Image = new Image();
		UI = new UI(Stage.Ins().GetWidth, Stage.Ins().GetHeight);
		Audience = new Audience();
	}

	public void Run()
	{
		GameIn();

		while (true)
		{	
			//스테이지 처음 시작시 or 클리어하고 다시 시작시
			if (Stage.Ins().CheckClear)
			{
				//스테이지를 마지막 3단계까지 클리어 했는지 안했는지 체크
				if (Stage.Ins().StageCount == Stage.Ins().GetMaxStage)
				{
					break;
				}

				Console.SetCursorPosition(70, 20);
				//스테이지 단계 표시
				Console.Write("Stage {0}", Stage.Ins().StageCount);
				//해당 스테이지의 데이터를 얻어옴
				Stage.Ins().GetMapData();
				//새로운 죄인 설정
				Stage.Ins().RoadNewSinner();

				//플레이어의 평판과 양심을 다시 0으로 설정
				Player.Confidence = 0;
				Player.Reputation = 0;
				Stage.Ins().CheckClear = false;
			}

			var k = Key();

			if (Update(k) == false)
			{
				break;
			}
			
			//스테이지가 클리어 상태가 아니면 UI 및 스테이지를 그림
			if (!Stage.Ins().CheckClear)
			{
				Stage.Ins().Render();
				UI.Render(Stage.Ins().CurrentSinnerID, Player, Audience);
				Console.SetCursorPosition(99, 15);
				//Image.Draw(Args, Stage.CurrentSinnerID);
			}
		}

		GameOut();
	}

	//누른 키 값을 가져옴
	ConsoleKeyInfo Key()
	{
		var K = Console.ReadKey();
		Console.Clear();
		return K;
	}

	bool Update(ConsoleKeyInfo Key)
	{
		switch (Key.Key)
		{
			//왼쪽 화살표 누르면 무죄 버튼 체크
			case ConsoleKey.LeftArrow:
				UI.InocenceCheck = true;
				UI.GuiltyCheck = false;
				break;
			
			//오른쪽 화살표 누르면 유죄 버튼 체크
			case ConsoleKey.RightArrow:
				UI.GuiltyCheck = true;
				UI.InocenceCheck = false;
				break;

			//엔터를 누르면 최종 결정
			case ConsoleKey.Enter:
				//무죄나 유죄 버튼을 눌렀을 때만
				if (UI.InocenceCheck || UI.GuiltyCheck)
				{
					//플레이어가 얻을 수 있는 양심 수치,평판 수치,포인트를 계산
					Player.Calc(UI.InocenceCheck,UI.GuiltyCheck);
					//버튼을 다시 안누른 상태로 전환
					UI.InocenceCheck = false;
					UI.GuiltyCheck = false;
					//정보 열람 상태를 다시 닫힘 상태로 바꿈
					for (int i = 0; i < 3; i++)
					{
						UI.InfoCheck[i] = true;
					}

					//해당 스테이지의 재판 할당량을 채웠는지 확인 
					if (Stage.Ins().QuotaCheck())
					{
						//재판 할당량을 채웠을 때 플레이어의 양심,평판 수치가 스테이지의 최저 양심,평판 수치보다 높거나 같은지 체크
						if (Player.Confidence >= Stage.Ins().LowestConfi && Player.Reputation >= Stage.Ins().LowestRepu)
						{
							//높거나 같으면 다음 스테이지로
							Stage.Ins().StageCount += 1;
							Stage.Ins().CheckClear = true;
							break;
						}

						else
						{
							//아니면 게임 오버
							return false;
						}
					}

					//재판 할당량을 안채웠으면 새로운 죄인 선정
					Stage.Ins().RoadNewSinner();

				}
				break;

			case ConsoleKey.S:
				//S를 누르면 정보2를 열람
				if (Player.Point < 10)
				{
					UI.PointLack = true;
				}

				else
				{
					//정보2의 텍스트를 열림으로 바꿈
					Stage.Ins().GetSinnerData().Text.Unrock[1] = true;
					//UI 열람 버튼(?)을 끔
					UI.InfoCheck[0] = false;
					//정보가 열렸으므로 해당 정보 타입을 토대로 청중들의 상태를 바꿈
					Audience.ChangeState(Stage.Ins().GetSinnerData().Text.InfoType(2));
					//정보를 열람했으므로 플레이어의 열람 포인트 감소
					Player.Point -= 10;
				}
				break;

			case ConsoleKey.D:
				//D를 누르면 정보3을 열람
				if (Player.Point < 20)
				{
					UI.PointLack = true;
				}

				else
				{
					//S키를 누를때와 동일 정보3인 것만 바뀜
					Stage.Ins().GetSinnerData().Text.Unrock[2] = true;
					UI.InfoCheck[1] = false;
					Audience.ChangeState(Stage.Ins().GetSinnerData().Text.InfoType(3));
					Player.Point -= 20;
				}
				break;

			case ConsoleKey.F:
				//F를 누르면 정보4를 열람
				if (Player.Point < 30)
				{
					UI.PointLack = true;
				}

				else
				{
					//S키를 누를 때와 동일 정보4인 것만 바뀜
					Stage.Ins().GetSinnerData().Text.Unrock[3] = true;
					UI.InfoCheck[2] = false;
					Audience.ChangeState(Stage.Ins().GetSinnerData().Text.InfoType(4));
					Player.Point -= 30;
				}
				break;

			default:
				break;
		}


		return true;
	}

	//게임을 클리어했거나 중간에 종료당하거나 했을 때의 함수
	void GameOut()
	{
		Console.Clear();
		
		//클리어하지 못했을 시
		if (Stage.Ins().StageCount < Stage.Ins().GetMaxStage)
		{
			Outtro.GameOver();
		}

		//클리어 했을 시
		else
		{
			Outtro.Clear();
		}

		Console.ReadLine();
	}

	void GameIn()
	{
		//값 얻어오기
		Player.GetPlayerData();
		Stage.Ins().GetMapData();
		Stage.Ins().Add(101);

		//게임 인트로
		
		//Enter 누를 시
		if (Intro.GameIntro())
		{
			Console.Clear();
			Intro.PrintText();
			Console.ReadLine();  
			Console.Clear();
		}

		//ESC 누를 시
		else
		{
			Console.Clear();
			Console.SetCursorPosition(65, 20);
			Console.Write("See you Next!");
			Console.ReadLine();
			Environment.Exit(0);
		}
	}

}

