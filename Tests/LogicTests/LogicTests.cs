﻿using Data;
using Logic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Diagnostics;
using System.Numerics;

namespace Tests
{
    [TestClass]
    public class LogicTests
    {
        internal class TestCandidate : ICandidate
        {
            public override int ID { get; }
            public override string FullName { get; }
            public override string Party { get; }
            public override bool IsChosen { get; set; }
            public override event EventHandler<Event>? OnPropertyChanged;

            public TestCandidate(int id, string name, string party)
            {
                ID = id;
                FullName = name;
                Party = party;
                IsChosen = false;
            }

            public override void ChooseCandidate()
            {
                IsChosen = true;
            }
        }

        internal class TestCandidateDatabase : ICandidateDatabase
        {
            private List<ICandidate> candidates = new List<ICandidate>();

            public override void AddCandidate(ICandidate candidate)
            {
                candidates.Add(candidate);
            }

            public override ICandidate? GetCandidate(int id)
            {
                if (candidates.ElementAt(id) == null)
                {
                    return null;
                }
                else
                {
                    return candidates.ElementAt(id);
                }
            }

            public override List<ICandidate> GetCandidates()
            {
                return candidates;
            }

            public override bool RemoveCandidate(int id)
            {
                if (candidates.ElementAt(id) == null)
                {
                    return false;
                }
                else
                {
                    candidates.RemoveAt(id);
                    return true;
                }
            }
        }

        internal class TestDataAPI : AbstractDataAPI
        {
            private readonly TestCandidateDatabase candidates;
            internal IDashBoard? dashboard { get; set; }
            internal static int hardCodedBoardW = 600;
            internal static int hardCodedBoardH = 600;

            public override ICandidate? GetCandidate(int id)
            {
                return candidates.GetCandidate(id);
            }

            public override List<ICandidate> GetCandidates()
            {
                return candidates.GetCandidates();
            }

            public override void AddCandidate(int id, string name, string party)
            {
                candidates.AddCandidate(new TestCandidate(id, name, party));
            }

            public override bool RemoveCandidate(int id)
            {
                return candidates.RemoveCandidate(id);
            }

            public override void CreateDashBoard()
            {
                dashboard = IDashBoard.CreateDashBoard(hardCodedBoardW, hardCodedBoardH);
            }
        }

        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}



