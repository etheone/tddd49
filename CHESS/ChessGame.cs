#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using CHESS;
#endregion

namespace CHESS
{

	/// The UI class
	public class ChessGame : Game
	{

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		private Texture2D dummyTexture;
		private int[] currentIndex = new int[] {99, 99};
		private Board board;
		private bool startGameClick = false;
		private bool firstClick;
		private Coord fromPos, toPos;
		List<Coord> legalMoves;

		private Texture2D[] textureArray;

		public ChessGame()
			: base()
		{
			firstClick = true;
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			board = new Board ();
			TargetElapsedTime = TimeSpan.FromTicks(666666);
			textureArray = new Texture2D[12];
			this.IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		//LoadContent will be called once per game, loading all content.
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			spriteBatch = new SpriteBatch(GraphicsDevice);
			dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
			dummyTexture.SetData(new Color[] { Color.White });
			LoadTextures();

		}

		protected override void UnloadContent()
		{

		}

		protected override void Update(GameTime gameTime)
		{

			base.Update(gameTime);
			MouseState ms = Mouse.GetState ();
			Boolean moved = false;
			if (ms.LeftButton == ButtonState.Pressed) {
				if (startGameClick == false) {
					startGameClick = true;
					board.startGame ();
					return;
				}
				if ((int)(ms.X) > 480) {
					board.startNewGame ();
					legalMoves = null;
					return;
				}
				if (!firstClick) {

					int newxPos = (int)(ms.X / 60);
					int newyPos = (int)(ms.Y / 60);

					toPos = new Coord (newxPos, newyPos);

					// Here we try to make a move
					board.tryMove (fromPos, toPos);

					legalMoves = null;

					currentIndex [0] = 99;
					currentIndex [1] = 99;
					firstClick = true;
					moved = true;
				
				} else {
					int xPos = (int)(ms.X / 60);
					int yPos = (int)(ms.Y / 60);
					if (board.board [xPos, yPos].color == board.turn) {
						fromPos = new Coord (xPos, yPos);
						if (moved == false) {
							if (currentIndex [0] == xPos && currentIndex [1] == yPos) {
								currentIndex [0] = 99;
								currentIndex [1] = 99;
							} else {
								currentIndex [0] = (int)(ms.X / 60);
								currentIndex [1] = (int)(ms.Y / 60);
							}
						}
						firstClick = false;
						legalMoves = Rules.getLegalMoves (board.board, fromPos);
					} else {
						Console.WriteLine ("NOT CORRECT TURN");
					}
				}
			}
		}

		/// This is called when the game should draw itself.
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			//Draw Squares and Pieces
			board.board = board.createBoardFromFile ();

			Color squareColor = Color.White;
			Texture2D textureToDraw;
			for (int y = 0; y < board.board.GetLength(0); y++) 
			{
				for (int x = 0; x < board.board.GetLength(0); x++) 
				{

					if ((int)(x) % 2 == 0)
					{
						if (y % 2 == 0)
						{
							squareColor = Color.White;
						}
						else
						{
							squareColor = Color.Gray;
						}
					}
					else
					{
						if (y % 2 == 0)
						{
							squareColor = Color.Gray;
						}
						else
						{
							squareColor = Color.White;
						}
					}
					if (x == currentIndex [0] && y == currentIndex [1]) {
						spriteBatch.Draw (dummyTexture, new Rectangle ((x * 60), (y * 60), 60, 60), Color.Blue);
					}  else {
						spriteBatch.Draw (dummyTexture, new Rectangle ((x * 60), (y * 60), 60, 60), squareColor);
					}

					if (board.board [x, y].type != Piece.PieceType.NONE) {
						if (board.board [x, y].color == Piece.PieceColor.WHITE) {
							textureToDraw = textureArray [board.board [x, y].textureRef];
							spriteBatch.Draw (textureToDraw, new Rectangle ((x * 60), (y * 60), 60, 60), Color.Beige);
						} else {
							textureToDraw = textureArray [board.board[x, y].textureRef];
							spriteBatch.Draw (textureToDraw, new Rectangle ((x * 60), (y * 60), 60, 60), Color.Beige);
						}
					} else {
						spriteBatch.Draw (dummyTexture, new Rectangle ((x * 60), (y * 60), 60, 60), squareColor);
					}
				}
			}

			if (legalMoves != null) {
				foreach (Coord x in legalMoves) {
					spriteBatch.Draw (dummyTexture, new Rectangle (((x.xpos * 60) + 25), ((x.ypos * 60) + 25), 10, 10), Color.LightBlue);
				}
			}
			spriteBatch.End();
			base.Draw(gameTime);
		}

		private void LoadTextures()
		{
			string dir = System.IO.Directory.GetCurrentDirectory();
			string dir2 = System.IO.Directory.GetParent (dir).FullName;
			string dir3 = System.IO.Directory.GetParent (dir2).FullName;
			textureArray[5] = Content.Load<Texture2D>(dir3 + "/Content/pawn-white.png");
			textureArray[4] = Content.Load<Texture2D>(dir3 + "/Content/knight-white.png");
			textureArray[3] = Content.Load<Texture2D>(dir3 + "/Content/bishop-white.png");
			textureArray[2] = Content.Load<Texture2D>(dir3 + "/Content/rook-white.png");
			textureArray[1] = Content.Load<Texture2D>(dir3 + "/Content/queen-white.png");
			textureArray[0] = Content.Load<Texture2D>(dir3 + "/Content/king-white.png");

			textureArray[11] = Content.Load<Texture2D>(dir3 + "/Content/pawn-black.png");
			textureArray[10] = Content.Load<Texture2D>(dir3 + "/Content/knight-black.png");
			textureArray[9] = Content.Load<Texture2D>(dir3 + "/Content/bishop-black.png");
			textureArray[8] = Content.Load<Texture2D>(dir3 + "/Content/rook-black.png");
			textureArray[7] = Content.Load<Texture2D>(dir3 + "/Content/queen-black.png");
			textureArray[6] = Content.Load<Texture2D>(dir3 + "/Content/king-black.png");

		}
	}
}