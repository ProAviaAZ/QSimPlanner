﻿using System.Windows.Forms;
using QSP.UI.Models.MsgBox;
using QSP.UI.Views;
using QSP.UI.Views.MsgBox;

namespace QSP.UI.Util
{
    public static class MsgBoxHelper
    {
        public static void ShowMessage(this Control c, string text, MessageLevel lvl,
            string caption = "")
        {
            var icon = (MsgBoxIcon)((int)lvl);
            c.ShowDialog(text, icon, caption, DefaultButton.Button1, "OK");
        }

        public static void ShowError(this Control control, string text, string caption = "")
        {
            control.ShowDialog(text, MsgBoxIcon.Error, caption, DefaultButton.Button1, "OK");
        }

        public static void ShowWarning(this Control control, string text, string caption = "")
        {
            control.ShowDialog(text, MsgBoxIcon.Warning, caption, DefaultButton.Button1, "OK");
        }

        public static void ShowInfo(this Control control, string text, string caption = "")
        {
            control.ShowDialog(text, MsgBoxIcon.Info, caption, DefaultButton.Button1, "OK");
        }

        public static MsgBoxResult ShowDialog(this Control parentControl, string text,
            MsgBoxIcon icon, string caption, DefaultButton defaultBtn, params string[] buttonTxt)
        {
            using (var frm = new MsgBoxForm())
            {
                frm.Init(text, icon, caption, defaultBtn, buttonTxt);
                var parentForm = parentControl?.SelfOrParentForm();

                if (parentForm == null)
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowInTaskbar = true;
                    frm.TopMost = true;
                    frm.ShowDialog();
                }
                else if (!parentForm.Visible ||
                    parentForm.WindowState == FormWindowState.Minimized)
                {
                    // The a form is not shown (e.g. minimized) , the Location and Size 
                    // cannot be trusted. Location can sometimes be a negative number.
                    // Use CenterScreen is better than CenterParent, which can cause 
                    // frm to show on top left corner.
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                }
                else
                {
                    frm.ShowDialog(parentForm);
                }

                return frm.SelectionResult;
            }
        }
    }
}
