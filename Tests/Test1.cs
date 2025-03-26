namespace Tests
{
    [TestClass]
    public sealed class Tests
    {
        [TestMethod]
        public void CalculatorTest()
        {
            TPUM.Calculator calculator = new TPUM.Calculator();
            int result = calculator.Add(1, 1);
            Assert.AreEqual(2, result);
        }
    }
}
