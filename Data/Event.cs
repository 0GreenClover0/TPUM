﻿namespace Data
{
    public class Event
    {
        internal ICandidate candidate;

        public Event(ICandidate candidate)
        {
            this.candidate = candidate;
        }
    }
}
