using System;
using System.Xml.Linq;
using System.IO;
using System.Linq;

namespace CHESS
{
	public class Board
	{
		public Piece[,] board;

		private AI ai2;
		public AI ai1;
		private bool fromFile = false;
	

		public String white = "ai";
		private String black = "ai";

		public Piece.PieceColor turn = Piece.PieceColor.WHITE;

		private bool whiteIsCheck;
		private bool blackIsCheck;

		public Board ()
		{
			if (fromFile == true) {

				board = createBoardFromFile ();

			} else {
				System.Console.WriteLine ("Initiating a new board");
				board = Board.newBoard ();

				whiteIsCheck = false;
				blackIsCheck = false;
				saveBoardToFile ();
			}

			if (black == "ai") {
				ai2 = new AI (Piece.PieceColor.BLACK);
			}

			if (white == "ai") {
				ai1 = new AI (Piece.PieceColor.WHITE);
			}
		}

		// Copy constructor
		public Board(Board b) {


			board = Board.newBoard ();
			for (int y = 0; y < 8; y++) {
				for (int x = 0; x < 8; x++) {
					board [x, y] = new Piece (b.board [x, y]);
				}
			}


			turn = b.turn;
			whiteIsCheck = b.whiteIsCheck;
			blackIsCheck = b.blackIsCheck;

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

		public Piece[,] createBoardFromFile() {
			Piece[,] board = new Piece[8,8];
			XDocument prevGame = XDocument.Load("game.xml");

			var lv0s = from lv0 in prevGame.Descendants ("settings")
				select new {
				Turn = lv0.Attribute("turn").Value,
				White = lv0.Attribute("white").Value,
				Black = lv0.Attribute("black").Value,
				Whiteischeck = lv0.Attribute("whiteischeck").Value,
				Blackischeck = lv0.Attribute("blackischeck").Value
			};
			
			foreach (var lv0 in lv0s)
			{
				this.whiteIsCheck = Convert.ToBoolean (lv0.Whiteischeck);
				this.blackIsCheck = Convert.ToBoolean (lv0.Blackischeck);
				this.white = lv0.White;
				this.black = lv0.Black;
				this.turn = (Piece.PieceColor)Enum.Parse(typeof(Piece.PieceColor), lv0.Turn);
			}

			var lv1s = from lv1 in prevGame.Descendants ("square")
				select new {
				Type = lv1.Attribute("type").Value,
				Color = lv1.Attribute("color").Value,
				Coordx = lv1.Attribute("coordx").Value,
				Coordy = lv1.Attribute("coordy").Value,
				Texture = lv1.Attribute("texture").Value
			};

			int count = 0;
			foreach (var lv1 in lv1s)
			{
				count += 1;

				Piece.PieceType type = (Piece.PieceType)Enum.Parse(typeof(Piece.PieceType), lv1.Type);
				Piece.PieceColor color = (Piece.PieceColor)Enum.Parse(typeof(Piece.PieceColor), lv1.Color);
				int x = Int32.Parse (lv1.Coordx);
				int y = Int32.Parse (lv1.Coordy);
				int txt = Int32.Parse (lv1.Texture);

				Piece temp = new Piece(type, x, y, color, txt);;
				board [x, y] = temp;
			}

			return board;
		}

		public void tryMove(Coord fromPos, Coord toPos) {

			Coord kingCoord = new Coord ();
			
			if ( Rules.isLegalMove (board, fromPos, toPos) ) {

				board[fromPos.xpos, fromPos.ypos].moved = true;

				Board tempBoard = new Board (this);

				Piece temp = tempBoard.board [fromPos.xpos, fromPos.ypos];

				temp.coord = toPos;
				if (tempBoard.board [toPos.xpos, toPos.ypos].type != Piece.PieceType.NONE) {
					tempBoard.board [toPos.xpos, toPos.ypos] = new Piece (Piece.PieceType.NONE, toPos.xpos, toPos.ypos, Piece.PieceColor.NONE, 99);
				}

				tempBoard.board [fromPos.xpos, fromPos.ypos] = tempBoard.board [toPos.xpos, toPos.ypos];
				tempBoard.board [toPos.xpos, toPos.ypos] = temp;

				foreach (Piece p in tempBoard.board) {
					if (p.type == Piece.PieceType.KING && p.color == turn) {
						kingCoord = new Coord (p.coord.xpos, p.coord.ypos);
					}

				}

				Piece.PieceColor colorToCheck;
				if (turn == Piece.PieceColor.WHITE) {
					colorToCheck = Piece.PieceColor.BLACK;
				} else {
					colorToCheck = Piece.PieceColor.WHITE;
				}

				if (!(Rules.isCheck (tempBoard.board, colorToCheck, kingCoord))) {

					swap (fromPos, toPos);

					//Investigate if move caused a Check.

					foreach (Piece p in board) {
						if (p.type == Piece.PieceType.KING && p.color != turn) {
							kingCoord = new Coord (p.coord.xpos, p.coord.ypos);
						}

					}

					if (Rules.isCheck (board, turn, kingCoord)) {
						if (turn == Piece.PieceColor.BLACK) {
							Console.WriteLine ("White is in check");
							whiteIsCheck = true;
						} else {
							Console.WriteLine ("Black is in check");
							blackIsCheck = true;
						}
					} else {
						whiteIsCheck = false;
						blackIsCheck = false;
					}

					if (turn == Piece.PieceColor.WHITE) {
						turn = Piece.PieceColor.BLACK;
						if (black == "ai") {
							ai2.calculateMove (this);
						}
					} else {
						turn = Piece.PieceColor.WHITE;

						if (white == "ai") {
							ai1.calculateMove (this);
						}
					}

				} else {
					// Check if chackmate here
					Console.WriteLine ("Cant move because king will be checked");
				}

				if (whiteIsCheck) {
					if (Rules.isCheckMate (this, Piece.PieceColor.WHITE)) {
						Console.WriteLine ("GAME IS OVER, BLACK WON");
					}
					Console.WriteLine ("check if white is checkmate");
				}
				if (blackIsCheck) {
					if (Rules.isCheckMate (this, Piece.PieceColor.BLACK)) {

						Console.WriteLine ("GAME IS OVER, WHITE WON");
					}
					Console.WriteLine ("check if black is checkmate");
				}

				saveBoardToFile ();

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

		public void startGame() {
			if (white == "ai") {
				ai1.calculateMove (this);
			}
		}

		public void startNewGame() {
			turn = Piece.PieceColor.WHITE;
			board = newBoard ();
			saveBoardToFile ();

		}

		public void saveBoardToFile() {

			XElement xmlTree = new XElement("Root",
			                                new XElement("settings", new XAttribute("turn", turn), 
			             new XAttribute("white", white), 
			             new XAttribute("black", black), 
			             new XAttribute("whiteischeck", whiteIsCheck), 
			             new XAttribute("blackischeck", blackIsCheck)));
			XElement boardToAdd = new XElement ("board");

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					Piece p = new Piece (board [j, i].type, j, i, board [j, i].color, board [j, i].textureRef);
					boardToAdd.Add (new XElement ("square", new XAttribute("color", p.color), 
					                              new XAttribute("coordx", p.coord.xpos), 
					                              new XAttribute("coordy", p.coord.ypos), 
					                              new XAttribute("type", p.type), 
					                              new XAttribute("texture", p.textureRef)));
				}
			}

			xmlTree.Add (boardToAdd);

			XDocument game = new XDocument();
			game.Add (xmlTree);

			game.Save ("game.xml");
		}


	}
}

