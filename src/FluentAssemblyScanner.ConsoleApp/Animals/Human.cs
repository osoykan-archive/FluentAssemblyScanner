namespace FluentAssemblyScanner.ConsoleApp.Animals
{
    [Smokes]
    public class Human : IAnimal
    {
        public bool CanWalk()
        {
            if (IsInjured())
            {
                return false;
            }

            return true;
        }

        public bool CanFly()
        {
            return false;
        }

        private bool IsInjured()
        {
            return true;
        }
    }
}