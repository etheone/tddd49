using System;

namespace CHESS
{
	public class Rules
	{

		public Rules ()
		{

		}



		public static bool isLegalMove(Piece[,] board, Coord fromPos, Coord toPos)
		{
			//Console.WriteLine (piece);
			//string pieceType = (piece.GetType ()).ToString ();
			//Console.WriteLine (pieceType);
			//Console.WriteLine (pieceType.GetType ());
			//pseudo
			//string caseSwitch = piece.GetType.toString ();
			Piece.PieceType curType = board [fromPos.xpos, fromPos.ypos].type;
			switch (curType) {
			case Piece.PieceType.PAWN:
				Console.WriteLine ("Pawn");
				return Pawn (board, fromPos, toPos);
				break;

			case Piece.PieceType.ROOK:
				Console.WriteLine ("Rook");
				break;

			case Piece.PieceType.KNIGHT:
				Console.WriteLine ("Knight");
				break;

			case Piece.PieceType.BISHOP:
				Console.WriteLine ("Bishop");
				break;

			case Piece.PieceType.QUEEN:
				Console.WriteLine ("Queen");
				break;

			case Piece.PieceType.KING:
				Console.WriteLine ("King");
				break;

					
			

			}
			return false;
		}	

		//Pawn
		public static bool Pawn(Piece[,] board, Coord fromPos, Coord toPos)
		{
			int direction;
			Console.WriteLine ("IN PAWN FUNC");
			int diffInX = fromPos.xpos - toPos.xpos;
			int diffInY = toPos.ypos - fromPos.ypos;
			if (board[fromPos.xpos,fromPos.ypos].color == Piece.PieceColor.WHITE) {
				direction = -1;
			} else {
				direction = 1;
			}

			///////////////////////////////////////////////
			/// Handles diagonal movements aka killmoves///
			///////////////////////////////////////////////
			//if(Math.Abs(diffInX) == 1) 
			if (diffInX == 1 || diffInX == -1) {
				Console.WriteLine ("In first IF");
				if (diffInY == direction) {
					Console.WriteLine ("In second IF");
					Console.WriteLine (board [fromPos.xpos, fromPos.ypos].type);
					Console.WriteLine(board [toPos.xpos, toPos.ypos].type);
					if (board [fromPos.xpos, fromPos.ypos].color != board [toPos.xpos, toPos.ypos].color && board [toPos.xpos, toPos.ypos].type != Piece.PieceType.NONE) {
						Console.WriteLine ("In third IF");
						return true;
					}
				}
				return false;
			}

			/////////////////////////////////////////////////////
			/// Handles vertical movements //////////////////////
			/////////////////////////////////////////////////////
			if (board [fromPos.xpos, fromPos.ypos + direction].type != Piece.PieceType.NONE) {
				return false;
			} 
			if(toPos.ypos - fromPos.ypos == direction * 2 && board [fromPos.xpos, fromPos.ypos + direction * 2].type != Piece.PieceType.NONE) {
				return false;
			}

			//Console.WriteLine ("Move to 0: " + moveTo [0] + " Piece.col: " + piece.col);

			if (fromPos.xpos - toPos.xpos == 0) {
				if (toPos.ypos - fromPos.ypos == direction) {
					Console.WriteLine ("LEGAL MOVE");
					//board[fromPos.xpos, fromPos.ypos].moved = true;
					return true;
				} else if(board[fromPos.xpos,fromPos.ypos].moved == false && toPos.ypos - fromPos.ypos == (direction + direction)) {
					Console.WriteLine ("LEGAL MOVE");
					//board[fromPos.xpos, fromPos.ypos].moved = true;
					return true;
				} else {
					Console.WriteLine ("ILLEGAL MOVE");
				}
			}

			///////////////////////////////////////////////
			/// Handles diagonal movements ////////////////
			///////////////////////////////////////////////

			return false;
		}

		//Rook stuff
		public void Rook(Board board)
		{

		}

		//Knight stuff
		public void Knight(Board board)
		{

		}

		//Bishop stuff
		public void Bishop(Board board)
		{

		}

		//Queen stuff
		public void Queen(Board board) 
		{

		}

		public void King(Board board)
		{

		}

	}
}

