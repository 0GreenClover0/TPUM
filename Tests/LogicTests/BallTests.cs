using Logic;

namespace Tests
{
    [TestClass]
    public class BallTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            int x = 123;
            int y = 456;
            IBall ball1 = IBall.CreateBall(x, y);
            Assert.AreEqual(ball1.x, x);
            Assert.AreEqual(ball1.y, y);
        }
        [TestMethod]
        public void SetTest()
        {
            int x = 50;
            int y = 100;
            IBall ball2 = IBall.CreateBall(0, 0);
            ball2.x = x;
            ball2.y = y;
            Assert.AreEqual(ball2.x, x);
            Assert.AreEqual(ball2.y, y);
        }
    }
}
