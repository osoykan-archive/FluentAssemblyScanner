namespace FluentAssemblyScanner.ConsoleApp.Animals
{
    public class Bird : IAnimal
    {
        [Voice]
        public bool CanFly()
        {
            return true;
        }

        public bool CanWalk()
        {
            return true;
        }
    }
}