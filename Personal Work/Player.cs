using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

class Player
{
	int ID;					//플레이어 ID
	int UsaublePoint;		//현재 사용 가능한 열람 포인트
	int CurrentConfidence;	//현재 양심 수치
	int CurrentReputation;	//현재 평판 수치

	int InocenceConfi;		//양심 증가 수치(무죄시)
	int InocenceRepu;		//평판 증가 수치(무죄시)
	int GuiltyConfi;		//양심 증가 수치(유죄시)
	int GuiltyRepu;			//평판 증가 수치(유죄시)

	int DefaultConfi;		//플레이어가 얻을 수 있는 기본 양심 수치
	int DefaultRepu;        //플레이어가 얻을 수 있는 기본 평판 수치
	int DefaultPoint;       //플레이어가 얻을 수 있는 기본 포인트 수치


	#region Property

	public int Point
	{
		get { return UsaublePoint; }
		set { UsaublePoint = value; }
	}

	public int Confidence
	{
		get { return CurrentConfidence; }
		set { CurrentConfidence = value; }
	}

	public int Reputation
	{
		get { return CurrentReputation; }
		set { CurrentReputation = value; }
	}

	#endregion

	public Player(int ID, int Confidence, int Reputation, int Point)
	{
		Debug.Assert(ID > 0);	//ID 값이 0 이하면 터짐

		this.ID = ID;
		this.Confidence = Confidence;
		this.Reputation = Reputation;
		this.Point = Point;
		InocenceConfi = 0;
		InocenceRepu = 0;
		GuiltyConfi = 0;
		GuiltyRepu = 0;
		DefaultConfi = 0;
		DefaultRepu = 0;
		DefaultPoint = 0;
	}

	public void GetPlayerData()
	{
		//플레이어 테이블에서 플레이어 ID로 값을 얻어옴(사용가능 포인트,현재 양심 수치,현재 평판 수치) 
		if (TableManager.Ins().GetPlayerStat(ID, out Player P))
		{
			Point = P.Point;
			CurrentConfidence = P.Confidence;
			Reputation = P.Reputation;
		}
	}

	public void Calc(bool Innocence, bool Guilty)
	{
		//현재 죄인의 스탯값을 얻어옴
		StatTable Temp = Stage.Ins().GetSinnerData().Stat;

		//청중들의 반응에 따라 적용된 수치(얻을 수 있는 양심과 평판 수치가 어느정도 변화하는지)값을 저장
		InocenceConfi = Temp.IConfi;
		InocenceRepu = Temp.IRepu;       
		GuiltyConfi = Temp.GConfi;        
		GuiltyRepu = Temp.GRepu;         

		DefaultRepu = Temp.Reputation;	//죄인의 기본 평판 수치(플레이어가 얻을 수 있는 수치)을 얻어옴
		DefaultConfi = Temp.Confidence;   //죄인의 기본 양심 수치를 얻어옴
		DefaultPoint = Temp.Point;

		//무죄인지 유죄인지 판별한 뒤 양심 수치와 평판 수치를 계산 
		//(플레이어 현재 수치=죄인으로부터 얻을 수 있는 기본+(무죄 판결 시/유죄 판결 시)얻는 수치)

		if(Innocence==true && Guilty==false)	//무죄일 때
		{
			this.Confidence += DefaultConfi+InocenceConfi;
			this.Reputation += DefaultRepu+InocenceRepu;
			this.Point += DefaultPoint;     //플레이어가 얻을 수 있는 포인트를 더함
		}
		
		else if(Innocence==false && Guilty==true)	//유죄일 때
		{
			this.Confidence += DefaultConfi+GuiltyConfi;
			this.Reputation += DefaultRepu+GuiltyRepu;
			this.Point += DefaultPoint;     //플레이어가 얻을 수 있는 포인트를 더함
		}
	}
}

