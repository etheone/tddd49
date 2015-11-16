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
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class ChessGame : Game
	{

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		private Texture2D dummyTexture;
		private int[] currentIndex = new int[] {99, 99};
		private Board board;
		private bool firstClick;
		private Coord fromPos, toPos;
		//private Coord toPos;

		//User Interface variables

		//Chess variables
		//private List<ChessBoard> boardHistory;
		//private ChessBoard boardInfo;
		private Texture2D[] textureArray;

		public ChessGame()
			: base()
		{
			firstClick = true;
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			board = new Board ();
			// Frame rate is 30 fps by default for Windows Phone.
			TargetElapsedTime = TimeSpan.FromTicks(666666);

			// Extend battery life under lock.
			//InactiveSleepTime = TimeSpan.FromSeconds(1);

			textureArray = new Texture2D[12];
			//undoRectangle = new Rectangle(540, 400, 200, 50);
			//boardHistory = new List<ChessBoard>();
			this.IsMouseVisible = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{


			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here

			spriteBatch = new SpriteBatch(GraphicsDevice);
			dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
			dummyTexture.SetData(new Color[] { Color.White });
			LoadTextures();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}


		protected override void Update(GameTime gameTime)
		{
			//int index;
			base.Update(gameTime);
			//Console.WriteLine ("Updating");
			MouseState ms = Mouse.GetState ();
			Boolean moved = false;
			if (ms.LeftButton == ButtonState.Pressed) {
				Console.WriteLine (firstClick);

				if (!firstClick) {

					int newxPos = (int)(ms.X / 60);
					int newyPos = (int)(ms.Y / 60);
					//Console.WriteLine ("Second click: " + newxPos + " " + newyPos);
					//int[] newIndex = new int[] {newxPos, newyPos};
					toPos = new Coord (newxPos, newyPos);

					// Here we try to make a move
					board.tryMove (fromPos, toPos);


					/*if (Rules.isLegalMove (board.board [currentIndex [0], currentIndex [1]], board, newIndex)) {
						EmptyPiece temp = new EmptyPiece (currentIndex [0], currentIndex [1], Piece.PieceColor.NONE);
						board.board [newxPos, newyPos] = board.board [currentIndex [0], currentIndex [1]];
						board.board [currentIndex [0], currentIndex [1]] = temp;
					}*/


					currentIndex [0] = 99;
					currentIndex [1] = 99;
					firstClick = true;
					moved = true;

				} else {



					int xPos = (int)(ms.X / 60);
					int yPos = (int)(ms.Y / 60);
					//Console.WriteLine ("First click: " + xPos + " " + yPos);
					fromPos = new Coord (xPos, yPos);


					if (moved == false) {
						if (currentIndex [0] == xPos && currentIndex [1] == yPos) {

							currentIndex [0] = 99;
							currentIndex [1] = 99;

						} else {
							currentIndex [0] = (int)(ms.X / 60);
							currentIndex [1] = (int)(ms.Y / 60);
						}

						if (xPos < 8 && yPos < 8) {
							if (currentIndex != null) {
								Console.WriteLine (currentIndex);
								Console.WriteLine (xPos + ", " + yPos);

							} else {

							}
						}
					}
					firstClick = false;
				}

			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			//Draw Squares and Pieces
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
							//Console.WriteLine ("HEI");
						}
						else
						{
							squareColor = Color.White;
						}
					}
					if (x == currentIndex [0] && y == currentIndex [1]) {
						spriteBatch.Draw (dummyTexture, new Rectangle ((x * 60), (y * 60), 60, 60), Color.Blue);
					} else {
						spriteBatch.Draw (dummyTexture, new Rectangle ((x * 60), (y * 60), 60, 60), squareColor);
					}
				//spriteBatch.Draw(dummyTexture, new Rectangle((i % Constants.NumberOfFiles) * Constants.SquareSize, (int)(i / Constants.NumberOfRanks) * Constants.SquareSize, Constants.SquareSize, Constants.SquareSize), squareColor);
					// this one spriteBatch.Draw (dummyTexture, new Rectangle ((x * 60), (y * 60), 60, 60), squareColor);
					//Console.WriteLine (board.board [y, x].GetType ());
					//if (board.board [x, y].GetType () != typeof(EmptyPiece)) {
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