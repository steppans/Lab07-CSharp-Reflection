namespace Animals
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public class MyAttribute: Attribute
    {
        private readonly string _comment;

        public MyAttribute(string comment) => _comment = comment;

        public string Comment => _comment;
    
    }
}
