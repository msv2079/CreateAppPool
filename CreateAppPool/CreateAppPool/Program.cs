using Microsoft.Web.Administration;
using System;
using System.Linq;
using System.Security.Principal;

namespace CreateAppPool
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				WindowsIdentity user = WindowsIdentity.GetCurrent();
				WindowsPrincipal principal = new WindowsPrincipal(user);

				if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
				{
					throw new Exception("O sistema deve ser executado como administrador!");
				}

				using (ServerManager serverManager = new ServerManager())
				{
					if (!serverManager.ApplicationPools.ToList().Any(x => x.Name == "MURILO_APPLICATIONPOOL"))
					{
						ApplicationPool newPool = serverManager.ApplicationPools.Add("MURILO_APPLICATIONPOOL");

						newPool.ManagedRuntimeVersion = "v4.0";
						newPool.Enable32BitAppOnWin64 = false;
						newPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;

						serverManager.CommitChanges();
						newPool.Recycle();
					}
				}

				Console.WriteLine("ApplicationPool Criado com sucesso!");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.ReadKey();
		}
	}
}
