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
        public NotifyIconWrapper()
        {
            InitializeComponent();

            toolStripMenuItem_Exit.Click += ToolStripMenuItem_Exit_Click;
        }

        public NotifyIconWrapper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Event handler for Exit menu click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
