using System.Threading.Tasks;


public class CrudServiceImpl : CrudService.CrudServiceBase
{
    private readonly ICrud _crudService;
    public CrudServiceImpl(ICrud crudService)
    {
        _crudService = crudService;
    }

    public override async Task<CrudData> GetCrudData(Empty request, ServerCallContext context)
    {
        var result = await _crudService.GetAsync(); // Assuming GetAsync returns ResultModel<Customer>
        var customer = result.Data; 

        return new CrudData
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Country = customer.Country,
        };
    }
}