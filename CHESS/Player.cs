using System;

namespace CHESS
{
	public class Player
	{

		PlayerType type;
		Board board;
		AI ai;

		public enum PlayerType
		{
			AI,
			Player
		}

		public Player (PlayerType type, Board board, Piece.PieceColor color)
		{
			this.type = type;
			this.board = board;
			if (this.type == PlayerType.AI) {
				ai = new AI (color);
			}
		}

		//If type is Player, waits for user input.
		public void nextMove() {
			if (this.type == PlayerType.AI) {
				ai.calculateMove (board);
			}

		}
	}
}

