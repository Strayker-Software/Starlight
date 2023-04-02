namespace Starlight.Infrastructure.IO.Interfaces
{
    public interface IOutput
    {
        void WriteLine(string value);

        void Write(string value);
    }
}