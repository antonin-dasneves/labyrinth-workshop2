using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Crawl
{
    /// <summary>
    /// Labyrinth crawler.
    /// </summary>
    public class Crawler:  ICrawler
    {
        /// <summary>
        /// Gets the current X position.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the current Y position.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Gets the current direction.
        /// </summary>
        public Direction Direction { get; }

        /// <summary>
        /// Gets the tile in front of the crawler.
        /// </summary>
        public Tile FacingTile { get; }

        public Crawler((int, int) StartingPosition, Tile facingTile)
        {
            X = StartingPosition.Item1;
            Y = StartingPosition.Item2;
            Direction = Direction.North;
            FacingTile = facingTile;
        }

        /// <summary>
        /// Pass the tile in front of the crawler and move into it.
        /// </summary>
        /// <returns>An inventory of the collectable items in the place reached.</returns>
        public Inventory Walk()
        {
            return null;
        }
    }
}