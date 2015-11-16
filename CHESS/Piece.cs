using System;

namespace CHESS
{
	public class Piece
	{
		public enum PieceColor 
		{
			WHITE,
			BLACK,
			NONE
		}

		public enum PieceType 
		{
			PAWN,
			ROOK,
			KNIGHT,
			BISHOP,
			QUEEN,
			KING,
			NONE
		}

		public Coord coord;
		public bool moved;
		//public  moved;
		//protected string color;
		public PieceColor color;
		public PieceType type;
		public int textureRef;

		public Piece (PieceType p, int x, int y, PieceColor c, int tf)
		{
			type = p;
			coord = new Coord (x, y);
			color = c;
			moved = false;
			textureRef = tf;
		}
	}
}

