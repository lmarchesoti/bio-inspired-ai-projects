namespace Visual_Solutions_Plotter {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public DataPlotter.Plotter Map = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Map = new DataPlotter.Plotter();
            this.SuspendLayout();
            // 
            // Map
            // 
            this.Map.AutoSize = true;
            this.Map.BorderBottom = 30;
            this.Map.BorderLeft = 30;
            this.Map.BorderRight = 20;
            this.Map.BorderTop = 20;
            this.Map.ColorAxis = System.Drawing.Color.Black;
            this.Map.ColorBg = System.Drawing.Color.White;
            this.Map.ColorDraw = System.Drawing.Color.DarkBlue;
            this.Map.ColorGrid = System.Drawing.Color.LightGray;
            this.Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Map.DrawMode = DataPlotter.Plotter.DrawModeType.Dot;
            this.Map.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Map.FontAxis = new System.Drawing.Font("Arial", 8F);
            this.Map.Location = new System.Drawing.Point(0, 0);
            this.Map.Name = "Map";
            this.Map.PenWidth = 4;
            this.Map.Size = new System.Drawing.Size(584, 562);
            this.Map.TabIndex = 0;
            this.Map.XData = null;
            this.Map.XGrid = 10D;
            this.Map.XLogBase = 0;
            this.Map.XRangeEnd = 100D;
            this.Map.XRangeStart = 0D;
            this.Map.YData = null;
            this.Map.YGrid = 10D;
            this.Map.YLogBase = 0;
            this.Map.YRangeEnd = 100D;
            this.Map.YRangeStart = 0D;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 562);
            this.Controls.Add(this.Map);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Genetic Algorithm Solution Plotter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}

