using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FileExplorer1
{
    public partial class Form1 : Form
    {
        private string currentPath = string.Empty;  // Lưu đường dẫn hiện tại
        private string sourcePath = string.Empty;   // Đường dẫn tệp/thư mục được cut/copy
        private bool isCut = false;                 // Đánh dấu xem là cut hay copy
        public Form1()
        {
            InitializeComponent();
            this.ListView.DoubleClick += new System.EventHandler(this.ListView_DoubleClick);

            InitializeFileExplorer();
        }



        private void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var node = e.Node;
            node.Nodes.Clear(); // Xóa dummy node

            string path = (string)node.Tag;
            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    var subNode = new TreeNode(Path.GetFileName(dir))
                    {
                        Tag = dir
                    };
                    subNode.Nodes.Add("Loading...");
                    node.Nodes.Add(subNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void LoadDirectoryContents(string path)
        {
            ListView.Items.Clear();  // Xóa các item hiện tại
            currentPath = path;

            try
            {
                // Load thư mục con
                foreach (var dir in Directory.GetDirectories(path))
                {
                    var item = new ListViewItem(Path.GetFileName(dir));
                    item.Tag = dir;  // Gắn đường dẫn vào Tag để xử lý sau
                    ListView.Items.Add(item);
                }

                // Load file trong thư mục
                foreach (var file in Directory.GetFiles(path))
                {
                    var item = new ListViewItem(Path.GetFileName(file));
                    item.Tag = file;  // Gắn đường dẫn vào Tag
                    ListView.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string path = (string)e.Node.Tag;
            LoadDirectoryContents(path);
        }
        private void ListView_ItemActivate(object sender, EventArgs e)
        {
            if (ListView.SelectedItems.Count > 0)
            {
                string path = (string)ListView.SelectedItems[0].Tag;
                if (Directory.Exists(path))
                {
                    LoadDirectoryContents(path);  // Nếu là thư mục, load nội dung
                }
                else
                {
                    System.Diagnostics.Process.Start(path);  // Nếu là file, mở file
                }
            }
        }
        private void Cut_Click(object sender, EventArgs e)
        {
            if (ListView.SelectedItems.Count > 0)
            {
                sourcePath = (string)ListView.SelectedItems[0].Tag;
                isCut = true;  // Đánh dấu là cut
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (ListView.SelectedItems.Count > 0)
            {
                sourcePath = (string)ListView.SelectedItems[0].Tag;
                isCut = false;  // Đánh dấu là copy
            }
        }

        private async Task CopyFileAsync(string source, string destination)
        {
            using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                using (FileStream destStream = new FileStream(destination, FileMode.CreateNew, FileAccess.Write))
                {
                    await sourceStream.CopyToAsync(destStream);  // Copy file bất đồng bộ
                }
            }
        }

        private void DirectoryCopy(string sourceDir, string destDir, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destDir);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDir, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDir, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        private async void Paste_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(sourcePath))
            {
                string destPath = Path.Combine(currentPath, Path.GetFileName(sourcePath));

                if (File.Exists(sourcePath))  // Nếu là file
                {
                    await CopyFileAsync(sourcePath, destPath);
                }
                else if (Directory.Exists(sourcePath))  // Nếu là thư mục
                {
                    DirectoryCopy(sourcePath, destPath, true);
                }

                if (isCut)  // Nếu là cut thì xóa file/thư mục gốc
                {
                    if (File.Exists(sourcePath))
                        File.Delete(sourcePath);
                    else if (Directory.Exists(sourcePath))
                        Directory.Delete(sourcePath, true);
                }

                LoadDirectoryContents(currentPath);  // Refresh lại ListView
            }
        }

        private void NewFolder_Click(object sender, EventArgs e)
        {
            string newFolderPath = Path.Combine(currentPath, "New Folder");
            Directory.CreateDirectory(newFolderPath);
            LoadDirectoryContents(currentPath);  // Refresh lại ListView
        }

        private void InitializeFileExplorer()
        {
            // Khởi tạo TreeView với danh sách các ổ đĩa
            foreach (var drive in DriveInfo.GetDrives())
            {
                var node = new TreeNode(drive.Name)
                {
                    Tag = drive.Name
                };
                node.Nodes.Add("Loading...");  // Dummy node
                TreeView.Nodes.Add(node);
            }

            // Sự kiện mở rộng TreeView (khi người dùng click mở ổ đĩa hoặc thư mục)
            TreeView.BeforeExpand += TreeView_BeforeExpand;
            TreeView.AfterSelect += TreeView_AfterSelect;

            // Double-click để mở file hoặc thư mục trong ListView
            ListView.ItemActivate += ListView_ItemActivate;

            // Khởi tạo Context Menu
            ContextMenuStrip.Items.Add("Cut", null, Cut_Click);
            ContextMenuStrip.Items.Add("Copy", null, Copy_Click);
            ContextMenuStrip.Items.Add("Paste", null, Paste_Click);
            ContextMenuStrip.Items.Add("New Folder", null, NewFolder_Click);
            ListView.ContextMenuStrip = ContextMenuStrip;
        }

        private void LoadDirectory(string path)
        {
            try
            {
                ListView.Items.Clear();
                currentPath = path;

                // Thêm các thư mục con vào listView
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (var subDir in dir.GetDirectories())
                {
                    ListView.Items.Add(new ListViewItem(subDir.Name) { ImageIndex = 0 });
                }

                // Thêm các file vào listView
                foreach (var file in dir.GetFiles())
                {
                    ListView.Items.Add(new ListViewItem(file.Name) { ImageIndex = 1 });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải thư mục: " + ex.Message);
            }
        }

        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            // Lấy file được chọn trong listView
            if (ListView.SelectedItems.Count > 0)
            {
                string selectedFile = ListView.SelectedItems[0].Tag.ToString(); // Lấy đường dẫn file từ Tag

                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(selectedFile)
                    {
                        UseShellExecute = true // Mở file với ứng dụng mặc định của hệ điều hành
                    };
                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở file: " + ex.Message);
                }
            }
            if (ListView.SelectedItems.Count > 0)
            {
                // Lấy đường dẫn đầy đủ của mục được chọn
                string selectedFile = Path.Combine(currentPath, ListView.SelectedItems[0].Text);

                // Kiểm tra xem mục được chọn là file hay thư mục
                if (File.Exists(selectedFile))
                {
                    // Mở file bằng ứng dụng mặc định
                    try
                    {
                        System.Diagnostics.Process.Start(selectedFile);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể mở file: " + ex.Message);
                    }
                }
                else if (Directory.Exists(selectedFile))
                {
                    // Nếu là thư mục, tải nội dung của thư mục
                    LoadDirectory(selectedFile);
                }
            }
        }
    }
}

