using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytracer.Extensions
{
    public static class UI
    {
        /// <summary>
        /// Invoke a delegate on the UI thread.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="action"></param>
        public static void UIThread(this System.Windows.Forms.Control ctrl, Action action)
        {
            if (ctrl == null)
                throw new ArgumentNullException("ctrl");

            if (action == null)
                throw new ArgumentNullException("action");

            if (ctrl.InvokeRequired)
                ctrl.Invoke(action);
            else
                action();
        }
    }
}