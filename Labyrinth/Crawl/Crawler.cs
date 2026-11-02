using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl
{
    /// <summary>
    /// Labyrinth crawler.
    /// </summary>
    public class Crawler:  ICrawler
    {
        public int X { get; }

        public int Y { get; }

        public Direction Direction { get; }

        public Tile FacingTile { get; set; }

        public Labyrinth Labyrinth;

        public Crawler((int, int) StartingPosition, Direction direction, Labyrinth labyrinth)
        {
            X = StartingPosition.Item1;
            Y = StartingPosition.Item2;
            Direction = direction;
            Labyrinth = labyrinth;
            FacingTile = CalcFacingTile();
        }

        public Tile CalcFacingTile()
        {
            Console.WriteLine($"X: {X + Direction.DeltaX}, Y: {Y + Direction.DeltaY}, Labyrinth Width: {Labyrinth.Width}, Labyrinth Height: {Labyrinth.Height}");
            if (X + Direction.DeltaX >= Labyrinth.Width 
                || Y + Direction.DeltaY >= Labyrinth.Height 
                || X + Direction.DeltaX < 0 
                || Y + Direction.DeltaY < 0)
            {
                return Outside.Singleton;
            }
            else
            {
                return Labyrinth.getTile(X + Direction.DeltaX, Y + Direction.DeltaY);
            }
        }

        public void TurnLeft()
        {
            Direction.TurnLeft();
            FacingTile = CalcFacingTile();
        }

        public void TurnRight()
        {
            Direction.TurnRight();
            FacingTile = CalcFacingTile();
        }

        public Inventory Walk()
        {
            return null;
        }
    }
}