using console = System.Console;

namespace MyFeedlyClient.Console
{
    static class ProgramExtensions
    {
        public static void Border(this Program program)
        {
            console.WriteLine(new string('-', 100));
        }

        public static void WriteAction(this Program program, string name, int key)
        {
            console.WriteLine($"{name}: {key}");
        }
    }
}