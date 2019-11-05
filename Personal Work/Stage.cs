using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

class Stage
{
	#region SINGLETON

	private static Stage StageInstance = null;

	public static void CreateInstance(int Width,int Height,char C)
	{
		if(StageInstance==null)
		{
			StageInstance = new Stage(Width, Height, C);
		}
	}

	public static Stage Ins()
	{
		return StageInstance;
	}

	#endregion
	
	int Width;
	int Height;
	char C;						//맵을 그릴 문자모양
	List<Sinner> SinnerList;	//죄인들의 목록을 만들 리스트
	int Quota;					//각 스테이지마다 재판 할당량(유무죄 상관없음)
	int CheckSinnerNum;         //현재 스테이지에서 재판한 죄인의 수
	int LowestConfidence;
	int LowestReputation;

	#region Property

	public int GetWidth
	{
		get { return Width; }
	}

	public int GetHeight
	{
		get { return Height; }
	}

	public int GetMaxStage	//최대 스테이지(총3단계,StageCount의 시작이 1이라 맞추기 위해 4로 설정)
	{
		get;
	} = 4;

	public int StageCount	//현재 스테이지 단계,초기값은 1
	{
		get;
		set;
	} = 1;

	public int LowestConfi	//각 스테이지에서 최소한으로 유지해야할 양심 수치
	{
		get { return LowestConfidence; }	
	}

	public int LowestRepu	//각 스테이지에서 최소한으로 유지해야할 평판 수치
	{
		get { return LowestReputation; }
	}

	public bool CheckClear		//각 스테이지를 클리어 했는지 안했는지 체크
	{
		get;
		set;
	} = true;

	public int CurrentSinnerID	//현재 재판하는 죄인의 ID
	{
		get;
		set;
	} = 0;

	public int CurrentNum	//현재 재판한 죄인의 수
	{
		get { return CheckSinnerNum; }
	}

	public int StageQuata	//현재 스테이지에서 재판해야하는 할당량
	{
		get { return Quota; }
	}

	#endregion

	private Stage(int Width, int Height, char C)
	{
		//높이나 넓이가 0이면 터짐
		Debug.Assert(Width != 0);
		Debug.Assert(Height != 0);

		if(Width<0)
		{
			Width *= -1;
		}

		if(Height<0)
		{
			Height *= -1;
		}

		this.Width = Width;
		this.Height = Height;
		this.C = C;
		this.SinnerList = new List<Sinner>();
		Quota = 0;
		CheckSinnerNum = 0;
	}

	public void Render()	//맵 그리기
	{
		for (int i = 0; i < Width; i++)
		{
			Console.Write(C);
		}

		Console.WriteLine();

		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width * 2; j++)
			{
				if (j == 0 || j == Width * 2 - 3)
				{
					Console.Write(C);
				}

				else
				{
					Console.Write(" ");
				}
			}
			Console.WriteLine();
		}

		for (int i = 0; i < Width; i++)
		{
			Console.Write(C);
		}
	}

	public void GetMapData()	//스테이지 테이블에서 해당 스테이지의 최저 양심 수치,최저 평판 수치,재판 할당량을 받아옴
	{
		if (TableManager.Ins().GetMapData(StageCount, out string Lowest))
		{
			char[] Check = { ' ' };
			string[] Data = Lowest.Split(Check, StringSplitOptions.RemoveEmptyEntries);
			LowestConfidence = Convert.ToInt32(Data[0]);
			LowestReputation = Convert.ToInt32(Data[1]);
			Quota = Convert.ToInt32(Data[2]);
			CheckSinnerNum = 0;	//새롭게 스테이지를 만드므로 재판한 죄인의 수도 0
		}
	}

	public void Add(int ID)		//죄인 테이블에 있는 값을 모두 얻어와 List에 넣음
	{
		Debug.Assert(ID != 0);

		if(ID<0)
		{
			ID *= -1;
		}

		Text T = new Text(string.Empty);
		StatTable Stat = new StatTable(0, 0, 0, 0);

		for (int i = 0; i < 6; i++)
		{
			Sinner New = new Sinner(ID+i);

			if (TableManager.Ins().GetSinnerText(New.SinnerID, out T) && TableManager.Ins().GetSinnerStat(New.SinnerID, out Stat))
			{
				New.Stat = Stat;
				New.Text = T;
				SinnerList.Add(New);
			}
		}
	}

	public Sinner GetSinnerData()	//현재 죄인의 데이터를 얻어옴(현재 죄인 ID를 이용)
	{
		int Index = 0;
		for (int i = 0; i < SinnerList.Count; i++)
		{
			if (SinnerList[i].SinnerID == CurrentSinnerID)
			{
				Index = i;
				break;
			}
		}
		return SinnerList[Index];
	}

	public void RoadNewSinner()	//새로운 죄인 선정
	{
		CurrentSinnerID = DecideSinnerID();

		for (int i = 1; i < 4; i++)	//이미 한 번 재판했던 죄인의 경우 정보가 열려있으므로 다시 닫기
		{
			this.GetSinnerData().Text.Unrock[i] = false;
		}

		return;

		int DecideSinnerID()	//죄인 선정 시 ID를 랜덤으로 설정
		{
			Random R = new Random();
			return 101 + R.Next() % 6;
		}
	}

	public bool QuotaCheck()	//해당 스테이지의 죄인 할당량을 채웠는지 확인
	{
		CheckSinnerNum += 1;
		return CheckSinnerNum == Quota ? true : false;
	}
}

