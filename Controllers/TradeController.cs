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
    public class TradeController : ControllerBase
    {
        private readonly IDbConnection _dbConnection;
        private readonly IRandomDataGenerate _randomDataGenerate;

        public TradeController(IDbConnection dbConnection, IRandomDataGenerate randomDataGenerate)
        {
            _dbConnection = dbConnection;
            _randomDataGenerate = randomDataGenerate;
        }

        /// <summary>
        /// 建立交易紀錄
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TimeSpan> CreateTrade()
        {
            Stopwatch stopwatch = new Stopwatch();
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }

            //取出客戶
            var sqlcommend = @" SELECT pk FROM member; ";
            var memberList = (await _dbConnection.QueryAsync<MemberDto>(sqlcommend)).ToList();
            var usedList = new HashSet<int>();

            stopwatch.Start();
            for (int i = 0; i < 2500; i++)
            {
                Random random = new Random();
                var type = 1;
                var randomNum = random.Next(1, memberList.Count());

                if (usedList.Where(x => x == randomNum).Any()) type = 2;
                else usedList.Add(randomNum);

                var member = memberList[randomNum];
               
                //建立交易資料
                sqlcommend = @"
                INSERT INTO borrow_fee 
                (member_fk,
                type,
                borrow_fee,
                create_time)
                VALUES(
                @member_fk,
                @type,
                @borrow_fee,
                @create_time
                );";

                var parameters = new DynamicParameters();
                parameters.Add("@member_fk", member.pk, DbType.Int32);
                parameters.Add("@type", type, DbType.Int16);
                parameters.Add("@borrow_fee", random.Next(), DbType.Int32);
                parameters.Add("@create_time", _randomDataGenerate.DateTimeGenerate(DateTime.Now.AddMonths(-18), 540).ToString("yyyy-MM-dd hh:mm:ss"), DbType.String);

                await _dbConnection.ExecuteAsync(sqlcommend, parameters);
            }
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }
    }
}
