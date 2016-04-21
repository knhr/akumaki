using akumaki.Core;
using System;
using System.ComponentModel;
using System.Windows;

namespace akumaki.UI
{
    /// <summary>
    /// NotifyIcon in the TaskTray
    /// </summary>
    public partial class NotifyIconWrapper : Component
    {
        #region Constructors

        public NotifyIconWrapper()
        {
            InitializeComponent();

            toolStripMenuItem_Debug.Click += ToolStripMenuItem_Debug_Click;
            toolStripMenuItem_Exit.Click += ToolStripMenuItem_Exit_Click;
        }

        public NotifyIconWrapper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion Constructors

        #region Internal Methods

        private void ToolStripMenuItem_Debug_Click(object sender, EventArgs e)
        {
            WindowLocationStore.Test();
        }

        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion Internal Methods
    }
}
