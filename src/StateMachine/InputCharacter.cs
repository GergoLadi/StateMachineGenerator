namespace StateMachineGenerator
{
    internal class InputCharacter
    {
        public string Value { get; set; }

        public InputCharacter(char value)
        {
            Value = value.ToString();
        }

        public InputCharacter(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
