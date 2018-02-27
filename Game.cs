using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Models;


namespace GameOfLife
{
    public class Game
    {
        public const int UPS = 20; // Updates per second
        public const int FPS = 60;

        public const int BoxSize = 50; // Cell pixel width/height
        public const int BoxesX = 10;
        public const int BoxesY = 10;

        public static bool Paused = true;

       

        public static Vector2 ScreenSize;

        private Grid grid;

        private KeyboardState keyboardState, lastKeyboardState;

        private GraphicsDeviceManager graphics;
        private Box1 box1;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / FPS);

            ScreenSize = new Vector2(CellsX, CellsY) * CellSize;

            graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            grid = new Grid();

            keyboardState = lastKeyboardState = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            box1 = new Box1(GraphicsDevice);

            Font = Content.Load<SpriteFont>("Font");

            Pixel = new Texture2D(box1.GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Toggle pause when spacebar is pressed.
            if (keyboardState.IsKeyDown(Keys.Space) && lastKeyboardState.IsKeyUp(Keys.Space))
                Paused = !Paused;

            // Clear the screen if backspace is pressed.
            if (keyboardState.IsKeyDown(Keys.Back) && lastKeyboardState.IsKeyUp(Keys.Back))
                grid.Clear();

            base.Update(gameTime);

            grid.Update(gameTime);

            lastKeyboardState = keyboardState;
        }

        protected override void Draw(GameTime gameTime)
        {
            if (Paused)
                GraphicsDevice.Clear(Color.Red);
            else
                GraphicsDevice.Clear(Color.White);

            box1.Begin();
            if (Paused)
            {
                string paused = "Paused";
                 box1.DrawString(Font, paused, ScreenSize / 2, Color.Gray, 0f, Font.MeasureString(paused) / 2, 1f, SpriteEffects.None, 0f);
            }
            grid.Draw(box1);
            box1.End();

            base.Draw(gameTime);
        }
    }
}
