namespace CoordinatesEditor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        
        private DataGridView dataGridView1;
        private Panel drawingPanel;
        private Button btnAddRow;
        private Button btnDeleteRow;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.drawingPanel = new System.Windows.Forms.Panel();
            this.btnAddRow = new System.Windows.Forms.Button();
            this.btnDeleteRow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            
            // dataGridView1
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(300, 450);
            this.dataGridView1.TabIndex = 0;
            
            // drawingPanel
            this.drawingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawingPanel.Location = new System.Drawing.Point(300, 0);
            this.drawingPanel.Name = "drawingPanel";
            this.drawingPanel.Size = new System.Drawing.Size(500, 450);
            this.drawingPanel.TabIndex = 1;
            this.drawingPanel.BackColor = Color.White;
            
            // btnAddRow
            this.btnAddRow.Location = new System.Drawing.Point(10, 400);
            this.btnAddRow.Text = "Add Row";
            this.btnAddRow.Click += (s, e) => points.Add(new Point());
            
            // btnDeleteRow
            this.btnDeleteRow.Location = new System.Drawing.Point(100, 400);
            this.btnDeleteRow.Text = "Delete Row";
            this.btnDeleteRow.Click += (s, e) => 
            {
                if(dataGridView1.CurrentRow != null)
                    points.RemoveAt(dataGridView1.CurrentRow.Index);
            };
            
            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(btnDeleteRow);
            this.Controls.Add(btnAddRow);
            this.Controls.Add(drawingPanel);
            this.Controls.Add(dataGridView1);
            this.Name = "MainForm";
            this.Text = "Coordinate Editor";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}