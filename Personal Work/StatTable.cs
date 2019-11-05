using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class StatTable
{
	//여기서 양심,평판은 해당 죄인의 양심,평판이 아니라 플레이어가 얻을 수 있는 정도임.헷갈리지 않아도 됨
	//증가 감소 수치도 플레이어가 얻을 수 있는 기본 수치에서 더하고 빼기하려는 것 뿐

	int ConfidenceDegree;	//플레이어가 얻을 수 있는 양심 수치
	int ReputationDegree;	//플레이어가 얻을 수 있는 평판 수치
	int ObtainablePoint;	//플레이어가 얻을 수 있는 열람 포인트 수치
	SinType SType;			//죄인의 죄형

	int InocenceConfidence;	//양심 증가 수치(무죄시)
	int InocenceReputation; //평판 증가 수치(무죄시)
	int GuiltyConfidence; //양심 증가 수치(유죄시)
	int GuiltyReputation; //평판 증가 수치(유죄시)

	#region Property

	public int Confidence
	{
		get { return ConfidenceDegree; }
	}

	public int Reputation
	{
		get { return ReputationDegree; }
	}

	public int Point
	{
		get { return ObtainablePoint; }
	}

	public SinType Sin
	{
		get { return SType; }
	}

	public int IConfi
	{
		get { return InocenceConfidence; }
	}

	public int IRepu
	{
		get { return InocenceReputation; }
	}

	public int GConfi
	{
		get { return GuiltyConfidence; }
	}

	public int GRepu
	{
		get { return GuiltyReputation; }
	}

	#endregion

	public StatTable(int ConfidenceDegree, int ReputationDegree, int ObtainablePoint, SinType SType)
	{
		this.ConfidenceDegree = ConfidenceDegree;
		this.ReputationDegree = ReputationDegree;
		this.ObtainablePoint = ObtainablePoint;
		this.SType = SType;
		InocenceConfidence = 0;
		InocenceReputation = 0;
		GuiltyConfidence = 0;
		GuiltyReputation = 0;
	}

	//정보를 새롭게 열람할 때마다 증가 감소하는 양심,평판 수치를 새롭게 설정
	public void Adjustment(int InocenceConfidence, int InocenceReputation, int GuiltyConfidence, int GuiltyReputation)
	{
		this.InocenceConfidence = InocenceConfidence;
		this.InocenceReputation = InocenceReputation;
		this.GuiltyConfidence = GuiltyConfidence;
		this.GuiltyReputation = GuiltyReputation;
	}
}

