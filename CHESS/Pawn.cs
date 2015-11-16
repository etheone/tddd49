using System;

namespace CHESS
{
	public class Pawn : Piece
	{
		//public bool moved;
		public Pawn (PieceType t, int x, int y, PieceColor c, int tf) : base(t, x, y, c, tf) 
		{
			moved = false;
		}


	}
}

