using System;

namespace CHESS
{
	public class EmptyPiece : Piece
	{
		public EmptyPiece (PieceType t, int x, int y, PieceColor c) : base(t, x, y, c, 0) {}
	}
}

