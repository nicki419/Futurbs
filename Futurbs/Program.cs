namespace Futurbs
{
    public class Program
    {
        public static Game game = new();
        public static void Main()
        {
            Console.CancelKeyPress += delegate {
                // call methods to clean up
                Console.Clear();
                Console.CursorVisible = true;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Keyboard Interrupt");
            };
            game.Play();
        }
    }
}

