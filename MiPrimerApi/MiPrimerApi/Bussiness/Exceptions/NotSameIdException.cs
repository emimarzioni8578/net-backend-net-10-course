namespace MiPrimerApi.Exceptions
{
    public class NotSameIdException : Exception
    {
        public NotSameIdException() { }

        public NotSameIdException(string message) : base(message) { } 
    }
}
