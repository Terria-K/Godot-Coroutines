namespace Godot.Coroutines
{
    public sealed class WaitForInput : YieldInstruction
    {
        private readonly string inputName;
        public WaitForInput(string inputName)
        {
            this.inputName = inputName;
        }

        public override bool Condition => !Input.IsActionJustPressed(inputName);
    }
}

