using System;

namespace MAXIMUS.Core.Libraries
{
    public class SmsInfo
    {
        private string _message;

        public string mobile_number { get; set; }
        public string message
        {
            get { return _message; }
            set
            {
                _message = value;
                if (!string.IsNullOrEmpty(_message))
                {
                    _message = Methods.StripHtml(_message);
                    if (!String.IsNullOrEmpty(_message) && _message.Length > 160)
                        _message = _message.Trim().Substring(0, 160);
                }
            }
        }
    }
    public class SmsResult
    {
        public string message { get; set; }
        public bool success  { get; set; }
    }

    public class SmsServiceResult
    {
        public string sms_id { get; set; }
        public string status_code { get; set; }
        public string request_id { get; set; }

        public override string ToString()
        {
            return string.Format("sms_id: {0}, status_code: {1}, request_id: {2}", sms_id, status_code, request_id);
        }
    }

}
