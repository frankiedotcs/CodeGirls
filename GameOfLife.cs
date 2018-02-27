using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Summary description for Game of Life Shapes and Grids
/// </summary>
namespace GameOfLife
{
    public class Shape
    {
        public Point Position { get; private set; }
        public Rectangle Bounds { get; private set; }

        public bool IsAlive { get; set; }

        public Shape(Point position)
        {
            Position = position;
            Bounds = new Rectangle(Position.X * Game.ShapeSize, Position.Y * Game.ShapeSize, Game.ShapeSize, Game.ShapeSize);

            IsAlive = false;
        }

        public void Update(MouseState mouseState)
        {
            if (Bounds.Contains(new Point(mouseState.X, mouseState.Y)))
            {

                if (mouseState.LeftButton == ButtonState.Pressed)
                    IsAlive = true; //with the left mous click, it will turn the box alive
                else if (mouseState.RightButton == ButtonState.Pressed)
                    IsAlive = false; //allows for correcting if you chose the wrong box, kills the box
            }
        }

        public void Draw(Box1 box1)
        {
            if (IsAlive)
                box1.Draw(Game.Pixel, Bounds, Color.Green);


        }
    }

    class Grid
    {
        public Point Size { get; private set; }
        /// <summary>
        /// private 2d
        /// </summary>
        private Box[,] boxes;

        /// <summary>
        /// Constructor
        /// </summary>
        public Grid()
        {
            Size = new Point(Game.BoxesX, Game.BoxesY);

            boxes = new Box[Size.X, Size.Y];

            for (int i = 0; i < Size.X; i++)
                for (int j = 0; j < Size.Y; j++)
                    boxes[i, j] = new Box(new Point(i, j));
        }

        /// <summary>
        /// updates the game on a set interval
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {

            //loops through every box on the grid
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    // Check the box's state and checks the neighbors as well!
                    bool living = boxes[i, j].IsAlive;
                    int count = GetLivingNeighbors(i, j);
                    bool result = false;

                    //applies the rules of the game to whatever box.
                    if (living && count < 2)
                        result = false;
                    if (living && (count == 2 || count == 3))
                        result = true;
                    if (living && count > 3)
                        result = false;
                    if (!living && count == 3)
                        result = true;

                    boxes[i, j].IsAlive = result;
                }
            }
        }
        /// <summary>
        /// checks the living neighbors next to the selected box.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int LivingNeighbors(int x, int y)
        {
            int count = 0;

            // bottom left.
            if (x != 0 && y != Size.Y - 1)
                if (boxes[x - 1, y + 1].IsAlive)
                    count++;

            // left
            if (x != 0)
                if (boxes[x - 1, y].IsAlive)
                    count++;

            //top left
            if (x != 0 && y != 0)
                if (boxes[x - 1, y - 1].IsAlive)
                    count++;

            // Check cell on the top.
            if (y != 0)
                if (boxes[x, y - 1].IsAlive)
                    count++;

            // right
            if (x != Size.X - 1)
                if (boxes[x + 1, y].IsAlive)
                    count++;

            //bottom right
            if (x != Size.X - 1 && y != Size.Y - 1)
                if (boxes[x + 1, y + 1].IsAlive)
                    count++;

            // bottom
            if (y != Size.Y - 1)
                if (boxes[x, y + 1].IsAlive)
                    count++;

            //top right
            if (x != Size.X - 1 && y != 0)
                if (boxes[x + 1, y - 1].IsAlive)
                    count++;

            return count;
        }
        /// <summary>
        /// draw function for the grid
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Box box in boxes)
                box.Draw(spriteBatch);

            // Draw vertical gridlines.
            for (int i = 0; i < Size.X; i++)
                spriteBatch.Draw(Game.Pixel, new Rectangle(i * Game.BoxSize - 1, 0, 1, Size.Y * Game.BoxSize), Color.DarkGray);

            // Draw horizontal gridlines.
            for (int j = 0; j < Size.Y; j++)
                spriteBatch.Draw(Game.Pixel, new Rectangle(0, j * Game.BoxSize - 1, Size.X * Game.BoxSize, 1), Color.DarkGray);
        }
    }
}
 