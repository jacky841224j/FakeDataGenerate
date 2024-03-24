namespace FakeDataGenerate.Interface
{
    public interface IRandomCodeGenerate
    {
        string InvitationCodeGenerate();

        DateTime DateTimeGenerate(int year);
    }
}
