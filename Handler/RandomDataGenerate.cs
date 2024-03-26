using FakeDataGenerate.Interface;
using System;

namespace FakeDataGenerate.Handler
{
    public class RandomDataGenerate : IRandomDataGenerate
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

        public DateTime DateTimeGenerate(DateTime dateTime,int range)
        {
            Random random = new Random();
            return dateTime.AddDays(random.Next(range));
        }
    }

}
