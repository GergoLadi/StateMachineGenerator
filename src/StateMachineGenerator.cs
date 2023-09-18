using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachineGenerator
{
    internal class StateMachineGenerator
    {
        private static readonly Random random = new Random();

        internal static StateMachine Generate(uint numStates, uint pctSelfLoop, uint pctRandomEdges, 
                                              uint pctReuseInputChar, uint pctReuseOutputChar)
        {
            var vertices = new List<Vertex>();
            var edges = new List<Edge>();
            var inputCharacters = new List<InputCharacter>();
            var outputCharacters = new List<OutputCharacter>();

            uint vertexCount = numStates;
            for (int i = 0; i < vertexCount; ++i)
            {
                vertices.Add(new Vertex("S" + i));
            }
            
            List<Vertex> reachable = new List<Vertex>();
            List<Vertex> unreachable = new List<Vertex>();

            // Initial state is always reachable, the rest are not
            reachable.Add(vertices.First()); 
            foreach (var vertex in vertices.Skip(1))
            {
                unreachable.Add(vertex);
            }

            // Round 1: Ensure that all the states are reachable
            for (int i = 0; i < unreachable.Count; ++i)
            {
                Vertex thisVertex = unreachable[i];
                Vertex randomSource = reachable[random.Next(0, reachable.Count)];

                InputCharacter input;
                OutputCharacter output;
                if (random.Next(1, 101) <= pctReuseInputChar && inputCharacters.Count > 0)
                {
                    var reusableMessageTypes = inputCharacters.Where(t => !edges.Any(e => e.In == t && e.Source == randomSource)).ToList();
                    if (!reusableMessageTypes.Any())
                    {
                        input = new InputCharacter((char) (inputCharacters.Count + 'A'));
                    }
                    else
                    {
                        input = reusableMessageTypes[random.Next(0, reusableMessageTypes.Count)];
                    }
                }
                else
                {
                    input = new InputCharacter((char) (inputCharacters.Count + 'A'));
                    inputCharacters.Add(input);
                }

                if (random.Next(1, 101) <= pctReuseOutputChar && outputCharacters.Count > 0)
                {
                    output = outputCharacters[random.Next(0, outputCharacters.Count)];
                }
                else
                {
                    output = new OutputCharacter((char) (outputCharacters.Count + 'a'));
                    outputCharacters.Add(output);
                }

                Edge newEdge = new Edge
                {
                    Source = randomSource,
                    Destination = thisVertex,
                    In = input,
                    Out = output,
                    Label = String.Format("{0} / {1} [{2}]", input, output, "RCH")
                };

                edges.Add(newEdge);
                reachable.Add(thisVertex);
            }

            // Round 2: add self loops
            foreach (var vertex in vertices)
            {
                if (random.Next(1, 101) <= pctSelfLoop)
                {
                    InputCharacter input;
                    OutputCharacter output;
                    if (random.Next(1, 101) <= pctReuseInputChar && inputCharacters.Count > 0)
                    {
                        var reusableMessageTypes = inputCharacters.Where(t => !edges.Any(e => e.In == t && e.Source == vertex)).ToList();
                        if (reusableMessageTypes.Count == 0)
                        {
                            input = new InputCharacter((char) (inputCharacters.Count + 'A'));
                        }
                        else
                        {
                            input = reusableMessageTypes[random.Next(0, reusableMessageTypes.Count)];
                        }
                    }
                    else
                    {
                        input = new InputCharacter((char) (inputCharacters.Count + 'A'));
                        inputCharacters.Add(input);
                    }

                    if (random.Next(1, 101) <= pctReuseOutputChar && outputCharacters.Count > 0)
                    {
                        output = outputCharacters[random.Next(0, outputCharacters.Count)];
                    }
                    else
                    {
                        output = new OutputCharacter((char) (outputCharacters.Count + 'a'));
                        outputCharacters.Add(output);
                    }

                    Edge newEdge = new Edge
                    {
                        Source = vertex,
                        Destination = vertex,
                        In = input,
                        Out = output,
                        Label = String.Format("{0} / {1} [{2}]", input, output, "SLF")
                    };

                    edges.Add(newEdge);
                }
            }

            // Round 3: add random edges
            foreach (var vertex in vertices)
            {
                if (random.Next(1, 101) <= pctRandomEdges)
                {
                    var possibleDestinations = vertices.Where(v => !edges.Any(e => e.Destination == v && e.Source == vertex) && v != vertex).ToList();

                    if (!possibleDestinations.Any())
                    {
                        continue;
                    }

                    InputCharacter input;
                    OutputCharacter output;
                    if (random.Next(1, 101) <= pctReuseInputChar && inputCharacters.Count > 0)
                    {
                        var reusableMessageTypes = inputCharacters.Where(t => !edges.Any(e => e.In == t && e.Source == vertex)).ToList();
                        if (reusableMessageTypes.Count == 0)
                        {
                            input = new InputCharacter((char) (inputCharacters.Count + 'A'));
                        }
                        else
                        {
                            input = reusableMessageTypes[random.Next(0, reusableMessageTypes.Count)];
                        }
                    }
                    else
                    {
                        input = new InputCharacter((char) (inputCharacters.Count + 'A'));
                        inputCharacters.Add(input);
                    }

                    if (random.Next(1, 101) <= pctReuseOutputChar && outputCharacters.Count > 0)
                    {
                        output = outputCharacters[random.Next(0, outputCharacters.Count)];
                    }
                    else
                    {
                        output = new OutputCharacter((char) (outputCharacters.Count + 'a'));
                        outputCharacters.Add(output);
                    }

                    Edge newEdge = new Edge
                    {
                        Source = vertex,
                        Destination = possibleDestinations[random.Next(0, possibleDestinations.Count)],
                        In = input,
                        Out = output,
                        Label = String.Format("{0} / {1} [{2}]", input, output, "RND")
                    };

                    edges.Add(newEdge);
                }
            }

            // Round 4: ensure that there is at least one valid transition from every state
            foreach (var vertex in vertices)
            {
                var outgoingEdges = edges.Where(e => e.Source == vertex).ToList();

                if (!outgoingEdges.Any())
                {
                    InputCharacter input;
                    OutputCharacter output;
                    if (random.Next(1, 101) <= pctReuseInputChar && inputCharacters.Count > 0)
                    {
                        input = inputCharacters[random.Next(0, inputCharacters.Count)];
                    }
                    else
                    {
                        input = new InputCharacter((char) (inputCharacters.Count + 'A'));
                        inputCharacters.Add(input);
                    }

                    if (random.Next(1, 101) <= pctReuseOutputChar && outputCharacters.Count > 0)
                    {
                        output = outputCharacters[random.Next(0, outputCharacters.Count)];
                    }
                    else
                    {
                        output = new OutputCharacter((char) (outputCharacters.Count + 'a'));
                        outputCharacters.Add(output);
                    }

                    Edge newEdge = new Edge
                    {
                        Source = vertex,
                        Destination = vertices[0],
                        In = input,
                        Out = output,
                        Label = String.Format("{0} / {1} [{2}]", input, output, "BCK")
                    };

                    edges.Add(newEdge);
                }
            }

            return new StateMachine(vertices, edges, inputCharacters, outputCharacters);
        }
    }
}
