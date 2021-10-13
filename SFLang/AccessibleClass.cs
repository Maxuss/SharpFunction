namespace SFLang
{
    public class AccessibleClass
    {
        public string Field { get; set; }

        // Empty 
        public AccessibleClass()
        {
            
        }

        public AccessibleClass(string value)
        {
            Field = value;
        }
    }
}