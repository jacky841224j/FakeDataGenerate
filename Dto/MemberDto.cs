namespace FakeDataGenerate.Dto
{
    public class MemberDto
    {
        public int pk { get; set; }

        public int agent_fk { get; set; }

        public string username { get; set; }

        public string invitation_code { get; set; }

        public string recommend { get; set; }

        public DateTime create_time { get; set; }
    }
}
