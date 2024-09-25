namespace FileExplorer1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ListView = new ListView();
            ProgressBar = new ProgressBar();
            BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            ContextMenuStrip = new ContextMenuStrip(components);
            Copy = new ToolStripMenuItem();
            Cut = new ToolStripMenuItem();
            Paste = new ToolStripMenuItem();
            NewFolder = new ToolStripMenuItem();
            TreeView = new TreeView();
            ContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // ListView
            // 
            ListView.Location = new Point(276, 12);
            ListView.Name = "ListView";
            ListView.Size = new Size(512, 426);
            ListView.TabIndex = 1;
            ListView.UseCompatibleStateImageBehavior = false;
            ListView.DoubleClick += ListView_DoubleClick;
            // 
            // ProgressBar
            // 
            ProgressBar.Location = new Point(5, 15);
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new Size(265, 25);
            ProgressBar.TabIndex = 2;
            // 
            // ContextMenuStrip
            // 
            ContextMenuStrip.Items.AddRange(new ToolStripItem[] { Copy, Cut, Paste, NewFolder });
            ContextMenuStrip.Name = "contextMenuStrip1";
            ContextMenuStrip.Size = new Size(135, 92);
            // 
            // Copy
            // 
            Copy.Name = "Copy";
            Copy.Size = new Size(134, 22);
            Copy.Text = "Copy";
            // 
            // Cut
            // 
            Cut.Name = "Cut";
            Cut.Size = new Size(134, 22);
            Cut.Text = "Cut";
            // 
            // Paste
            // 
            Paste.Name = "Paste";
            Paste.Size = new Size(134, 22);
            Paste.Text = "Paste";
            // 
            // NewFolder
            // 
            NewFolder.Name = "NewFolder";
            NewFolder.Size = new Size(134, 22);
            NewFolder.Text = "New Folder";
            // 
            // TreeView
            // 
            TreeView.Location = new Point(5, 46);
            TreeView.Name = "TreeView";
            TreeView.Size = new Size(265, 392);
            TreeView.TabIndex = 3;
            TreeView.AfterSelect += TreeView_AfterSelect;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TreeView);
            Controls.Add(ProgressBar);
            Controls.Add(ListView);
            Name = "Form1";
            Text = "Form1";
            ContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ListView ListView;
        private ProgressBar ProgressBar;
        private System.ComponentModel.BackgroundWorker BackgroundWorker;
        private ContextMenuStrip ContextMenuStrip;
        private TreeView TreeView;
        private ToolStripMenuItem Copy;
        private ToolStripMenuItem Cut;
        private ToolStripMenuItem Paste;
        private ToolStripMenuItem NewFolder;
    }
}
