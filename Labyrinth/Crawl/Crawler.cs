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
            FacingTile = CalcFacingTile(Labyrinth);
        }

        public Tile CalcFacingTile(Labyrinth labyrinth)
        {
            return X + Direction.DeltaX < labyrinth.Width 
                    && Y + Direction.DeltaY < labyrinth.Height 
                    && X + Direction.DeltaX >= 0 
                    && Y + Direction.DeltaY >= 0 ?
             labyrinth.getTile(X + Direction.DeltaX, Y + Direction.DeltaY) : Outside.Singleton;
        }

        public void TurnLeft()
        {
            Direction.TurnLeft();
            FacingTile = CalcFacingTile(Labyrinth);
        }

        public Inventory Walk()
        {
            return null;
        }
    }
}