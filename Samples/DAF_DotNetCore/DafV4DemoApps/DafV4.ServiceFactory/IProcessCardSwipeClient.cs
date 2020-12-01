using DafV4.Dtos;
using System.Threading.Tasks;

namespace DafV4.ServiceFactory
{
    public interface IProcessCardSwipeClient
    {
        Task<ProcessCardSwipeResponseDto> ProcessCardSwipe(ProcessCardSwipeRequestDto dto);
    }
}
