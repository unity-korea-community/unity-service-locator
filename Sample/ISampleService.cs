namespace UNKO.ServiceLocator.Sample
{
    public interface ISampleService
    {
        string ReturnString();
    }

    public class SampleServiceA : ISampleService
    {
        public string ReturnString() => "A";
    }

    public class SampleServiceB : ISampleService
    {
        public string ReturnString() => "B";
    }

    public class SampleServiceC : ISampleService
    {
        public string ReturnString() => "C";
    }

    public class SampleServiceD : ISampleService
    {
        public string ReturnString() => "D";
    }
}
