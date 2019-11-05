using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

class Sinner
{
	int ID;
	StatTable SinnerStat;	//죄인의 기본 스탯을 저장할 클래스
	Text SinnerText;		//죄인의 정보(이야기)를 저장할 클래스

	#region Property

	public int SinnerID
	{
		get { return ID; }
	}

	public Text Text
	{
		get { return SinnerText; }
		set { SinnerText = value; }
	}

	public StatTable Stat
	{
		get { return SinnerStat; }
		set { SinnerStat = value; }
	}

	#endregion

	public Sinner(int ID)
	{
		Debug.Assert(ID != 0);

		if(ID<0)
		{
			ID *= -1;
		}

		this.ID = ID;
	}

	public void ShowSinnerStory(int X, int Y)
	{
		SinnerText.TextShow(X, Y);	//해당 죄인의 정보를 text클래스로부터 보여줌
	}
}

