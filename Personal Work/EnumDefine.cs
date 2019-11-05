using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//죄인들 죄형
public enum SinType
{
	None,
	Fire, //방화
	Robbery, //강도
	Accident, //사고
	Wound, //상해
	Violence, //폭행
	Murder, //살인
	Max,
}

//청중들의 상태
public enum AudienceState
{
	None,
	Anger,      //분노
	Discomfort, //불쾌
	Regret,     //안타까움
	Angrevate,  //극도로 화난
	Max,
}

//죄인들에 대한 각 정보 타입
public enum InformationType
{
	None,
	Normal,	   //평범
	Regret,	   //안타까움
	Malice,	   //악의
	Discomfort,//불쾌
	Max,
}
