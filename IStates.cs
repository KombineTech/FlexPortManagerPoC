using Flex;

namespace FlexPortManagerPoC
{
    public interface IStateProtocol
    {

        [StorageLink(eState.ProtocolIn)]
        string ProtocolIn { get; set; }
        [StorageLink(eState.ProtocolOut)]
        string ProtocolOut { get; set; }
    }
}
