using Mod.ModHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mod.CSharpInteractive
{
    internal class CSharpInteractiveForm : Form
    {
        internal TextBox textBoxCode;
        internal SplitContainer splitContainerCodeAndOutput;
        internal SplitContainer splitContainerMain;
        internal TextBox textBoxOutput;
        internal Button buttonRun;
        internal Button buttonClearLogs;
        private GroupBox groupBoxOutput;
        private System.Windows.Forms.Panel panelOutput;
        private System.Windows.Forms.Panel panelCode;
        internal static CSharpInteractiveForm instance = new CSharpInteractiveForm();
        internal static Thread csharpInteractiveFormThread = new Thread(() => 
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            instance.ShowDialog();
        });

        static CSharpInteractiveForm()
        {
            csharpInteractiveFormThread.TrySetApartmentState(ApartmentState.STA);
            csharpInteractiveFormThread.IsBackground = true;
            csharpInteractiveFormThread.Name = "CSharpInteractive";
        }

        public CSharpInteractiveForm()
        {
            InitializeComponent();
            Width = UnityEngine.Screen.width;
            Height = UnityEngine.Screen.height;
            WindowState = FormWindowState.Maximized;
        }

        private void InitializeComponent()
        {
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.splitContainerCodeAndOutput = new System.Windows.Forms.SplitContainer();
            this.panelCode = new System.Windows.Forms.Panel();
            this.panelOutput = new System.Windows.Forms.Panel();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.buttonClearLogs = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.splitContainerCodeAndOutput.Panel1.SuspendLayout();
            this.splitContainerCodeAndOutput.Panel2.SuspendLayout();
            this.splitContainerCodeAndOutput.SuspendLayout();
            this.panelCode.SuspendLayout();
            this.panelOutput.SuspendLayout();
            this.groupBoxOutput.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxCode
            // 
            this.textBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCode.BackColor = System.Drawing.Color.Black;
            this.textBoxCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCode.ForeColor = System.Drawing.Color.White;
            this.textBoxCode.Location = new System.Drawing.Point(1, 1);
            this.textBoxCode.Multiline = true;
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(646, 300);
            this.textBoxCode.TabIndex = 0;
            this.textBoxCode.Text = "GameScr.info1.addInfo(\"Hello World!\", 0);";
            // 
            // splitContainerCodeAndOutput
            // 
            this.splitContainerCodeAndOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCodeAndOutput.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerCodeAndOutput.Location = new System.Drawing.Point(0, 0);
            this.splitContainerCodeAndOutput.Name = "splitContainerCodeAndOutput";
            this.splitContainerCodeAndOutput.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerCodeAndOutput.Panel1
            // 
            this.splitContainerCodeAndOutput.Panel1.Controls.Add(this.panelCode);
            // 
            // splitContainerCodeAndOutput.Panel2
            // 
            this.splitContainerCodeAndOutput.Panel2.Controls.Add(this.panelOutput);
            this.splitContainerCodeAndOutput.Size = new System.Drawing.Size(648, 414);
            this.splitContainerCodeAndOutput.SplitterDistance = 302;
            this.splitContainerCodeAndOutput.TabIndex = 1;
            // 
            // panelCode
            // 
            this.panelCode.BackColor = System.Drawing.Color.White;
            this.panelCode.Controls.Add(this.textBoxCode);
            this.panelCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCode.Location = new System.Drawing.Point(0, 0);
            this.panelCode.Name = "panelCode";
            this.panelCode.Size = new System.Drawing.Size(648, 302);
            this.panelCode.TabIndex = 1;
            // 
            // panelOutput
            // 
            this.panelOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.panelOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOutput.Controls.Add(this.groupBoxOutput);
            this.panelOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOutput.ForeColor = System.Drawing.Color.White;
            this.panelOutput.Location = new System.Drawing.Point(0, 0);
            this.panelOutput.Name = "panelOutput";
            this.panelOutput.Size = new System.Drawing.Size(648, 108);
            this.panelOutput.TabIndex = 1;
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Controls.Add(this.textBoxOutput);
            this.groupBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxOutput.ForeColor = System.Drawing.Color.White;
            this.groupBoxOutput.Location = new System.Drawing.Point(0, 0);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(646, 106);
            this.groupBoxOutput.TabIndex = 2;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output";
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.BackColor = System.Drawing.Color.Black;
            this.textBoxOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOutput.ForeColor = System.Drawing.Color.White;
            this.textBoxOutput.Location = new System.Drawing.Point(3, 18);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.Size = new System.Drawing.Size(640, 85);
            this.textBoxOutput.TabIndex = 1;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerMain.IsSplitterFixed = true;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerCodeAndOutput);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.buttonClearLogs);
            this.splitContainerMain.Panel2.Controls.Add(this.buttonRun);
            this.splitContainerMain.Size = new System.Drawing.Size(763, 414);
            this.splitContainerMain.SplitterDistance = 648;
            this.splitContainerMain.TabIndex = 2;
            // 
            // buttonClearLogs
            // 
            this.buttonClearLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearLogs.Location = new System.Drawing.Point(11, 46);
            this.buttonClearLogs.Name = "buttonClearLogs";
            this.buttonClearLogs.Size = new System.Drawing.Size(88, 30);
            this.buttonClearLogs.TabIndex = 1;
            this.buttonClearLogs.Text = "Clear logs";
            this.buttonClearLogs.UseVisualStyleBackColor = true;
            this.buttonClearLogs.Click += new System.EventHandler(this.buttonClearLogs_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRun.Location = new System.Drawing.Point(11, 10);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(88, 30);
            this.buttonRun.TabIndex = 0;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // CSharpInteractiveForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.ClientSize = new System.Drawing.Size(763, 414);
            this.Controls.Add(this.splitContainerMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "CSharpInteractiveForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Mono C# Interactive";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.CSharpInteractiveForm_Load);
            this.ResizeEnd += new System.EventHandler(this.CSharpInteractiveForm_ResizeEnd);
            this.splitContainerCodeAndOutput.Panel1.ResumeLayout(false);
            this.splitContainerCodeAndOutput.Panel2.ResumeLayout(false);
            this.splitContainerCodeAndOutput.ResumeLayout(false);
            this.panelCode.ResumeLayout(false);
            this.panelCode.PerformLayout();
            this.panelOutput.ResumeLayout(false);
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        internal static void Log(object obj)
        {
            //instance.Invoke(() =>
            //{
                string msg = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] " + obj.ToString();
                if (!msg.EndsWith(Environment.NewLine))
                    msg += Environment.NewLine;
                instance.textBoxOutput.Text += msg;
            //});
        }

        internal static void ShowForm()
        {
            if (csharpInteractiveFormThread.IsAlive)
            {
                instance.BringToFront();
                return;
            }
            try
            {
                csharpInteractiveFormThread.Start();
            }
            catch (ThreadStateException)
            {
                csharpInteractiveFormThread = new Thread(() =>
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    instance.ShowDialog();
                });
                csharpInteractiveFormThread.Start();
            }
        }

        internal static void CloseForm()
        {
            if (csharpInteractiveFormThread.IsAlive)
                csharpInteractiveFormThread.Abort();
            instance.Close();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            textBoxCode.Text = textBoxCode.Text.Trim().TrimEnd('\r', '\n', '\t');
            if (textBoxCode.Text.IndexOf('\n') == -1 && !textBoxCode.Text.EndsWith(";"))
                textBoxCode.Text += ';';
            MainThreadDispatcher.dispatcher(() => MonoInteractiveCodeExecutor.RunInteractiveCode(textBoxCode.Text));
        }

        private void buttonClearLogs_Click(object sender, EventArgs e)
        {
            textBoxOutput.Text = "";
        }

        private void CSharpInteractiveForm_ResizeEnd(object sender, EventArgs e)
        {
            Refresh();
        }

        private void CSharpInteractiveForm_Load(object sender, EventArgs e)
        {

        }
    }
}
