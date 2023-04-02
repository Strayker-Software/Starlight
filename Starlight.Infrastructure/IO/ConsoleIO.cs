using Starlight.Infrastructure.IO.Interfaces;

namespace Starlight.Infrastructure.IO
{
    public class ConsoleIO : IInput, IOutput
    {
        public void Write(string value)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public char ReadChar()
        {
            throw new NotImplementedException();
        }

        public string ReadLine()
        {
            throw new NotImplementedException();
        }
    }
}