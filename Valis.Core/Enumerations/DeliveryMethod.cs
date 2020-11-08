
namespace Valis.Core
{
    public enum DeliveryMethod : byte
    {
        All = 0,
        AllResponded = 1,
        NotResponded = 2,
        NewAndUnsent = 3,
        Custom = 4
    }
}
