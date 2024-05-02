namespace FlexPortManagerPoC
{
    public interface IUserStop
    {
        bool Stop { get; set; }
    }

    public interface IUserPay
    {
        bool Pay { get; set; }
    }

    public interface IUserDuration
    {
        int Duration { get; set; }

    }

    public interface IUserProgram
    {
        public int Program { get; set; }

    }
}
