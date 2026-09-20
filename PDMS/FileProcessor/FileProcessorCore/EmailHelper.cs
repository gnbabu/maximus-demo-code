using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;

using System.Text.RegularExpressions;

namespace FileProcessorCore
{
	public static class EmailHelper
	{
		private static Regex CONNECTION_STRING_REGEX = new Regex(@"Data Source=(?<server>\w*);.*Initial Catalog=(?<database>\w*);", RegexOptions.IgnoreCase);

		public static string ErrorEmailRecipients
		{
			get
			{
				if (string.IsNullOrEmpty(EWSConfiguration.AppSettings("ErrorEmailRecipients")))
				{
					throw new ArgumentException("'ErrorEmailRecipients' appSetting key not found.");
				}
				return EWSConfiguration.AppSettings("ErrorEmailRecipients");
			}
		}

		public static string EmailSender
		{
			get
			{
				if (string.IsNullOrEmpty(EWSConfiguration.AppSettings("EmailSender")))
				{
					throw new ArgumentException("'EmailSender' appSetting key not found.");
				}
				return EWSConfiguration.AppSettings("EmailSender");
			}
		}

		public static bool SendEmail(LogFile log, string toRecipients, string subject, string textBody, string htmlBody)
		{
			string from = EmailSender;
			
			return SendMail(log, from, toRecipients, null, null, subject, htmlBody, !string.IsNullOrWhiteSpace(htmlBody), textBody, !string.IsNullOrWhiteSpace(textBody));
		}

		public static bool SendEmail(LogFile log, string toRecipients, string bccRecipients, string subject, string textBody, string htmlBody)
		{
			string from = EmailSender;

			return SendMail(log, from, toRecipients, null, bccRecipients, subject, htmlBody, !string.IsNullOrWhiteSpace(htmlBody), textBody, !string.IsNullOrWhiteSpace(textBody));
		}

		public static bool SendSuccessLogEmail(LogFile log, string applicationName, string logText)
		{
			string from = EmailSender;
			string to = ErrorEmailRecipients;
			
			string location = GetApplicationLocation();
			string subject = string.Format("SUCCESS: {0} {1}", location, applicationName);
			

			string body = string.Format("The {3} has finished running at {2}{0}{0}LOG:{0}{0}{1}",
				Environment.NewLine, logText, DateTime.Now, applicationName);

			return SendMail(log, from, to, null, null, subject, body, false, body, true);
		}

		public static bool SendFailureLogEmail(LogFile log, string applicationName, string errorMessage)
		{
			string from = EmailSender;
			string to = ErrorEmailRecipients;
			
			string location = GetApplicationLocation();
			string subject = string.Format("FAILURE: {0} {1}", location, applicationName);
			

			string body = string.Format("The {2} has failed:{0}{0}{1}{0}{0}NOTE: The processor will not run until the Status in BATCH_ACTIVITY table is reset to 'C'.",
				Environment.NewLine, errorMessage, applicationName);
			
			return SendMail(log, from, to, null, null, subject, body, false, body, true);
		}

		private static string GetApplicationLocation()
		{
			string location = "";

			var connectionString = ConfigurationManager.ConnectionStrings[ConnectionFactory.CONNECTION_STRING_CONFIG_KEY].ConnectionString;
			if (!string.IsNullOrEmpty(connectionString))
			{
				var res = CONNECTION_STRING_REGEX.Match(connectionString);

				if (res.Success)
				{
					if (res.Groups["server"] != null && res.Groups["database"] != null)
					{
						location = string.Format("[{0}.{1}]", res.Groups["server"].Value, res.Groups["database"].Value);
					}
					else if (res.Groups["server"] != null)
					{
						location = string.Format("[Server:{0}]", res.Groups["server"].Value);
					}
					else if (res.Groups["database"] != null)
					{
						location = string.Format("[Database:{0}]", res.Groups["database"].Value);
					}
				}
			}

			return location;
		}

		private static bool SendMail(LogFile log, string from, string to, string cc, string bcc, string subject, string htmlBody, bool useHTMLFormat,
                                    string plainTextBody, bool usePlainTextFormat)
        {
			try
			{
				MailMessage msg = new MailMessage();
				SmtpClient smtpclient;

				msg.From = new MailAddress(from);

				msg.To.Add(new MailAddress(to));
				msg.Subject = subject;


				if (!string.IsNullOrWhiteSpace(cc))
				{
					msg.CC.Add(new MailAddress(cc));
				}

				if (!string.IsNullOrWhiteSpace(bcc))
				{
					msg.Bcc.Add(new MailAddress(bcc));
				}

				if (useHTMLFormat)
				{
					msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html"));
				}

				if (usePlainTextFormat)
				{
					msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plainTextBody, null, "text/plain"));
				}


				smtpclient = new SmtpClient();

				smtpclient.Send(msg);

				msg = null;
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				Exception exTemp = ex;

				sb.Append(exTemp.Message);

				while (exTemp.InnerException != null)
				{
					exTemp = exTemp.InnerException;
					sb.Append(exTemp.Message);
				}

				log.WriteError(string.Format("Error sending email to '{0}': {1}", to, sb.ToString()));

				return false;
			}

			return true;
        }
	}
}
