using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class PersonService(ApplicationContext applicationContext) : IPersonService
{
    public async Task<Person> Get(int id,CancellationToken cancellationToken) => (await applicationContext.Persons.FindAsync(id,cancellationToken))!;

    public async Task<IEnumerable<Person>> GetAll(CancellationToken cancellationToken) => await applicationContext.Persons.ToListAsync(cancellationToken);

    public async Task Add(Person person, CancellationToken cancellationToken){
        await applicationContext.Persons.AddAsync(person, cancellationToken);
        await applicationContext.SaveChangesAsync(cancellationToken);
    }
}

public interface IPersonService
{
    Task<Person> Get(int id,CancellationToken cancellationToken);
    Task<IEnumerable<Person>> GetAll(CancellationToken cancellationToken);
    Task Add(Person person,CancellationToken cancellationToken);
}