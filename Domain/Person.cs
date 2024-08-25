namespace Domain;

public class Person(int id, string firstName, string lastName, string nationalCode)
{
    public Person(string firstName, string lastName, string nationalCode) : this(0, firstName, lastName, nationalCode)
    {
    }

    public int Id { get; private set; } = id;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
    public string NationalCode { get; private set; } = nationalCode;
}