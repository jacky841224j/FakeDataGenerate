namespace FakeDataGenerate.Interface
{
    public interface IRandomDataGenerate
    {
        string InvitationCodeGenerate();

        DateTime DateTimeGenerate(DateTime dateTime, int range);
    }
}
