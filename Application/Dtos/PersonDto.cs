namespace Application.Dtos;

public class PersonDto(string firstName, string lastName, string nationalCode)
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string NationalCode { get; set; } = nationalCode;
}