using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

class TableManager
{
	#region SINGLETONE

	private static TableManager TableInstance = null;

	public static void CreateInstance()
	{
		if (TableInstance == null)
		{
			TableInstance = new TableManager();
		}
	}

	public static TableManager Ins()
	{
		return TableInstance;
	}

	#endregion

	Dictionary<int, Player> PlayerTb = new Dictionary<int, Player>();		//플레이어 데이터 테이블
	Dictionary<int, StatTable> SinnerTb = new Dictionary<int, StatTable>();	//죄인 스탯 테이블 
	Dictionary<int, Text> SinnerTextTb = new Dictionary<int, Text>();		//죄인 정보 테이블
	Dictionary<int, string> MapTb = new Dictionary<int, string>();			//각 스테이지 정보 테이블

	private TableManager()
	{
		//텍스트 읽어와서 작업
		string[] SinnerData = File.ReadAllLines(@"..\..\SinnerData.txt");
		string SinnerStory = File.ReadAllText(@"..\..\SinnerStory.txt", Encoding.Default);
		string PlayerData = File.ReadAllText(@"..\..\PlayerData.txt");
		string[] MapData = File.ReadAllLines(@"..\..\StageData.txt");

		//죄인
		char[] Check = { ',' };
		for (int i = 0; i < SinnerData.Length; i++)
		{
			string[] Data = SinnerData[i].Split(Check, StringSplitOptions.RemoveEmptyEntries);
			SinnerTb.Add(Convert.ToInt32(Data[0]), new StatTable(Convert.ToInt32(Data[1]), Convert.ToInt32(Data[2]), Convert.ToInt32(Data[3]), (SinType)Convert.ToInt32(Data[4])));
		}

		//죄인 정보
		char[] Check2 = { '(', '★' };
		string[] Data2 = SinnerStory.Split(Check2, StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < Data2.Length / 2; i++)
		{
			SinnerTextTb.Add(Convert.ToInt32(Data2[2 * i]), new Text(Data2[2 * i + 1]));
		}

		//플레이어 정보
		string[] Data3 = PlayerData.Split(Check, StringSplitOptions.RemoveEmptyEntries);
		PlayerTb.Add(Convert.ToInt32(Data3[0]), new Player(Convert.ToInt32(Data3[0]), Convert.ToInt32(Data3[1]), Convert.ToInt32(Data3[2]), Convert.ToInt32(Data3[3])));

		//맵정보
		for (int i = 0; i < MapData.Length; i++)
		{
			string[] Data4 = MapData[i].Split(Check, StringSplitOptions.RemoveEmptyEntries);
			MapTb.Add(Convert.ToInt32(Data4[0]), Data4[1]);
		}
	}

	//죄인 스탯을 ID를 통해서 줌
	public bool GetSinnerStat(int ID, out StatTable Stat)
	{
		return SinnerTb.TryGetValue(ID, out Stat);
	}

	//플레이어 스탯을 ID를 통해서 줌
	public bool GetPlayerStat(int ID, out Player Stat)
	{
		return PlayerTb.TryGetValue(ID, out Stat);
	}

	//죄인 정보를 ID를 통해서 줌
	public bool GetSinnerText(int ID, out Text Text)
	{
		return SinnerTextTb.TryGetValue(ID, out Text);
	}

	//맵(스테이지)정보를 스테이지 카운트(현재 스테이지 단계)를 통해서 줌
	public bool GetMapData(int StageNum, out string LowestValue)
	{
		return MapTb.TryGetValue(StageNum, out LowestValue);
	}
}

