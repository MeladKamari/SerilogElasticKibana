using Application;
using Application.Dtos;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PersonController(ILogger<HomeController> logger, IPersonService personService) : ControllerBase
{
  
    [HttpGet]
    public async Task<IEnumerable<Person>> GetAll(CancellationToken cancellationToken) =>
        await personService.GetAll(cancellationToken);

    [HttpGet]
    public async Task<Person> Get(int id, CancellationToken cancellationToken) =>
        await personService.Get(id, cancellationToken);

    [HttpPost]
    public async Task Add(PersonDto person, CancellationToken cancellationToken) =>
        await personService.Add(new Person(person.FirstName, person.LastName, person.NationalCode),
            cancellationToken);
}