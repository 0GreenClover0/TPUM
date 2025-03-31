namespace Data
{
    internal class Candidate : ICandidate
    {
        public override int ID { get; }
        public override string FullName { get; }
        public override string Party { get; }
        public override bool IsChosen { get; set; }
        public override event EventHandler<Event>? OnPropertyChanged;

        public Candidate(int id, string name, string party)
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
}