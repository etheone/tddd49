using System;
using System.Collections.Generic;
using System.Linq;

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
				//Console.WriteLine ("Pawn");
				return Pawn (board, fromPos, toPos);
				

			case Piece.PieceType.ROOK:
				//Console.WriteLine ("Rook");
				return Rook (board, fromPos, toPos);
				

			case Piece.PieceType.KNIGHT:
				//Console.WriteLine ("Knight");
				return Knight (board, fromPos, toPos);
				

			case Piece.PieceType.BISHOP:
				//Console.WriteLine ("Bishop");
				return Bishop (board, fromPos, toPos);
				

			case Piece.PieceType.QUEEN:
				//Console.WriteLine ("Queen");
				return Queen (board, fromPos, toPos);
				

			case Piece.PieceType.KING:
				//Console.WriteLine ("King");
				return King (board, fromPos, toPos);
			
			}
			return false;
		}	

		//Pawn
		public static bool Pawn(Piece[,] board, Coord fromPos, Coord toPos)
		{
			int direction;
			//Console.WriteLine ("IN PAWN FUNC");
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
				//Console.WriteLine ("In first IF");
				if (diffInY == direction) {
					//Console.WriteLine ("In second IF");
					//Console.WriteLine (board [fromPos.xpos, fromPos.ypos].type);
					//Console.WriteLine(board [toPos.xpos, toPos.ypos].type);
					if (board [fromPos.xpos, fromPos.ypos].color != board [toPos.xpos, toPos.ypos].color && board [toPos.xpos, toPos.ypos].type != Piece.PieceType.NONE) {
						//Console.WriteLine ("In third IF");
						//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
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
					//Console.WriteLine ("LEGAL MOVE");
					//board[fromPos.xpos, fromPos.ypos].moved = true;
					//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
					return true;
				} else if(board[fromPos.xpos,fromPos.ypos].moved == false && toPos.ypos - fromPos.ypos == (direction + direction)) {
					//Console.WriteLine ("LEGAL MOVE");
					//board[fromPos.xpos, fromPos.ypos].moved = true;
					//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
					return true;
				} else {
					//Console.WriteLine ("ILLEGAL MOVE");
				}
			}

			///////////////////////////////////////////////
			/// Handles diagonal movements ////////////////
			///////////////////////////////////////////////

			return false;
		}

		//Rook stuff
		private static bool Rook(Piece[,] board, Coord fromPos, Coord toPos)
		{
			if (fromPos.xpos == toPos.xpos && fromPos.ypos == toPos.ypos) {
				//Console.WriteLine ("This happend");
				return false;
			}

			if (isPathNotBlocked (board, fromPos, toPos)) {

				if (isStraightMove(fromPos, toPos))
				{
					if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
						//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
						return true;
					}
				}
			}

			return false;
		}

		//Knight stuff
		private static bool Knight(Piece[,] board, Coord fromPos, Coord toPos)
		{
			int diffInX = toPos.xpos - fromPos.xpos;
			int diffInY = toPos.ypos - fromPos.ypos;

			if (Math.Abs (diffInX) == 1 && Math.Abs (diffInY) == 2) {
				if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
					//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
					return true;
				}
			}

			if (Math.Abs (diffInX) == 2 && Math.Abs (diffInY) == 1) {
				if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
					//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
					return true;
				}
			}

			return false;
		}

		//Bishop stuff
		private static bool Bishop(Piece[,] board, Coord fromPos, Coord toPos)
		{
			if (fromPos.xpos == toPos.xpos && fromPos.ypos == toPos.ypos) {
				//Console.WriteLine ("This happend");
				return false;
			}

			if (isPathNotBlocked (board, fromPos, toPos)) {

				if (isDiagonalMove(fromPos, toPos))
				{
					if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
						//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
						return true;
					}
				}

			}

			return false;
		}

		//Queen stuff
		private static bool Queen(Piece[,] board, Coord fromPos, Coord toPos) 
		{
			if (fromPos.xpos == toPos.xpos && fromPos.ypos == toPos.ypos) {
				//Console.WriteLine ("This happend");
				return false;
			}

			if (isPathNotBlocked (board, fromPos, toPos)) {

				if (isStraightMove(fromPos, toPos) || isDiagonalMove(fromPos, toPos))
				{
					if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
						//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
						return true;
					}
				}



			}



			return false;

		}

		private static bool King(Piece[,] board, Coord fromPos, Coord toPos)
		{

			int diffInX = toPos.xpos - fromPos.xpos;
			int diffInY = toPos.ypos - fromPos.ypos;

			if (Math.Abs (diffInX) == 1 && Math.Abs (diffInY) == 0) {
				if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
					//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
					return true;
				}
			}

			if (Math.Abs (diffInX) == 0 && Math.Abs (diffInY) == 1) {
				if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
					//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
					return true;
				}
			}

			if (Math.Abs (diffInX) == 1 && Math.Abs (diffInY) == 1) {
				if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
					//return checkIfCheck (board, board [fromPos.xpos, fromPos.ypos].color, fromPos, toPos);
					return true;
				}
			}

			return false;

		}

		private static bool isPathNotBlocked(Piece[,] board, Coord fromPos, Coord toPos)
		{
			int diffInX = toPos.xpos - fromPos.xpos;
			int diffInY = toPos.ypos - fromPos.ypos;
			int steps;

			if (diffInX == 0) {
				//Console.WriteLine ("THERE IS A MOVE IN Y");
				if (diffInY < 0) {
					steps = -1;
				} else {
					steps = 1;
				}

				for (int i = steps; i != diffInY; i += steps) {
					if (board [fromPos.xpos, fromPos.ypos + i].type != Piece.PieceType.NONE) {
						return false;
					}
				}

				return true;
			}

			if (diffInY == 0) {
				//Console.WriteLine ("THERE IS A MOVE IN X");
				if (diffInX < 0) {
					steps = -1;
				} else {
					steps = 1;
				}

				for (int i = steps; i != diffInX; i += steps) {
					if (board [fromPos.xpos + i, fromPos.ypos].type != Piece.PieceType.NONE) {
						return false;
					}
				}

				return true;
			}

			int stepsX = diffInX / Math.Abs (diffInX);
			int stepsY = diffInY / Math.Abs (diffInY);
			int currX = fromPos.xpos + stepsX;
			int currY = fromPos.ypos + stepsY;
			//Console.WriteLine ("THERE IS A SNEMOVE");
			if (diffInX != 0 && diffInY != 0) {
				while (currX != toPos.xpos && currY != toPos.ypos) {

					if (board [currX, currY].type != Piece.PieceType.NONE) {

						return false;
					}

					currX += stepsX;
					currY += stepsY;
				}
			}

			return true;

		}

		private static bool isDiagonalMove(Coord fromPos, Coord toPos) {

			int diffInX = toPos.xpos - fromPos.xpos;
			int diffInY = toPos.ypos - fromPos.ypos;

			return Math.Abs (diffInX) == Math.Abs (diffInY);

		}

		private static bool isStraightMove(Coord fromPos, Coord toPos) {

			int diffInX = toPos.xpos - fromPos.xpos;
			int diffInY = toPos.ypos - fromPos.ypos;

			if (diffInX == 0 || diffInY == 0)
				return true;

			return false;

		}

		public static List<Coord> getLegalMoves(Piece[,] board, Coord fromPos) {

			List<Coord> temp = new List<Coord>();
			Coord tempCord;

			for (int i = 0; i < board.GetLength(0); i++) {
				for (int j = 0; j < board.GetLength(0); j++) {
					tempCord = new Coord (i, j);
					//tempCord.xpos = i;
					//tempCord.ypos = j;
					if (isLegalMove (board, fromPos, tempCord)) {
						temp.Add (tempCord);
						//Console.WriteLine (i + " , " + j + " is a valid move");
					}
				}
			}

			return temp;
		}

		public static bool isCheck(Piece[,] board, Piece.PieceColor color, Coord kingCoord) {

			List<Coord> possibleMoves = new List<Coord> ();

			foreach (Piece p in board) {
				if (p.color == color) {
					Console.WriteLine (p.color);
					Console.WriteLine (color);
					possibleMoves = getLegalMoves (board, p.coord);
					foreach (Coord c in possibleMoves) {
						if (c.xpos == kingCoord.xpos && c.ypos == kingCoord.ypos) {
							return true;
						}
					}
				}
			}

			return false;
		}
		/*
		//Creates a temporary board with the next move and investigates if it would put colors own king in check.
		public static bool checkIfCheck(Piece[,] board, Piece.PieceColor color, Coord fromPos, Coord toPos) {

			Piece[,] tempBoard = new Piece[8,8];
			tempBoard = (Piece[,])board.Clone ();

			Piece temp = tempBoard [fromPos.xpos, fromPos.ypos];
			temp.coord = toPos;
			if (tempBoard [toPos.xpos, toPos.ypos].type != Piece.PieceType.NONE) {
				tempBoard [toPos.xpos, toPos.ypos] = new Piece (Piece.PieceType.NONE, toPos.xpos, toPos.ypos, Piece.PieceColor.NONE, 99);
			}
			tempBoard [toPos.xpos, toPos.ypos].coord = fromPos;
			tempBoard [fromPos.xpos, fromPos.ypos] = tempBoard [toPos.xpos, toPos.ypos];
			tempBoard [toPos.xpos, toPos.ypos] = temp;

			Console.WriteLine ("-------------------------------BEGIN-----------------------_");

			for (int i = 0; i < board.GetLength(0); i++)
			{
				for (int j = 0; j < board.GetLength(1); j++)
				{
					//string s = board[i, j];
					System.Console.Write (board[j,i].type );
					System.Console.Write ("   ");
					//if (board[i, j] is Rook) {
					//	Console.WriteLine("It's a Rook!");
					//}

					//Console.WriteLine(s);
				}
				System.Console.WriteLine ("\n");
			}

			Coord kingCoord = new Coord ();

			Console.WriteLine ("-------------------------------BETWEEN----------------------_");

			for (int i = 0; i < tempBoard.GetLength(0); i++)
			{
				for (int j = 0; j < tempBoard.GetLength(1); j++)
				{
					//string s = board[i, j];
					System.Console.Write (tempBoard[j, i].type );
					System.Console.Write ("   ");
					//if (board[i, j] is Rook) {
					//	Console.WriteLine("It's a Rook!");
					//}
					if (tempBoard [j, i].type == Piece.PieceType.KING && tempBoard [j, i].color == color) {
						kingCoord = tempBoard [j, i].coord;
					}
					//Console.WriteLine(s);
				}
				System.Console.WriteLine ("\n");
			}

			Console.WriteLine ("-------------------------------END-----------------------_");





			return isCheck (tempBoard, color, kingCoord);
		}*/

		public static bool isCheckMate(Piece[,] board, Piece.PieceColor color) {

			bool stillCheck = true;
			List<Coord> currPieceLegalMoves = new List<Coord> ();
			Coord kingCoord = new Coord ();
			for (int y = 0; y < board.GetLength(0); y++) {
				for (int x = 0; x < board.GetLength(0); x++) {
					if (board [x, y].color == color && stillCheck) {
						currPieceLegalMoves = getLegalMoves (board, board [x, y].coord);
						foreach (Coord c in currPieceLegalMoves) {

							Piece[,] tempBoard = new Piece[8,8];
							tempBoard = (Piece[,])board.Clone ();

							Piece temp = tempBoard [x, y];
							//temp.coord = toPos;
							if (tempBoard [c.xpos, c.ypos].type != Piece.PieceType.NONE) {
								tempBoard [c.xpos, c.ypos] = new Piece (Piece.PieceType.NONE, c.xpos, c.ypos, Piece.PieceColor.NONE, 99);
							}
							tempBoard [c.xpos, c.ypos].coord = new Coord(x,y);
							tempBoard [x, y] = tempBoard [c.xpos, c.ypos];
							tempBoard [c.xpos, c.ypos] = temp;

							foreach (Piece p in tempBoard) {
								if (p.type == Piece.PieceType.KING && p.color == color) {
									kingCoord = new Coord (p.coord.xpos, p.coord.ypos);
									Console.WriteLine (kingCoord.xpos + " + " + kingCoord.ypos);
								}

							}

							if (color == Piece.PieceColor.WHITE) {
								stillCheck = isCheck (tempBoard, Piece.PieceColor.BLACK, kingCoord);
								if (stillCheck == false) {
									Console.WriteLine ("Piece: " + board [x, y].type + " at coord: " + x + ", " + y + " moved to: " + c.xpos + ", " + c.ypos + " will uncheck the check");
									return false;
								}
								//stillCheck = isCheck(tempBoard, Piece.PieceColor.BLACK, kingCoord);

							} else {
								stillCheck = isCheck (tempBoard, Piece.PieceColor.WHITE, kingCoord);
								if (stillCheck == false) {
									Console.WriteLine ("Piece: " + board [x, y].type + " at coord: " + x + ", " + y + " moved to: " + c.xpos + ", " + c.ypos + " will uncheck the check");
									return false;
								};
								//stillCheck = isCheck (tempBoard, Piece.PieceColor.WHITE, kingCoord);
							}

						}
					}

				}
			}

			return true;
		}


		//Might be nice to have a seperate function for this but could also use the isCheck.... probably use the existing.
		public static bool unChecked(Piece[,] board, Piece.PieceColor color, Coord kingCoord) {
			return true;
		}



	}
}

