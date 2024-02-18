namespace TorchFilterStreams
{
    public interface IInput
    {
        string Path { get; }
        void LoadFile();
    }
}
