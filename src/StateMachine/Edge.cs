namespace StateMachineGenerator
{
    internal class Edge
    {
        public string Label { get; set; }
        public Vertex Source { get; set; }
        public Vertex Destination { get; set; }
        public InputCharacter In { get; set; }
        public OutputCharacter Out { get; set; }
    }
}
