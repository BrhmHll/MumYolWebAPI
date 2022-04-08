using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Integrations
{
    public static class SmsIntegration
    {
        public static IDataResult<string> SendSms(string phone, string msg)
        {
			var prmSendData = "<MainmsgBody><UserName>" + "1381B34" + "-" + "6872" + "</UserName><PassWord>" + "13813401" + "</PassWord><Action>0</Action><Mesgbody>" + msg + "</Mesgbody><Numbers>0" + phone + "</Numbers><Originator>MOMAND</Originator><SDate></SDate></MainmsgBody>";

			try
			{
				WebClient wUpload = new WebClient();
				wUpload.Proxy = null;
				Byte[] bPostArray = Encoding.UTF8.GetBytes(prmSendData);
				Byte[] bResponse = wUpload.UploadData("http://g3.iletimx.com/", "POST", bPostArray);
				Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
				string sWebPage = new string(sReturnChars);
				return new SuccessDataResult<string>(sWebPage);
			}
			catch
			{
				return new ErrorDataResult<string>();
			}
		}

		public static string GenerateCode()
        {
			Random random = new Random();
			return random.Next(999999).ToString();
        }
	}
}
