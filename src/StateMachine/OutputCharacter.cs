namespace StateMachineGenerator
{
    internal class OutputCharacter
    {
        public string Value { get; set; }

        public OutputCharacter(char value)
        {
            Value = value.ToString();
        }

        public OutputCharacter(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
