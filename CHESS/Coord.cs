using System;

namespace CHESS
{
	public struct Coord
	{	

		public Coord(int xPos, int yPos)
		{
			_Xpos = xPos;
			_Ypos = yPos;
		}

		public int xpos
		{
			get { return _Xpos; }
		}
		public int ypos
		{
			get { return _Ypos; }
		}

		private int _Xpos;
		private int _Ypos;



	}
}

