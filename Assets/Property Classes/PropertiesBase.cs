using System;

namespace SuddenlyEntertainment
{
	public class PropertiesBase
	{
		protected string _name;
		public string Name { get { return _name; } }

		public PropertiesBase ()
		{
			_name = "TestProperties";
		}
	}
}

