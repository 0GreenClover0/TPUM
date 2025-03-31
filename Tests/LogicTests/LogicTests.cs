using Data;
using Logic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Diagnostics;
using System.Numerics;

namespace Tests
{
    [TestClass]
    public class LogicTests
    {
        internal class TestDataAPI : AbstractDataAPI
        {
            public override void AddCandidate(int id, string name, string party)
            {
                throw new NotImplementedException();
            }

            public override void CreateDashBoard()
            {
                throw new NotImplementedException();
            }

            public override ICandidate? GetCandidate(int id)
            {
                throw new NotImplementedException();
            }

            public override List<ICandidate> GetCandidates()
            {
                throw new NotImplementedException();
            }

            public override bool RemoveCandidate(int id)
            {
                throw new NotImplementedException();
            }
        }

        internal class TestBall : ICandidate
        {
            public override int ID => throw new NotImplementedException();

            public override string FullName => throw new NotImplementedException();

            public override string Party => throw new NotImplementedException();

            public override bool IsChosen { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public override event EventHandler<Event>? OnPropertyChanged;

            public override void ChooseCandidate()
            {
                throw new NotImplementedException();
            }
        }

        // Add more such test methods below...
        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}



