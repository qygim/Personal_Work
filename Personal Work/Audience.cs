using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Audience
{
	AudienceState State;	//청중들 상태
	bool SinCheck;			//처음 죄인 정보나 나왔을 때 정보1만으로 청중들 상태를 나타내기 위한 체크 변수

	#region Property

	public AudienceState AudienceState
	{
		get { return State; }
	}

	//처음 죄인 정보를 보여줄 때 죄명만으로 청중 상태를 정하기 위한 변수
	//정보가 열람되면 false로 바뀐다.
	public bool Check
	{
		get { return SinCheck; }
	}

	#endregion

	public Audience()
	{
		State = AudienceState.None;
		SinCheck = true;
	}
	
	//정보가 열람 될 때마다 바뀌는 청중들의 상태
	public void AudienceBehavior()
	{
		switch (State)
		{
			case AudienceState.None:
				Console.WriteLine("청중들은 별 관심이 없어보입니다");
				break;
			case AudienceState.Anger:
				Console.WriteLine("청중들이 죄인에게 분노합니다");
				break;
			case AudienceState.Discomfort:
				Console.WriteLine("청중들이 죄인을 불쾌해합니다");
				break;
			case AudienceState.Regret:
				Console.WriteLine("청중들은 죄인을 살짝 동정합니다");
				break;
			case AudienceState.Angrevate:
				Console.WriteLine("청중들은 당장 처벌하길 원합니다");
				break;
			default:
				break;
		}
	}

	//처음 정보1만 줄 때 죄명만으로 청중들의 상태 결정(다른 정보가 열리면 실행되지 않는다)
	public void SetState(int ID)
	{
		SinType Sin = Stage.Ins().GetSinnerData().Stat.Sin;

		switch (Sin)
		{
			case SinType.Fire:
			case SinType.Murder:
			case SinType.Accident:
				State = AudienceState.Anger;
				break;

			case SinType.Robbery:
			case SinType.Violence:
			case SinType.Wound:
				State = AudienceState.Discomfort;
				break;

			default:
				Console.WriteLine("State Error");
				break;
		}
	}

	//정보가 열람 될 때마다 해당 정보 타입에 대한 청중들의 상태 변화
	//어느정도 변할지가 너무 어렵다...
	public void ChangeState(InformationType Type)
	{
		SinCheck = false;

		switch (Type)
		{
			case InformationType.Regret:
				State = AudienceState.Regret;
				Stage.Ins().GetSinnerData().Stat.Adjustment(20, 0, 0, -20);
				break;

			case InformationType.Malice:
				State = AudienceState.Angrevate;
				Stage.Ins().GetSinnerData().Stat.Adjustment(0,0,0,20);
				break;

			case InformationType.Discomfort:
				State = AudienceState.Discomfort;
				Stage.Ins().GetSinnerData().Stat.Adjustment(0, 0, 10, 10);
				break;
			default:
				break;
		}
	}
}

