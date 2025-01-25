#nullable enable
namespace SmallTests.Entities
{
    public class Person 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Person()
        {
                
        }

        public Person(string name)
        {
            Name = name;
        }

    }
}
