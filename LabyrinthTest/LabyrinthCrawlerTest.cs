using Labyrinth.Build;
using Labyrinth.Crawl;
using Labyrinth.Tiles;
using Labyrinth.Items;

namespace LabyrinthTest.Crawl;

[TestFixture(Description = "Integration test for the crawler implementation in the labyrinth")]
public class LabyrinthCrawlerTest
{
    #region Initialization
    [Test]
    public void InitWithCenteredX()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                | x|
                +--+
                """));
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void InitWithMultipleXUsesLastOne()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                |xx|
                +--+
                """));
        var test = laby.NewCrawler();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
    }

    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
           Assert.Throws<ArgumentException>(() => new Labyrinth.Labyrinth(new AsciiParser("""
                    +--+
                    |  |
                    +--+
                """)));
    }
    #endregion

    #region Labyrinth borders
    [Test]
    public void FacingNorthOnUpperTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +-x+
                |  |
                +--+
                """));
        var test = laby.NewCrawler();

        Assert.That(test.FacingTile, Is.EqualTo(Outside.Singleton));
    }

    [Test]
    public void FacingWestOnFarLeftTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                x  |
                +--+
                """));
        var test = laby.NewCrawler();
        ((Crawler)test).TurnLeft();

        Assert.That(test.FacingTile, Is.EqualTo(Outside.Singleton));
    }

    [Test]
    public void FacingEastOnFarRightTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                |  x
                +--+
                """));
        var test = laby.NewCrawler();
        ((Crawler)test).TurnRight();

        Assert.That(test.FacingTile, Is.EqualTo(Outside.Singleton));
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                |  |
                +x-+
                """));
        var test = laby.NewCrawler();
        ((Crawler)test).TurnRight();
        ((Crawler)test).TurnRight();

        Assert.That(test.FacingTile, Is.EqualTo(Outside.Singleton));
    }
    #endregion

    #region Moves
    [Test]
    public void TurnLeftFacesWestTile()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                |x |
                +--+
                """));
        var test = laby.NewCrawler();
        ((Crawler)test).TurnLeft();

        Assert.That(test.Direction, Is.EqualTo(Direction.West));
    }

    [Test]
    public void WalkReturnsInventoryAndChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                |  |
                |x |
                +--+
                """));
        var test = laby.NewCrawler();
        var previousPosition = (test.X, test.Y);
        var previousTile = test.FacingTile;
        var inventory = ((Crawler)test).Walk();

        using var all = Assert.EnterMultipleScope();
        
        Assert.That((test.X, test.Y), Is.Not.EqualTo(previousPosition));
        Assert.That(test.FacingTile, Is.Not.EqualTo(previousTile));
        Assert.That(inventory, Is.InstanceOf<Inventory>());
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                |  |
                |x |
                +--+
                """));
        var test = laby.NewCrawler();
        var previousPosition = (test.X, test.Y);
        var previousTile = test.FacingTile;
        ((Crawler)test).TurnRight();
        var inventory = ((Crawler)test).Walk();

        using var all = Assert.EnterMultipleScope();
        
        Assert.That((test.X, test.Y), Is.Not.EqualTo(previousPosition));
        Assert.That(test.FacingTile, Is.Not.EqualTo(previousTile));
        Assert.That(test.Direction, Is.EqualTo(Direction.East));
        Assert.That(inventory, Is.InstanceOf<Inventory>());
    }

    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
       var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                |  |
                |- |
                |x |
                +--+
                """));
        var test = laby.NewCrawler();
       Assert.Throws<InvalidOperationException>(() => ((Crawler)test).Walk());
    }
    #endregion

    #region Items and doors
    [Test]
    public void WalkInARoomWithAnItem()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                |k/|
                |x |
                +--+
                """));
        
        var test = laby.NewCrawler();
        var inventory = test.Walk();
        
        Assert.That(((Crawler)test).Inventory, Is.Not.Null);
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        Assert.That(false);
    }

    [Test]
    public void WalkUseKeyToOpenADoorAndPass()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                |xk|
                +-/|
                """));
        var test = laby.NewCrawler();

        test.Direction.TurnRight();

        var inventory = test.Walk();

        test.Direction.TurnRight();
        ((Door)test.FacingTile).Open(inventory);

        test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.Direction, Is.EqualTo(Direction.South));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion
}