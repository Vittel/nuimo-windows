namespace TestDesktopDriver.Nuimo
{
    public struct Fly
    {
        public Action action;
        public byte hoverDistance;

        public Fly(Action action, byte hoverDistance)
        {
            this.action = action;
            this.hoverDistance = hoverDistance;
        }

        public enum Action
        {
            Left = 0,
            Right = 1,
            Forward = 2,
            Backward = 3,
            Hover = 4
        }
    }
}
