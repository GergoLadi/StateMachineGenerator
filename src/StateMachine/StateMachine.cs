using System.Collections.Generic;
using System.Text;

namespace StateMachineGenerator
{
    internal class StateMachine
    {
        public List<Vertex> Vertices;
        public List<Edge> Edges;
        public List<InputCharacter> InputCharacters;
        public List<OutputCharacter> OutputCharacters;

        public StateMachine(List<Vertex> vertices, List<Edge> edges, 
                            List<InputCharacter> inputCharacters, List<OutputCharacter> outputCharacters)
        {
            Vertices = vertices;
            Edges = edges;
            InputCharacters = inputCharacters;
            OutputCharacters = outputCharacters;
        }

        public string Dump()
        {
            var buffer = new StringBuilder();
            foreach (var vertex in Vertices)
            {
                buffer.AppendFormat("{0}\r\n", vertex.Label);
            }

            buffer.AppendLine("");

            foreach (var edge in Edges)
            {
                buffer.AppendFormat("{0}\t{1}\t{2}\t{3}\r\n", edge.Source.Label, edge.Destination.Label, edge.In, edge.Out);
            }

            return buffer.ToString();
        }
    }
}
