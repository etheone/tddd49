using System;
using System.Collections.Generic;


namespace CHESS
{
	public class AI
	{

		private int bestPoints;
		private Coord moveFrom;
		private	Coord moveTo;
		List<Coord[]> moves;

		private Piece.PieceColor color;
		public AI (Piece.PieceColor c)
		{
			color = c;
		}

		public void calculateMove(Board board) {
			moves = new List<Coord[]> ();
			bestPoints = -1000;
			moveFrom = new Coord();
			moveTo = new Coord ();


			List<Coord> possibleMoves = new List<Coord> ();


			foreach (Piece p in board.board) {
				if (p.color == color) {

					possibleMoves = Rules.getLegalMoves (board.board, p.coord);
					Coord currentPieceCoord = p.coord;
					foreach (Coord c in possibleMoves) {

						if (causeCheck(board, currentPieceCoord, c)) {
							continue;
						} else {

							Piece.PieceType pieceAtToPos = board.board [c.xpos, c.ypos].type;
							int deduct = reducePoints (board, p.coord, c);
							switch (pieceAtToPos) {
							case Piece.PieceType.NONE:

								isBetterMove (2 - deduct, p.coord, c);
								break;
							case Piece.PieceType.PAWN:
								isBetterMove (6 - deduct, p.coord, c);
								break;
							case Piece.PieceType.ROOK:
								isBetterMove (10 - deduct, p.coord, c);
								break;
							case Piece.PieceType.KNIGHT:
								isBetterMove (10 - deduct, p.coord, c);
								break;
							case Piece.PieceType.BISHOP:
								isBetterMove (10 - deduct, p.coord, c);
								break;
							case Piece.PieceType.QUEEN:
								isBetterMove (15 - deduct, p.coord, c);
								break;
							case Piece.PieceType.KING:
								isBetterMove (14 - deduct, p.coord, c);
								break;
							}
						}

					}
				}
			}

			Random rnd = new Random();
			int index = rnd.Next (0, moves.Count);
			if (moves.Count == 0) {
				Console.WriteLine ("Out of moves!!");
			} else {
				board.tryMove (moves [index] [0], moves [index] [1]);
			}

		}

		private bool causeCheck(Board board, Coord fromPos, Coord toPos) {

			Board tempBoard = new Board (board);

			Piece temp = tempBoard.board [fromPos.xpos, fromPos.ypos];

			temp.coord = toPos;
			if (tempBoard.board [toPos.xpos, toPos.ypos].type != Piece.PieceType.NONE) {
				tempBoard.board [toPos.xpos, toPos.ypos] = new Piece (Piece.PieceType.NONE, toPos.xpos, toPos.ypos, Piece.PieceColor.NONE, 99);
			}

			tempBoard.board [fromPos.xpos, fromPos.ypos] = tempBoard.board [toPos.xpos, toPos.ypos];
			tempBoard.board [toPos.xpos, toPos.ypos] = temp;
	
			Piece.PieceColor colorToCheck;

			if(color == Piece.PieceColor.WHITE) {
				colorToCheck = Piece.PieceColor.BLACK;
			} else {
				colorToCheck = Piece.PieceColor.WHITE;
			}

			Coord kingCoord = new Coord ();

			foreach (Piece p in tempBoard.board) {
				if (p.type == Piece.PieceType.KING && p.color == color) {
					kingCoord = new Coord (p.coord.xpos, p.coord.ypos);
				}

			}

			if (Rules.isCheck (tempBoard.board, colorToCheck, kingCoord)) {
				return true;
			}

			return false;
		}

		private void isBetterMove(int pieceWorth, Coord fromPos, Coord toPos) {
			if (pieceWorth == bestPoints) {
				Coord[] coords = {fromPos, toPos};
				moves.Add(coords);

			} else if (pieceWorth > bestPoints) {
				moves.Clear();
				bestPoints = pieceWorth;
				moveFrom = fromPos;
				moveTo = toPos;
				Coord[] coords = {fromPos, toPos};
				moves.Add(coords);
			} 
		}

		private int reducePoints(Board board, Coord fromPos, Coord toPos) {

			int pointsToDeduct = 0;

			Board tempBoard = new Board (board);

			Piece temp = tempBoard.board [fromPos.xpos, fromPos.ypos];

			temp.coord = toPos;
			if (tempBoard.board [toPos.xpos, toPos.ypos].type != Piece.PieceType.NONE) {
				tempBoard.board [toPos.xpos, toPos.ypos] = new Piece (Piece.PieceType.NONE, toPos.xpos, toPos.ypos, Piece.PieceColor.NONE, 99);
			}
			//tempBoard.board [c.xpos, c.ypos].coord = new Coord(x,y);
			tempBoard.board [fromPos.xpos, fromPos.ypos] = tempBoard.board [toPos.xpos, toPos.ypos];
			tempBoard.board [toPos.xpos, toPos.ypos] = temp;

			Piece.PieceColor colorToCheck;

			if(color == Piece.PieceColor.WHITE) {
				colorToCheck = Piece.PieceColor.BLACK;
			} else {
				colorToCheck = Piece.PieceColor.WHITE;
			}

			List<Coord> test = new List<Coord>();

			foreach (Piece p in tempBoard.board) {
				if (p.color == colorToCheck) {
					List<Coord> tempList = new List<Coord>();
					tempList = Rules.getLegalMoves (tempBoard.board, p.coord);
					foreach(Coord c in tempList) {
						test.Add (c);
					}
				}

			}

			Piece.PieceType pieceAtPos;

			foreach (Piece p in tempBoard.board) {
				if (p.color == color) {
					if (test.Contains (p.coord)) {
						pieceAtPos = p.type;

						switch (pieceAtPos) {

						case Piece.PieceType.NONE:
							if (pointsToDeduct > 0) {	
								break;
							} else {
								pointsToDeduct = 0;
							}
							break;
						case Piece.PieceType.PAWN:
							if (pointsToDeduct >= 7) {
								break;
							} else {
								pointsToDeduct = 7;
							}
							break;
						case Piece.PieceType.ROOK:
							if (pointsToDeduct >= 11) {
								break;
							} else {
								pointsToDeduct = 11;
							}
							break;
						case Piece.PieceType.KNIGHT:
							if (pointsToDeduct >= 11) {
								break;
							} else {
								pointsToDeduct = 11;
							}
							break;
						case Piece.PieceType.BISHOP:
							if (pointsToDeduct >= 11) {
								break;
							} else {
								pointsToDeduct = 11;
							}
							break;
						case Piece.PieceType.QUEEN:
							if (pointsToDeduct >= 16) {
								break;
							} else {
								pointsToDeduct = 16;
							}
							break;
						case Piece.PieceType.KING:
							if (pointsToDeduct >= 15) {
								break;
							} else {
								pointsToDeduct = 15;
							}
							break;
						}

					}
				}
			}

			return pointsToDeduct;   
		}
			                     
	}
}

