using System;
using System.Collections.Generic;

namespace MAXIMUS.DataExchange.PDMS
{
	public interface ITask
	{
		string Name { get; set; }
		void SetNextTask(ITask nextTask);
		ITask GetNextTask();
		void ConcatNextTask(ITask lastTask);
		bool HasNextTask();
		List<TaskData> Run(List<TaskData> lstResults);
		List<TaskData> Run();
		bool ExecuteTask(TaskData result);
	}

	public abstract class BaseTask : ITask
	{
		#region Private Members

		private ITask _nextTask;

		#endregion

		#region ITask Properties

		public string Name { get; set; }

		#endregion

		#region Constructors

		public BaseTask(string name)
		{
			Name = name;
		}

		#endregion

		#region ITask Methods

		public void SetNextTask(ITask nextTask)
		{
			this._nextTask = nextTask;
		}

		public bool HasNextTask()
		{
			return _nextTask != null;
		}

		public ITask GetNextTask()
		{
			return _nextTask;
		}

		public void ConcatNextTask(ITask lastTask)
		{
			ITask endTask = this;

			while (endTask.HasNextTask())
			{
				endTask = endTask.GetNextTask();
			}

			endTask.SetNextTask(lastTask);
		}

		public List<TaskData> Run()
		{
			return Run(new List<TaskData>());
		}

		public List<TaskData> Run(List<TaskData> lstResults)
		{
			TaskData result = new TaskData(Name);
			result.StartDateTime = DateTime.Now;

			result.Successful = ExecuteTask(result);

			result.EndDateTime = DateTime.Now;

			lstResults.Add(result);

			if (_nextTask != null)
			{
				lstResults = _nextTask.Run(lstResults);
			}

			return lstResults;
		}

		public abstract bool ExecuteTask(TaskData result);

		#endregion
	}
}
