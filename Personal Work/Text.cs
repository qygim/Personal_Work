using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

class Text
{
	//주석의 설명이 이해가 가지 않으면 파일에서 sinnerstory.txt를 보면 됨.

	string TextArray;		//테이블에서 sinnerID를 빼고 받아올 이야기 저장 변수
	string[] Information;	//테이블에서 받아온 텍스트에서 "정보"의 위치를 찾기 위해 지정할 문자열 
	char[] Check;
	int[] InformationIndex;	//"정보"라고 써져있는 글 위치를 저장함
	int[] InformationType;  //"정보"내용의 타입을 저장함
	bool[] UnrockInfo;

	public bool[] Unrock	//해당 정보가 열람을 했는지 안했는지 저장함
	{
		get { return UnrockInfo; }
		set { UnrockInfo = value; }
	}

	public Text(string Data)
	{
		Information = new string[4] { "정보1", "정보2", "정보3", "정보4" };
		Check = new char[1] { '\n' };
		TextArray = Data;
		InformationIndex = new int[5];
		InformationType = new int[4];
		Unrock = new bool[4];

		for(int i=0;i<4;i++)
		{
			Unrock[i] = false;	//처음 죄인 정보를 줄 때 정보1말고는 주지 않기때문에 다 false로 설정
		}

		Unrock[0] = true;	//정보1만 보여주게 1만 true로 설정
	}

	public void TextShow(int X, int Y)
	{
		//전체로 읽어온 죄인 정보를 줄 단위로 나눔
		string[] Story = TextArray.Split(Check, StringSplitOptions.RemoveEmptyEntries);

		//정보1,정보2,정보3,정보4라고 써진 행의 위치를 찾아 저장
		FindInformationIndex(Story, Information, InformationIndex);

		//정보1,정보2,정보3,정보4의 정보 타입을 저장
		FindInformationType(InformationType, Story);

		Console.SetCursorPosition(X, Y);	//죄명 출력
		Console.WriteLine(Story[0]);

		//열람한 정보들만 출력
		for(int i=0;i<4;i++)
		{
			if(Unrock[i])
			{
				for(int j=InformationIndex[i];j<InformationIndex[i+1];j++)
				{
					Console.SetCursorPosition(X, Y + j+(i*1));
					Console.WriteLine(Story[j]);
				}
			}
		}

		return;

		void FindInformationIndex(string[] S, string[] Check, int[] Index)
		{
			for (int i = 0; i < Check.Length; i++)
			{
				Console.SetCursorPosition(X, Y + i);
				for (int j = 0; j < Story.Length; j++)
				{
					Console.SetCursorPosition(X, Y + j);
					if ((Story[j].Contains(Check[i])))
					{
						Index[i] = j;
					}
				}
			}
			Index[Index.Length - 1] = S.Length;
		}

		void FindInformationType(int[] TypeIndex,string[] S)
		{
			char[] Check = { ' ' };
			string TypeInfo = S[1];
			string[] Index = TypeInfo.Split(Check, StringSplitOptions.RemoveEmptyEntries);
			for(int i=0;i<TypeIndex.Length;i++)
			{
				TypeIndex[i] = Convert.ToInt32(Index[i]);
			}
		}
	}

	//정보를 열람할 때마다 해당 정보 타입에 따라 청중들의 반응이 달라져서 해당 정보 타입을 넘겨주기 위한 함수
	public InformationType InfoType(int Num)
	{
		//Num은 2,3,4(정보2,정보3,정보4)로 들어오고 Index로는 1,2,3이여서 NUM-1값을 넘겨줌
		return (InformationType)InformationType[Num-1];
	}
}

