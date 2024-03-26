using Dapper;
using FakeDataGenerate.Dto;
using FakeDataGenerate.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace FakeDataGenerate.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MeberController : ControllerBase
    {
        private readonly IDbConnection _dbConnection;
        private readonly IRandomDataGenerate _randomDataGenerate;

        public MeberController(IDbConnection dbConnection, IRandomDataGenerate randomDataGenerate)
        {
            _dbConnection = dbConnection;
            _randomDataGenerate = randomDataGenerate;
        }

        /// <summary>
        /// 生成客戶資料
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TimeSpan> CreateMember()
        {
            Stopwatch stopwatch = new Stopwatch();
            int insertcount = 0;

            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }

            var sqlcommend = @"
                INSERT INTO member 
                (username,
                invitation_code,
                create_time)
                VALUES(
                @username,
                @invitation_code,
                @create_time
                );";

            stopwatch.Start();
            for (int x = 0; x < 1000; x++)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@username", _randomDataGenerate.InvitationCodeGenerate(), DbType.String);
                parameters.Add("@invitation_code", _randomDataGenerate.InvitationCodeGenerate(), DbType.String);
                parameters.Add("@create_time", _randomDataGenerate.DateTimeGenerate(DateTime.Now, 365).ToString("yyyy-MM-dd hh:mm:ss"), DbType.String);
                insertcount += await _dbConnection.ExecuteAsync(sqlcommend, parameters);
            }
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }

        /// <summary>
        /// 建立推薦人資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TimeSpan> CreateInvitation()
        {
            Stopwatch stopwatch = new Stopwatch();
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }

            var sqlcommend = @"
                SELECT pk,agent_fk,username
                FROM member 
                WHERE agent_fk is null or agent_fk = '';
                ";

            //取出沒有推薦人的帳號
            var memberList = (await _dbConnection.QueryAsync<MemberDto>(sqlcommend)).ToList();

            stopwatch.Start();
            foreach (var member in memberList)
            {
                var parent = new MemberDto();
                Random random = new Random();
                var randomNum = random.Next(0, memberList.Count());

                //隨機取一筆當作推薦人
                parent = memberList.ToList()[randomNum];

                //若取到自己則重新取值
                if (member.pk == parent.pk)
                {
                    if (randomNum == 0)
                        parent = memberList.ToList()[random.Next(1, memberList.Count())];
                    else if (randomNum == memberList.Count())
                        parent = memberList.ToList()[random.Next(0, memberList.Count() - 1)];
                    else
                        parent = memberList.ToList()[random.Next(randomNum, memberList.Count() - 1)];
                }

                var parameters = new DynamicParameters();
                sqlcommend = @"UPDATE member SET agent_fk = @agent_fk , recommend = @recommend WHERE pk = @pk;";
                parameters.Add("@pk", member.pk, DbType.Int32);
                parameters.Add("@agent_fk", parent.pk, DbType.Int32);
                parameters.Add("@recommend", parent.username, DbType.String);
                await _dbConnection.ExecuteAsync(sqlcommend, parameters);
            }
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }
    }
}
