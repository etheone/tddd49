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
			Piece.PieceType curType = board [fromPos.xpos, fromPos.ypos].type;
			switch (curType) {
			case Piece.PieceType.PAWN:
				return Pawn (board, fromPos, toPos);

			case Piece.PieceType.ROOK:
				return Rook (board, fromPos, toPos);

			case Piece.PieceType.KNIGHT:
				return Knight (board, fromPos, toPos);

			case Piece.PieceType.BISHOP:
				return Bishop (board, fromPos, toPos);

			case Piece.PieceType.QUEEN:
				return Queen (board, fromPos, toPos);

			case Piece.PieceType.KING:
				return King (board, fromPos, toPos);
			}
			return false;
		}	

		public static bool Pawn(Piece[,] board, Coord fromPos, Coord toPos)
		{
			int direction;
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

			if (diffInX == 1 || diffInX == -1) {
				if (diffInY == direction) {
					if (board [fromPos.xpos, fromPos.ypos].color != board [toPos.xpos, toPos.ypos].color && board [toPos.xpos, toPos.ypos].type != Piece.PieceType.NONE) {
						return true;
					}
				}
				return false;
			}

			/////////////////////////////////////////////////////
			/// Handles vertical movements //////////////////////
			/////////////////////////////////////////////////////

			if (fromPos.ypos > 0 && fromPos.ypos < 7) {
				if (board [fromPos.xpos, fromPos.ypos + direction].type != Piece.PieceType.NONE) {
					return false;
				} 
				if (toPos.ypos - fromPos.ypos == direction * 2 && board [fromPos.xpos, fromPos.ypos + direction * 2].type != Piece.PieceType.NONE) {
					return false;
				}
			} else {
				board [fromPos.xpos, fromPos.ypos].type = Piece.PieceType.QUEEN;
				if (board [fromPos.xpos, fromPos.ypos].color == Piece.PieceColor.BLACK) {
					board [fromPos.xpos, fromPos.ypos].textureRef = 7;
				} else {
					board [fromPos.xpos, fromPos.ypos].textureRef = 1;
				}

			}

		/*	if (fromPos.xpos - toPos.xpos == 0) {
				if (toPos.ypos - fromPos.ypos == direction) {
					return true;
				} else if(board[fromPos.xpos,fromPos.ypos].moved == false && toPos.ypos - fromPos.ypos == (direction + direction)) {
					return true;
				}
			}*/

			if (fromPos.xpos - toPos.xpos == 0) {
				if (toPos.ypos - fromPos.ypos == direction) {
					return true;
				} else if(toPos.ypos - fromPos.ypos == (direction + direction)) {
					if(board [fromPos.xpos, fromPos.ypos].color == Piece.PieceColor.WHITE && fromPos.ypos == 6) {
						return true;
					} else if (board [fromPos.xpos, fromPos.ypos].color == Piece.PieceColor.BLACK && fromPos.ypos == 1) {
						return true;
					}

				}
			}

			/*if (fromPos.xpos - toPos.xpos == 0) {
				if (toPos.ypos - fromPos.ypos == direction) {
					return true;
				} else if(board [fromPos.xpos, fromPos.ypos].color == Piece.PieceColor.WHITE && fromPos.ypos == 6) {
					return true;
				} else if (board [fromPos.xpos, fromPos.ypos].color == Piece.PieceColor.BLACK && fromPos.ypos == 1) {
					return true;
				}

			}*/
			return false;
		}

		private static bool Rook(Piece[,] board, Coord fromPos, Coord toPos)
		{
			if (fromPos.xpos == toPos.xpos && fromPos.ypos == toPos.ypos) {
				return false;
			}

			if (isPathNotBlocked (board, fromPos, toPos)) {

				if (isStraightMove(fromPos, toPos))
				{
					if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
						return true;
					}
				}
			}
			return false;
		}

		private static bool Knight(Piece[,] board, Coord fromPos, Coord toPos)
		{
			int diffInX = toPos.xpos - fromPos.xpos;
			int diffInY = toPos.ypos - fromPos.ypos;

			if (Math.Abs (diffInX) == 1 && Math.Abs (diffInY) == 2) {
				if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
					return true;
				}
			}

			if (Math.Abs (diffInX) == 2 && Math.Abs (diffInY) == 1) {
				if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
					return true;
				}
			}
			return false;
		}

		private static bool Bishop(Piece[,] board, Coord fromPos, Coord toPos)
		{
			if (fromPos.xpos == toPos.xpos && fromPos.ypos == toPos.ypos) {
				return false;
			}

			if (isPathNotBlocked (board, fromPos, toPos)) {

				if (isDiagonalMove(fromPos, toPos))
				{
					if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
						return true;
					}
				}
			}

			return false;
		}

		private static bool Queen(Piece[,] board, Coord fromPos, Coord toPos) 
		{
			if (fromPos.xpos == toPos.xpos && fromPos.ypos == toPos.ypos) {
				return false;
			}

			if (isPathNotBlocked (board, fromPos, toPos)) {

				if (isStraightMove(fromPos, toPos) || isDiagonalMove(fromPos, toPos))
				{
					if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
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
					return true;
				}
			}

			if (Math.Abs (diffInX) == 0 && Math.Abs (diffInY) == 1) {
				if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
					return true;
				}
			}

			if (Math.Abs (diffInX) == 1 && Math.Abs (diffInY) == 1) {
				if (board [toPos.xpos, toPos.ypos].color != board [fromPos.xpos, fromPos.ypos].color) {
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
					if (isLegalMove (board, fromPos, tempCord)) {
						temp.Add (tempCord);
					}
				}
			}

			return temp;
		}

		public static bool isCheck(Piece[,] board, Piece.PieceColor color, Coord kingCoord) {
			List<Coord> possibleMoves = new List<Coord> ();

			foreach (Piece p in board) {
				if (p.color == color) {
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

		public static bool isCheckMate(Board board, Piece.PieceColor color) {

			bool stillCheck = true;
			List<Coord> currPieceLegalMoves = new List<Coord> ();
			Coord kingCoord = new Coord ();
			for (int y = 0; y < board.board.GetLength(0); y++) {
				for (int x = 0; x < board.board.GetLength(0); x++) {

					if (board.board [x, y].color == color && stillCheck) {
						currPieceLegalMoves = getLegalMoves (board.board, board.board [x, y].coord);
						foreach (Coord c in currPieceLegalMoves) {
	
							Board tempBoard = new Board (board);

							Piece temp = tempBoard.board [x, y];
						
							temp.coord = c;
							if (tempBoard.board [c.xpos, c.ypos].type != Piece.PieceType.NONE) {
								tempBoard.board [c.xpos, c.ypos] = new Piece (Piece.PieceType.NONE, c.xpos, c.ypos, Piece.PieceColor.NONE, 99);
							}
							tempBoard.board [x, y] = tempBoard.board [c.xpos, c.ypos];
							tempBoard.board [c.xpos, c.ypos] = temp;

							foreach (Piece p in tempBoard.board) {
								if (p.type == Piece.PieceType.KING && p.color == color) {
									kingCoord = new Coord (p.coord.xpos, p.coord.ypos);
								}
							}

							if (color == Piece.PieceColor.WHITE) {
								stillCheck = isCheck (tempBoard.board, Piece.PieceColor.BLACK, kingCoord);
								if (stillCheck == false) {;
									return false;
								}
							} else {
								stillCheck = isCheck (tempBoard.board, Piece.PieceColor.WHITE, kingCoord);
								if (stillCheck == false) {
									return false;
								}

							}

						}
					}

				}
			}
			return true;
		}

	}
}

