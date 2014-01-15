//  	File Name			:	EventOnUnitDeathArgs.cs
//
// 	Created By		:	robo
// 	Created On		:	1/15/2014
//
// 	Purpose			:	<${Purpose}>
using System;

namespace AnotherExpieramentalProject
{
	public class OnUnitDeathEventArgs : EventArgs
	{
		public string UnitName;
		public double goldGained;
		public double expierenceGained;
		public int Level;
	}
}

