using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CCWinServiceLearn
{
    public partial class Service1 : ServiceBase
    {
        #region MyRegion

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            File.AppendAllText("CCWinService.txt", "======CCWinService Start=====");
        }

        protected override void OnStop()
        {
            File.AppendAllText("CCWinService.txt", "======CCWinService Stop=====");
        }

        protected override void OnPause()
        {
            File.AppendAllText("CCWinService.txt", "======CCWinService OnPause=====");
        }

        protected override void OnContinue()
        {
            File.AppendAllText("CCWinService.txt", "======CCWinService OnContinue=====");
        }

        #endregion
    }
}
