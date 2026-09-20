using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FileProcessorCore
{
	public abstract class BatchProcessor
	{
		public static string SourceSystem
		{
			get
			{
				string sourceSystem = EWSConfiguration.AppSettings("SourceSystem");
				if (string.IsNullOrEmpty(sourceSystem))
				{
					throw new ArgumentException("'SourceSystem' appSetting key not found.");
				}
				return sourceSystem;
			}
		}

		public static string SourceModule
		{
			get
			{
				string sourceModule = EWSConfiguration.AppSettings("SourceModule");

				if (string.IsNullOrEmpty(sourceModule))
				{
					throw new ArgumentException("'SourceModule' appSetting key not found.");
				}

				return sourceModule;
			}
		}

		public static string ApplicationName
		{
			get
			{
				return string.Format("{0} {1}", SourceSystem, SourceModule);
			}
		}

		private bool WriteLogToConsole
		{
			get
			{
				return !string.IsNullOrWhiteSpace(EWSConfiguration.AppSettings("WriteLogToConsole")) &&
					bool.Parse(EWSConfiguration.AppSettings("WriteLogToConsole"));
			}
		}

		public LogFile Log;
		protected bool HasErrors;

		#region Constructors

		public BatchProcessor()
		{
			Log = new LogFile(ApplicationName, WriteLogToConsole);
		}

		public BatchProcessor(LogFile log)
		{
			Log = log;
		}

		#endregion

		public void Run()
		{
			Log.Start();

			try
			{

				Log.Write(string.Format("Running {0}.", ApplicationName));

				Log.Write(string.Empty);

				Log.WriteVerbose("Begin checking Batch Processor Status...");
				/*
				DateTime dtLastRan;
				bool useOverrideConfig;
				string overrideConfigValues;
				
				bool runProcesor = Authorization.StartProccessor(SourceSystem, SourceModule, out dtLastRan, out useOverrideConfig, out overrideConfigValues);

				if (useOverrideConfig && !string.IsNullOrEmpty(overrideConfigValues))
				{
					EWSConfiguration.OverrideConfig(overrideConfigValues);
				}

				if (!runProcesor)
				{
					Log.Write("End checking Batch Processor Status: NOT RUNNING");

					Log.WriteError("BATCH_ACTIVITY Status is not 'C', processor will not run.");

					Log.Write("Begin sending Error email...");

					EmailHelper.SendFailureLogEmail(Log, ApplicationName, "Processor Status is not 'C', the processor will not run.");

					Log.Write("End sending Error email.");

					Log.Write("Exiting.");
					return;
				}

				Log.WriteVerbose("End checking Batch Processor Status: OK");
				*/

				Execute();

				Log.End();
				TimeSpan time = Log.TotalTime.Value;
				Log.Write(string.Format("Time: {0} Hours, {1} Minutes, {2} Seconds", time.Hours, time.Minutes, time.Seconds));
				
				if (HasErrors)
				{
					throw new Exception("Error running processor.");
				}
				else
				{
					Log.SendStatusEmail();

					// bool finished = Authorization.FinishProccessor(SourceSystem, SourceModule);
				}
			}
			catch (Exception ex)
			{
				Log.WriteError(ex.Message);
				EmailHelper.SendFailureLogEmail(Log, ApplicationName, Log.GetLog());
			}
			finally
			{
				ConnectionFactory.CloseConnection();
			}
			


		}

		public abstract void Execute();
	}
}
