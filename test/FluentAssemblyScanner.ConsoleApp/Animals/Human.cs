namespace FluentAssemblyScanner.ConsoleApp.Animals
{
    [Smokes]
    public class Human : IAnimal
    {
        [Voice]
        public bool CanFly()
        {
            return false;
        }

        public bool CanWalk()
        {
            if (IsInjured())
            {
                return false;
            }

            return true;
        }

        private bool IsInjured()
        {
            return true;
        }
    }
}