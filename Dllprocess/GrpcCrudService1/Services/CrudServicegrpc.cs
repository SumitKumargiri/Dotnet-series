using Dllprocess.Interface;
using Dllprocess.Model;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcCrudService1
{
    public class CrudServicegrpc : Crud.CrudService.CrudServiceBase
    {
        private readonly ICrud _crudService;

        public CrudServicegrpc(ICrud crudService)
        {
            _crudService = crudService;
        }

        public override async Task<DataResponse> GetData(Empty request, ServerCallContext context)
        {
            var items = await _crudService.GetAsync();

            var response = new DataResponse();
            response.Items.AddRange(items.Select(item => new Crud
            {
                Id = item.id,
                Name = item.name,
                Email = item.email,
                Country = item.country
            }));

            return response;
        }
    }
}
