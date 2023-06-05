using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class Main : Form
    {
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        public bool IsMaximized 
        { 
            get
            {
                return Settings.Default.IsMaximized;
            }
            set
            {
                Settings.Default.IsMaximized = value;
            }
        }
        public Main()
        {
            InitializeComponent();

            RefreshDiary();

            SetColumnsHeader();

            if(IsMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }

        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();
            dgvDiary.DataSource = students;
        }

        private void SetColumnsHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Grupa";
            dgvDiary.Columns[2].HeaderText = "Imię";
            dgvDiary.Columns[3].HeaderText = "Nazwisko";
            dgvDiary.Columns[4].HeaderText = "Uwagi";
            dgvDiary.Columns[5].HeaderText = "Matematyka";
            dgvDiary.Columns[6].HeaderText = "Technologia";
            dgvDiary.Columns[7].HeaderText = "Fizyka";
            dgvDiary.Columns[8].HeaderText = "Język polski";
            dgvDiary.Columns[9].HeaderText = "Język obcy";
            dgvDiary.Columns[10].HeaderText = "Zajęcia dodatkowe";
            dgvDiary.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();

        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego dane chcesz edytować.");
                return;
            }
            var addEditStudent = new AddEditStudent(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));

            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego chcesz usunąć.");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];
            
            var confirmDelete = MessageBox.Show($"Czy na pewno chcesz usunąć ucznia " +
                $"{(selectedStudent.Cells[1].Value + " " + selectedStudent.Cells[2].Value).Trim()}",
                "Usuwanie ucznia",
                MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                await DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }
        }

        private async Task DeleteStudent(int id)
        {
            await Task.Run(() =>
            {
                var students = _fileHelper.DeserializeFromFile();
                students.RemoveAll(x => x.Id == id);
                _fileHelper.SerializeToFile(students);
            });
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void btnAdd_MouseEnter(object sender, EventArgs e)
        {
            btnAdd.BackColor = System.Drawing.Color.Aqua;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsMaximized = WindowState == FormWindowState.Maximized;

            Settings.Default.Save();
        }
    }
}
