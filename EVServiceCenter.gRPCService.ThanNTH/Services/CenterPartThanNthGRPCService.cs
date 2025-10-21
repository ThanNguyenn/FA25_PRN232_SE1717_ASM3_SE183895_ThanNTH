using EVServiceCenter.gRPCService.ThanNTH.Protos;
using EVServiceCenter.Services.ThanNTH;
using Grpc.Core;
using System.Text.Json;
using System.Text.Json.Serialization;
using static EVServiceCenter.gRPCService.ThanNTH.Protos.CenterPartThanNthGRPC;

namespace EVServiceCenter.gRPCService.ThanNTH.Services;

public class CenterPartThanNthGRPCService : CenterPartThanNthGRPCBase
{
    private readonly IServiceProviders _serviceProviders;

    public CenterPartThanNthGRPCService(IServiceProviders serviceProviders)
    {
        _serviceProviders = serviceProviders;
    }

    public override async Task<CenterPartThanNthList> GetAllAsync(EmptyRequest request, ServerCallContext context)
    {
        var result = new CenterPartThanNthList();
        try
        {
            var centerParts = await _serviceProviders.CenterPartThanNthService.GetAllAsync();
            var opt = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

            var centerPartsJson = JsonSerializer.Serialize(centerParts, opt);

            var items = JsonSerializer.Deserialize<List<CenterPartThanNth>>(centerPartsJson, opt);
            result.Items.AddRange(items);
        }
        catch (Exception)
        {

            throw new Exception("Error occurred while fetching data");
        }
        return result;

    }

    public override async Task<CenterPartThanNth> GetByIdAsync(CenterPartThanNthIdRequest request, ServerCallContext context)
    {
        var result = new CenterPartThanNth();
        try
        {
            var centerPart = await _serviceProviders.CenterPartThanNthService.GetByIdAsync(request.CenterPartThanNthid  );
            var opt = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            var centerPartJson = JsonSerializer.Serialize(centerPart, opt);
            result = JsonSerializer.Deserialize<CenterPartThanNth>(centerPartJson, opt);
        }
        catch (Exception)
        {
            throw new Exception("Error occurred while fetching data");
        }
        return result;
    }

    public override async Task<MutationRelay> CreateAsync(CenterPartThanNth request, ServerCallContext context)
    {
        try
        {
            var opt = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

            var centerPartJson = JsonSerializer.Serialize(request, opt);

            var items = JsonSerializer.Deserialize<EVServiceCenter.Repositories.ThanNTH.Models.CenterPartThanNth>(centerPartJson, opt);
            var result = await _serviceProviders.CenterPartThanNthService.CreateAsync(items);
            return new MutationRelay(){ Result = result };
        }
        catch (Exception e)
        {

            throw new Exception(e.Message);
        }
        //return base.CreateAsync(request, context);
    }

    public override async Task<MutationRelay> UpdateAsync(CenterPartThanNth request, ServerCallContext context)
    {
        try
        {
            var opt = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
            var centerPartJson = JsonSerializer.Serialize(request, opt);
            var items = JsonSerializer.Deserialize<EVServiceCenter.Repositories.ThanNTH.Models.CenterPartThanNth>(centerPartJson, opt);
            var result = await _serviceProviders.CenterPartThanNthService.UpdateAsync(items);
            return new MutationRelay() { Result = result };
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public override async Task<MutationRelay> DeleteAsync(CenterPartThanNthIdRequest request, ServerCallContext context)
    {
        try
        {

            var result = await _serviceProviders.CenterPartThanNthService.DeleteAsync(request.CenterPartThanNthid);
            return new MutationRelay() { Result = result ? 1: 0 };
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }


}
