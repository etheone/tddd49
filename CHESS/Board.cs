using System;

namespace CHESS
{
	public class Board
	{
		//Player player1;
		//Player player2;
		public Piece[,] board
		{
			get {
				return _board;
			}
			set {
				_board = Board.newBoard ();
			}
		}
		private Piece[,] _board;


		public Board ()
		{
			System.Console.WriteLine ("Hello");
			board = Board.newBoard ();
			for (int i = 0; i < board.GetLength(0); i++)
			{
				for (int j = 0; j < board.GetLength(1); j++)
				{
					//string s = board[i, j];
					System.Console.Write (board[j,i].GetType() );
					System.Console.Write ("   ");
					//if (board[i, j] is Rook) {
					//	Console.WriteLine("It's a Rook!");
					//}

					//Console.WriteLine(s);
				}
				System.Console.WriteLine ("\n");
			}
		}

		private static Piece[,] newBoard()
		{
			Piece[,] board = new Piece[8,8];
			for (int i = 0; i < 8; i++) {
				board[i,1] = new Piece(Piece.PieceType.PAWN, i, 1, Piece.PieceColor.BLACK, 11);
			}
			board [0, 0] = new Piece (Piece.PieceType.ROOK, 0, 0, Piece.PieceColor.BLACK, 8);
			board [7,0] = new Piece (Piece.PieceType.ROOK, 7, 0, Piece.PieceColor.BLACK, 8);
			board [1,0] = new Piece (Piece.PieceType.KNIGHT, 1, 0, Piece.PieceColor.BLACK, 10);
			board [6, 0] = new Piece (Piece.PieceType.KNIGHT, 6, 0, Piece.PieceColor.BLACK, 10);
			board [2, 0] = new Piece (Piece.PieceType.BISHOP, 2, 0, Piece.PieceColor.BLACK, 9);
			board [5, 0] = new Piece (Piece.PieceType.BISHOP, 5, 0, Piece.PieceColor.BLACK, 9);
			board [4, 0] = new Piece (Piece.PieceType.KING, 4, 0, Piece.PieceColor.BLACK, 6);
			board [3, 0] = new Piece (Piece.PieceType.QUEEN, 3, 0, Piece.PieceColor.BLACK, 7);
			for (int i = 0; i < 8; i++) {
				board[i,6] = new Piece(Piece.PieceType.PAWN, i, 6, Piece.PieceColor.WHITE, 5);
			}
			board [0, 7] = new Piece (Piece.PieceType.ROOK, 0, 7, Piece.PieceColor.WHITE, 2);
			board [7, 7] = new Piece (Piece.PieceType.ROOK, 7, 7, Piece.PieceColor.WHITE, 2);
			board [1, 7] = new Piece (Piece.PieceType.KNIGHT, 1, 7, Piece.PieceColor.WHITE, 4);
			board [6, 7] = new Piece (Piece.PieceType.KNIGHT, 6, 7, Piece.PieceColor.WHITE, 4);
			board [2, 7] = new Piece (Piece.PieceType.BISHOP, 2, 7, Piece.PieceColor.WHITE, 3);
			board [5, 7] = new Piece (Piece.PieceType.BISHOP, 5, 7, Piece.PieceColor.WHITE, 3);
			board [4, 7] = new Piece (Piece.PieceType.KING, 4, 7, Piece.PieceColor.WHITE, 0);
			board [3, 7] = new Piece (Piece.PieceType.QUEEN, 3, 7, Piece.PieceColor.WHITE, 1);

			for (int i = 2; i < 6; i++) {
				for (int j = 0; j < 8; j++) {
					board [j, i] = new Piece(Piece.PieceType.NONE, i, j, Piece.PieceColor.NONE, 99);
				}
			}

			return board;
		}

		public void tryMove(Coord fromPos, Coord toPos) {
			Console.WriteLine ("In tryMove-funcition");
			Console.WriteLine ("From x: " + fromPos.xpos + " y: " + fromPos.ypos);
			Console.WriteLine ("To x: " + toPos.xpos + " y: " + toPos.ypos);

			if ( Rules.isLegalMove (board, fromPos, toPos) ) {
				board[fromPos.xpos, fromPos.ypos].moved = true;
				swap (fromPos, toPos);
				Console.WriteLine ("Here we should make the move.");
			}
		}

		private void swap(Coord fromPos, Coord toPos) 
		{
			Piece temp = board [fromPos.xpos, fromPos.ypos];
			temp.coord = toPos;
			if (board [toPos.xpos, toPos.ypos].type != Piece.PieceType.NONE) {
				board [toPos.xpos, toPos.ypos] = new Piece (Piece.PieceType.NONE, toPos.xpos, toPos.ypos, Piece.PieceColor.NONE, 99);
			}
			board [toPos.xpos, toPos.ypos].coord = fromPos;
			board [fromPos.xpos, fromPos.ypos] = board [toPos.xpos, toPos.ypos];
			board [toPos.xpos, toPos.ypos] = temp;

		}

	}
}

