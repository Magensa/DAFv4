using DafV4.Dtos;
using System.Threading.Tasks;

namespace DafV4.ServiceFactory
{
    public interface IProcessTokenClient
    {
        Task<ProcessTokenResponseDto> ProcessToken(ProcessTokenRequestDto dto);
    }
}
