namespace ModernBaseLibrary.Text
{
    using System.Collections.Generic;

    public class PunctuationFSMBuilder : IFSMBuilder
    {
        public FSM Build()
        {
            var punctuations = new List<char>()
            {
                '.',
                ',',
                '(',
                ')'
            };

            var fsm = new FSM();

            var state0 = new FSMState() { Id = "0", IsStart = true, IsEnd = false }; fsm.States.Add(state0);
            var stateEnd = new FSMState() { Id = "end", IsStart = false, IsEnd = true }; fsm.States.Add(stateEnd);
            foreach (var punctuation in punctuations)
            {
                var transition = new FSMTransition() { Start = state0, End = stateEnd, Item = punctuation };
                fsm.Transitions.Add(transition);
            }

            return fsm;
        }
    }
}
