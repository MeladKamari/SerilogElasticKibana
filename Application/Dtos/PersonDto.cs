namespace Application.Dtos;

public class PersonDto(string firstName, string lastName, string nationalCode)
{
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
    public string NationalCode { get; private set; } = nationalCode;
}