namespace mba.BooksLibrary.Client.Exceptions {
    class ApiCallException : Exception {
        public ApiCallException() : base() { }
        public ApiCallException(string message) : base(message) { }
        public ApiCallException(string message,Exception innerException) : base(message,innerException) { 
            Console.WriteLine(message + " - " + innerException.Message);
        }
    }
}