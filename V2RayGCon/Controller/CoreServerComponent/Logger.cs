﻿namespace V2RayGCon.Controller.CoreServerComponent
{
    sealed public class Logger :
        VgcApis.Models.BaseClasses.ComponentOf<CoreServerCtrl>,
        VgcApis.Models.Interfaces.CoreCtrlComponents.ILogger
    {
        VgcApis.Libs.Sys.QueueLogger qLogger =
            new VgcApis.Libs.Sys.QueueLogger();
        Service.Setting setting;

        public Logger(Service.Setting setting)
        {
            this.setting = setting;
        }

        #region public methods
        public void Log(string message)
        {
            qLogger.Log(message);
            try
            {
                setting.SendLog($"[{coreInfo.GetName()}] {message}");
            }
            catch { }
        }

        States coreInfo;
        public override void Prepare()
        {
            coreInfo = GetContainer().GetComponent<States>();
        }

        Views.WinForms.FormSingleServerLog logForm = null;
        public void ShowLogForm()
        {
            lock (logForm)
            {
                if (logForm == null)
                {
                    var title = coreInfo.GetSummary();
                    logForm = new Views.WinForms.FormSingleServerLog(title, qLogger);
                    logForm.FormClosed += (s, a) => logForm = null;
                    logForm.Show();
                }

                logForm.Activate();
            }
        }
        #endregion

        #region private methods

        #endregion

        #region protected methods
        protected override void Cleanup()
        {
            logForm?.Close();
            qLogger.Dispose();
        }
        #endregion
    }
}
