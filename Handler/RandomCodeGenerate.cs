using FakeDataGenerate.Interface;
using System;

namespace FakeDataGenerate.Handler
{
    public class RandomCodeGenerate : IRandomCodeGenerate
    {
        private static string _charDic = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 隨機生成邀請碼
        /// </summary>
        /// <returns></returns>
        public string InvitationCodeGenerate()
        {
            Random _rdm = new Random();

            string result = string.Empty;
            for (int i = 0; i < 7; i++)
            {
                int nextIndex = _rdm.Next(_charDic.Length);
                result += _charDic[nextIndex];
            }
            return result;
        }

        public DateTime DateTimeGenerate(int year)
        {
            Random random = new Random();
            //隨機日期範圍
            int range = 365;
            //時間年份
            DateTime startDate = new DateTime(year, 1, 1);
            return startDate.AddDays(random.Next(range));
        }
    }

}
