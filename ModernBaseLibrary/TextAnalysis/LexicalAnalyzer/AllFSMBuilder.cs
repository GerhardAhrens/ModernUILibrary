namespace ModernBaseLibrary.Text
{
    public class AllFSMBuilder : IFSMBuilder
    {
        public FSM Build()
        {
            var fsm = new FSM();

            var stateStart = new FSMState() { Id = "start", IsStart = true, IsEnd = false }; fsm.States.Add(stateStart);
            var stateEnd = new FSMState() { Id = "end", IsStart = false, IsEnd = true }; fsm.States.Add(stateEnd);            

            var transition = new FSMTransition() { Start = stateStart, End = stateEnd, Item = '*' };
            fsm.Transitions.Add(transition);

            return fsm;
        }
    }
}
