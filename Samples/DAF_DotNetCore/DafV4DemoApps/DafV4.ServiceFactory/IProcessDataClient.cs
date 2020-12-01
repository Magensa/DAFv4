using DafV4.Dtos;
using System.Threading.Tasks;

namespace DafV4.ServiceFactory
{
    public interface IProcessDataClient
    {
        Task<ProcessDataResponseDto> ProcessData(ProcessDataRequestDto processDataRequestDto);
    }
}
