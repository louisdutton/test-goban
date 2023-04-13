using Go;

namespace GobanTest;

[TestClass]
public class GobanTest
{
    [TestMethod]
    public void WhiteTakenWhenSurrounded()
    {
        Goban goban = new Go.Goban(new string[] {
            ".#.",
            "#o#",
            ".#."
        });

        Assert.IsTrue(goban.IsTaken(1, 1));
    }

    [TestMethod]
    public void WhiteIsNotTakenWhenItHasALiberty()
    {
        Goban goban = new Go.Goban(new string[] {
            "...",
            "#o#",
            ".#."
        });

        Assert.IsFalse(goban.IsTaken(1, 2));
    }

    [TestMethod]
    public void BlackShapeTakenWhenSurrounded()
    {

        Goban goban = new Go.Goban(new string[] {
          "oo.",
          "##o",
          "o#o",
          ".o."
        });

        Assert.IsTrue(goban.IsTaken(0, 1));
        Assert.IsTrue(goban.IsTaken(1, 1));
        Assert.IsTrue(goban.IsTaken(1, 2));
    }

    [TestMethod]
    public void BlackShapeIsNoteTakenWhenItHasALiberty()
    {
        Goban goban = new Go.Goban(new string[] {
          "oo.",
          "##.",
          "o#o",
          ".o."
        });

        Assert.IsFalse(goban.IsTaken(0, 1));
        Assert.IsFalse(goban.IsTaken(1, 1));
        Assert.IsFalse(goban.IsTaken(1, 2));
    }

    [TestMethod]
    public void SquareShapeIsTaken()
    {
        Goban goban = new Go.Goban(new string[] {
          "oo.",
          "##o",
          "##o",
          "oo."
        });

        Assert.IsTrue(goban.IsTaken(0, 1));
        Assert.IsTrue(goban.IsTaken(0, 2));
        Assert.IsTrue(goban.IsTaken(1, 1));
        Assert.IsTrue(goban.IsTaken(1, 2));
    }
}