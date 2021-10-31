using System.Collections.Generic;
using System.Threading.Tasks;

namespace TL.XR.Infrastructure.Interfaces
{
    public interface IValidator<T>
    {
        /// <summary>
        /// ValidateAsync should return an empty IEnumerable if valid, a list of errors otherwise
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> ValidateAsync(T value);
    }
}
